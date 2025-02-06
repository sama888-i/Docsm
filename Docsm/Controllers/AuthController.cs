﻿using AutoMapper;
using Docsm.DTOs.AuthDtos;
using Docsm.Exceptions;
using Docsm.Helpers;
using Docsm.Helpers.Enums;
using Docsm.Models;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;


namespace Docsm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        UserManager<User> _userManager,
        SignInManager<User> _signInManager,
        IMapper _mapper ,
        jwtTokens  _jwtTokens,
        IEmailService _service
        
        ) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult>Register(RegisterDto dto)
        {
            var user=await _userManager.Users.Where(x=>x.UserName==dto.UserName ||x.Email==dto.Email).FirstOrDefaultAsync();
            if (user != null)
            {
                if (user.UserName == dto.UserName)
                    throw new ExistException("Username already using by another user");
                else
                    throw new ExistException("This email is already registered");
            }
            user=_mapper.Map<User>(dto);
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(user, nameof(Roles.User));
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _service.SendConfirmEmailAsync(user.Email,  token);
            return Ok(new{ Message = "User registered successfully"});
        }


        [HttpPost("login")]
        public async Task<IActionResult>Login(LoginDto dto)
        {
            var user = await _userManager.Users.Where(x => x.UserName == dto.UsernameOrEmail  || x.Email == dto.UsernameOrEmail ).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException<User>();
            }
            var result = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);
            if (!result.Succeeded)
            {
                if (result.IsNotAllowed)
                    ModelState.AddModelError("", "You must confirm your account");
                if (result.IsLockedOut)
                    ModelState.AddModelError("", "Wait untill" + user.LockoutEnd!.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                return Unauthorized(new { message = "Username or Password is incorrect" });
            }
            var token = await _jwtTokens.GenerateJwtToken(user);
            return Ok(token);
        }



        [HttpGet("EmailConfirm")]
        public async Task<IActionResult> EmailConfirm(string email,string token)
        {

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Məlumatlar düzgün deyil.");
            }           
            var user = await _userManager.FindByEmailAsync(email);
            if (email == null)
            {
                throw new NotFoundException<User>();
            }           
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok("Email təsdiqləndi.");
            }
            return BadRequest("Token düzgün deyil.");

        }
    }
}
