using AutoMapper;
using FPTU_Starter.API.Exception;
using FPTU_Starter.Application;
using FPTU_Starter.Infrastructure;
using FPTU_Starter.Infrastructure.Database;
using FPTU_Starter.Infrastructure.Dependecy_Injection;
using Microsoft.Extensions.Options;
using FPTU_Starter.Infrastructure.MapperConfigs;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using FPTU_Starter.Application.IRepository;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.Services;
using FPTU_Starter.Infrastructure.Repository;

namespace FPTU_Starter.API
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.AddInfrastructure(builder.Configuration);
            //add CORS
            builder.Services.AddCors(p => p.AddPolicy("MyCors", build =>
            {
                build.WithOrigins("*")
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));
            builder.Services.AddAuthentication().AddGoogle(opts =>
            {
                opts.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                opts.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                //opts.Scope.Add("profile");
                //opts.Events.OnCreatingTicket = (context) =>
                //{
                //    var picture = context.User.GetProperty("picture").GetString();

                //    context.Identity.AddClaim(new Claim("picture", picture));

                //    return Task.CompletedTask;
                //};
            });
            builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                {
                    NamingStrategy = new Newtonsoft.Json.Serialization.KebabCaseNamingStrategy()
                };
            });
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionHandler>();
            });
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAutoMapper(typeof(MapperConfig).Assembly);
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Your API Title",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
            Enter 'Bearer' [space] and then your token in the text input below.
            Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("MyCors");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
