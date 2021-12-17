using AutoAukcionas.Auth;
using AutoAukcionas.Data;
using AutoAukcionas.Data.DTOs.Auth;
using AutoAukcionas.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAukcionas
{
    public class Startup
    {
        private readonly IConfiguration _cofiguration;
        public Startup(IConfiguration configuration)
        {
            _cofiguration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<AuctionUser, IdentityRole>()
                .AddEntityFrameworkStores<AuctionContext>()
                .AddDefaultTokenProviders();

            services.AddDirectoryBrowser();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters.ValidAudience = _cofiguration["JWT:ValidAudience"];
                    options.TokenValidationParameters.ValidIssuer = _cofiguration["JWT:ValidIssuer"];
                    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cofiguration["JWT:Secret"]));
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("SameUser", policy => policy.Requirements.Add(new SameUserRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, SameUserAuthorization>();
            

            services.AddDbContext<AuctionContext>();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();

            services.AddTransient<ICountryRepository, CountryRepository>();
            services.AddTransient<ICarRepository, CarRepository>();
            services.AddTransient<IBetRepository, BetRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddTransient<ITokenManager, TokenManager>();
            services.AddTransient<IRefreshToken, RefreshToken>();
            services.AddTransient<DatabaseSeeder, DatabaseSeeder>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
            });

            services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
            //services.AddSingleton<IRefreshToken>(x => new RefreshToken(x.GetService<IConfiguration>(), x.GetService<IRefreshTokenRepository>(), x.GetService<ITokenManager>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseCors(builder => {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
