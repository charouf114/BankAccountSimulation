using Application.Common.Interfaces.Persistance;
using Application.Common.Interfaces.Services;
using Infrastructure.Identity;
using Infrastructure.Persistance.Repositories;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("BankDB"),
              builder => builder.MigrationsAssembly(typeof(ApplicationDBContext).Assembly.FullName)));

            services.AddScoped<IApplicationDBContext>(provider => provider.GetRequiredService<ApplicationDBContext>());

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();


            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);
            services.AddSingleton(Options.Create(jwtSettings));
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                });


            return services;

        }
    }
}