﻿using Docsm.Options;
using Docsm.Services.Implements;
using Docsm.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System.Text;

namespace Docsm
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddJwtOptions(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<JwtOptions>(Configuration.GetSection("JwtOptions"));
            return services;
        }
        public static IServiceCollection AddSmtpOptions(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<SmtpOptions>(Configuration.GetSection(SmtpOptions.Name));
            return services;
        }
        public static IServiceCollection AddStripeOptions(this IServiceCollection services, IConfiguration Configuration)
        {
            var stripeSettings = Configuration.GetSection("Stripe");
            services.Configure<StripeOptions>(stripeSettings);

            StripeConfiguration.ApiKey = stripeSettings["SecretKey"];

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
           
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISpecialtyService, SpecialtyService>();
            services.AddScoped<IDoctorService , DoctorService>();
            services.AddScoped<ITimeScheduleService ,TimeScheduleService>();
            services.AddScoped<IPatientService ,PatientService>();
            services.AddScoped<IAppointmentService ,AppointmentService>();
            services.AddScoped<IReviewService, Docsm.Services.Implements.ReviewService>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddMemoryCache();
            return services;
        }



        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration Configuration)
        {
            JwtOptions jwtOpt = new JwtOptions();
            jwtOpt.Issuer = Configuration.GetSection("JwtOptions")["Issuer"]!;
            jwtOpt.Audience = Configuration.GetSection("JwtOptions")["Audience"]!;
            jwtOpt.SecretKey = Configuration.GetSection("JwtOptions")["SecretKey"]!;
            var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOpt.SecretKey));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signInKey,
                        ValidAudience = jwtOpt.Audience,
                        ValidIssuer = jwtOpt.Issuer,
                        ClockSkew = TimeSpan.Zero
                        

                    };

                });
            return services;
        }
    }
}
