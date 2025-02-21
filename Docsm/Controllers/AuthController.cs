using AutoMapper;
using Docsm.DataAccess;
using Docsm.DTOs.AuthDtos;
using Docsm.Exceptions;
using Docsm.Helpers;
using Docsm.Helpers.Enums;
using Docsm.Helpers.Enums.Status;
using Docsm.Models;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;


namespace Docsm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        ApoSystemDbContext _context,
        UserManager<User> _userManager,
        SignInManager<User> _signInManager,
        IMapper _mapper ,
        jwtTokens  _jwtTokens,
        IEmailService _service,
        IMemoryCache _cache
        
        ) : ControllerBase
    {
        [HttpPost("RegisterForPatient")]
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
            await _userManager.AddToRoleAsync(user, nameof(Roles.Patient ));
            await _service.SendConfirmEmailAsync(user.Email!);
            return Ok(new{ Message = "User registered successfully"});
        }
        [HttpPost("RegisterForDoctor")]
        public async Task<IActionResult> DoctorRegister(DoctorRegisterDto dto)
        {
            var user = await _userManager.Users.Where(x => x.UserName == dto.UserName || x.Email == dto.Email).FirstOrDefaultAsync();
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
            
            await _userManager.AddToRoleAsync(user, nameof(Roles.Doctor ));   
            
            await _context.SaveChangesAsync();
           
            await _service.SendConfirmEmailAsync(user.Email!);
            return Ok(new { Message = "User registered successfully" });


        }


        [HttpPost("login")]
        public async Task<IActionResult>Login(LoginDto dto)
        {
            var user = await _userManager.Users.Where(x => x.UserName == dto.UsernameOrEmail  || x.Email == dto.UsernameOrEmail ).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException<User>();
            }
            var result = await _signInManager.PasswordSignInAsync(user, dto.Password, dto.RememberMe , false);
            if (!result.Succeeded)
            {
                if (result.IsNotAllowed)
                   throw new BadRequestException( "You must confirm your account");
                if (result.IsLockedOut)
                   throw new BadRequestException("Wait until " + user.LockoutEnd!.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                return Unauthorized(new { message = "Username or Password is incorrect" });
            }         
            var token = await _jwtTokens.GenerateJwtToken(user);
            return Ok(token);
        }



        [HttpGet("EmailConfirm")]
        public async Task<IActionResult> EmailConfirm(string email,int code)
        {

            if (string.IsNullOrEmpty(email) ||code<=0)
            {
                return BadRequest("Məlumatlar düzgün deyil.");
            }
            if (!_cache.TryGetValue(email, out int cachedCode) || cachedCode != code)
            {
                throw new CodeInvalidException();
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (email == null)
            {
                throw new NotFoundException<User>();
            }
            user!.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            _cache.Remove(email);
            return Ok("Email tesdiqlendi");
            

        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult>ForgotPassword(ForgotPasswordDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email)) return BadRequest();
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                throw new NotFoundException<User>();
            }
            await _service.SendResetPasswordAsync(user.Email);
            return Ok("Emaile sifrenin sifirlanmasi ucun kod gonderildi");
        }



        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email) || dto.Code <= 0 || string.IsNullOrEmpty(dto.NewPassword))
            {
                return BadRequest("Məlumatlar düzgün deyil.");
            }

            if (!_cache.TryGetValue(dto.Email, out int cachedCode)||cachedCode!=dto.Code)
            {
                throw new CodeInvalidException();
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                throw new NotFoundException<User>();
            }
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user,resetToken, dto.NewPassword);
            if (result.Succeeded)
            {
                _cache.Remove(dto.Email);
                return Ok("Şifrə uğurla sıfırlandı.");
            }

            return BadRequest("Şifrə sıfırlama uğursuz oldu.");
        }
        
    }
}
