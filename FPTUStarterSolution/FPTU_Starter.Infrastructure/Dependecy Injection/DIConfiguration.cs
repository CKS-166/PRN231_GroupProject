using AutoMapper;
using FPTU_Starter.Application.IEmailService;
using FPTU_Starter.Application.IRepository;
using FPTU_Starter.Application.ITokenService;
using FPTU_Starter.Application.Services;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Domain;
using FPTU_Starter.Domain.EmailModel;
using FPTU_Starter.Domain.Entity;
using FPTU_Starter.Infrastructure.Authentication;
using FPTU_Starter.Infrastructure.BackgroundWorkerService;
using FPTU_Starter.Infrastructure.CloudinaryClassSettings;
using FPTU_Starter.Infrastructure.Database;
using FPTU_Starter.Infrastructure.Database.Interface;
using FPTU_Starter.Infrastructure.EmailService;
using FPTU_Starter.Infrastructure.MapperConfigs;
using FPTU_Starter.Infrastructure.OuterService.Implementation;
using FPTU_Starter.Infrastructure.OuterService.Interface;
using FPTU_Starter.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FPTU_Starter.Infrastructure.Dependecy_Injection
{
    public static class DIConfiguration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
        {
            //DBContext
            service.AddDbContext<MyDbContext>(option =>
            option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly(typeof(DIConfiguration).Assembly.FullName)), ServiceLifetime.Scoped);
            //MongoDB 
            service.AddSingleton<IMongoDbContext, MongoDBContext>();
            //BaseRepository          
            service.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            //Identity
            service.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MyDbContext>()
                .AddDefaultTokenProviders();


            //Authentication
            service.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuration["Jwt:key"]!))
                };
            });


            //Email Configuration
            service.Configure<IdentityOptions>(
                opts => opts.SignIn.RequireConfirmedEmail = true
                );
            var emailCofig = configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfig>();
            service.AddSingleton(emailCofig);

            //CloudinarySetting
            service.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

            //Services and Repositories            
            service.AddTransient<IUserRepository, UserRepository>();
            service.AddScoped<IAuthenticationService, AuthenticationService>();
            service.AddScoped<ITokenGenerator, TokenGenerator>();
            service.AddScoped<IEmailService, EmailService.EmailService>();
            service.AddScoped<IGoogleService, GoogleService>();
            service.AddScoped<IProjectRepository, ProjectRepository>();
            service.AddScoped<IProjectManagementService, ProjectManagementService>();
            service.AddScoped<IPackageManagementService, PackageManagementService>();
            service.AddScoped<ICategoryService, CategoryService>();
            service.AddScoped<ICategoryRepository, CategoryRepository>();
            service.AddScoped<IPackageRepository, PackageRepository>();
            service.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
            service.AddScoped<ISubCategoryManagmentService,SubCategoryManagementService>();
            service.AddScoped<IPhotoService, UploadPhotoService>();
            service.AddScoped<IVideoService, UploadVideoService>();
            service.AddScoped<IWalletRepository, WalletRepository>();
            service.AddScoped<IWalletService, WalletService>();
            service.AddScoped<ITransactionRepository, TransactionRepository>();
            service.AddScoped<IUserManagementService, UserManagementService>();
            service.AddScoped<IUserRepository, UserRepository>();
            service.AddScoped<ITransactionService,TransactionService>();
            service.AddScoped<ILikeRepository,LikeRepository>();
            service.AddScoped<ICommentRepository,CommentRepository>();
            service.AddScoped<IInteractionService,InteractionService>();
            service.AddScoped<IAboutUsManagementService, AboutUsManagementService>();
            service.AddScoped<IAboutUsRepository, AboutUsRepository>();
            service.AddScoped<IStageManagementService, StageManagementService>();
            service.AddScoped<IStageRepository, StageRepository>();
            service.AddScoped<IRewardItemRepository, RewardItemRepository>();
            service.AddScoped<IWithdrawRepository, WithdrawRepository>();
            service.AddScoped<IWithdrawService, WithdrawService>();
            service.AddScoped<ISystemWalletRepository,SystemWalletRepository>();
            service.AddScoped<ISystemWalletService,SystemWalletService>();
            service.AddScoped<IBankAccountRepository,BankAccountRepository>();
            service.AddScoped<ICommissionService, CommissionService>();
            service.AddHostedService<WorkerService>();
            return service;
        }
    }
}
