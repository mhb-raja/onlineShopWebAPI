using Core.Security;
using Core.Services.Implementations;
using Core.Services.Interfaces;
using Core.Utilities.Extensions;
using DataLayer.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.IO;
using System.Text;

namespace sampleEshop.WebApi
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "sampleEshop.WebApi", Version = "v1" });
            });

            services.AddSingleton<IConfiguration>(
                new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json")
                .Build()
                );

            #region Add DbContext
            services.AddApplicationDbContext(Configuration);
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            #endregion

            #region Application Services

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<ISliderService2, SliderService2>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPasswordHelper, PasswordHelper>();
            //services.AddScoped<IMailSender, SendEmailService>();
            //services.AddScoped<IViewRenderService, RenderViewToString>();
            services.AddScoped<IOrderService, OrderService>();
            //services.AddScoped<IAccessService, AccessService>();
            

            #endregion

            #region Authentication

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Security.issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Security.secretKeyStr))
                    };
                });

            #endregion

            #region CORS

            services.AddCors(options =>
            {
                //policyname must be much more complicated #################
                options.AddPolicy(Security.corsPolicyName, builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build();//.AllowCredentials() removed - worked on netcoreapp2.1!!!
                });
            });

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "sampleEshop.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //-------
            app.UseStaticFiles();
            app.UseCors(Security.corsPolicyName);//policyname must be much more complicated #################
            app.UseAuthentication();

            //-------------------

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
