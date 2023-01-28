using System;
using System.Text;
using Hubtel.Wallets.Api.DAL;
using Hubtel.Wallets.Api.DAL.Entities;
using Hubtel.Wallets.Api.Interfaces;
using Hubtel.Wallets.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Hubtel.Wallets.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<HubtelDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("Default")));
            services.AddControllers();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Hubtel Wallet API",
                    Description = "API service to be used to manage a user's wallet on the Hubtel app ",
                    Contact = new OpenApiContact
                    {
                        Name = "Isaac Amankwaah Anane",
                        Email = string.Empty,
                        Url = new Uri("https://hubtel.com"),
                    },
                });
            });

            services.AddIdentityCore<AppUser>(options =>
                {
                    options.User.RequireUniqueEmail = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<HubtelDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("M1cr0T0kn!!Th@t1sUbreK@b1eByEu$0RY0uDouBt"))
                    };
                });

            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hubtel Wallet API");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}