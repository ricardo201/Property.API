using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Core.Options;
using PropertyBuilding.Core.Services;
using PropertyBuilding.Infrastructure.Data;
using PropertyBuilding.Infrastructure.Filters;
using PropertyBuilding.Infrastructure.Interfaces;
using PropertyBuilding.Infrastructure.Repositories;
using PropertyBuilding.Infrastructure.Services;
using PropertyBuilding.Infrastructure.Validators;
using System;
using System.Text;

namespace PropertyBuilding.API
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
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"])),
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidAudience = Configuration["Authentication:ValidAudience"],
                    ValidIssuer = Configuration["Authentication:ValidIssuer"]
                };
            });
            services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilter>();
            })
            .AddNewtonsoftJson(options => {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                }); ;
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Property.API", Version = "v1" });
            });            
            services.AddTransient<IEncriptService, EncriptService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IOwnerService, OwnerService>();
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IPropertyImageService, PropertyImageService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IPropertyTraceService, PropertyTraceService>();
            services.AddDbContext<PropertyBuildingDataBaseContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("PropertyBuildingApiConnection"),
            b => b.MigrationsAssembly("PropertyBuilding.API")));
            services.AddMvc(options => {
                options.Filters.Add<ValidationFilter>();
            }).AddFluentValidation(options => {               
                options.RegisterValidatorsFromAssemblyContaining<SignInValidator>();
                options.RegisterValidatorsFromAssemblyContaining<OwnerValidator>();
                options.RegisterValidatorsFromAssemblyContaining<PropertyValidator>();
                options.RegisterValidatorsFromAssemblyContaining<PropertyImageValidator>();
                options.RegisterValidatorsFromAssemblyContaining<PropertyTraceValidator>();
            });
            services.AddSingleton<IUriService>(provider => {
                var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accesor.HttpContext.Request;
                var absolutUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(absolutUri);
            });
            services.Configure<PaginationOptions>(Configuration.GetSection("Pagination"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Property.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}