using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CarWebApp.Configuration;
using CarWebApp.Data;
using CarWebApp.Entities;
using CarWebApp.Filters;
using CarWebApp.Middleware;
using CarWebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Rotativa.AspNetCore;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace CarWebApp
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
            var authenticationSettings = new AuthenticationSettings();

            Configuration.GetSection("Authentication").Bind(authenticationSettings);
            services.AddSingleton(authenticationSettings);
            services.AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = "Bearer";
                    option.DefaultScheme = "Bearer";
                    option.DefaultChallengeScheme = "Bearer";
                }
            ).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = true;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
                };
            });

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ICarModelService, CarModelService>();
            services.AddScoped<DbSeeder>();
            services.AddScoped<AuthenticationFilter, AuthenticationFilter>();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<AuthenticationFilter>();
            });
            services.AddRazorPages();

            services.AddSwaggerGen();
            services.AddAutoMapper(typeof(Program).Assembly);

            services.AddDbContext<DatabaseContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionString"))
            );


            services.AddMvcCore();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DbSeeder seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI();

                seeder.Seed().Wait();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<AuthorizationHeaderMiddleware>();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseMiddleware<AssignUserInfoMiddleware>();
            
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = "https://localhost:5001/swagger";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
            RotativaConfiguration.Setup(env as IHostingEnvironment );
        }
    }
}