
using FluentValidation;
using Hangfire;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Survey.Domain.Interfaces.IRepository;
using Survey.Infrastructure.DTOs.Auth;
using Survey.Infrastructure.Errors;
using Survey.Infrastructure.implementation.Repository;
using Survey.Infrastructure.implementation.Service;
using Survey.Infrastructure.IService;
using System.Reflection;
using System.Text;

namespace Survey.API.Helper
{
    public static class RegisterServices
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            //DbContext Configuration 
            services.DbContextConfiguration(configuration);

            //Mapster
            services.MapsterConfiguration();

            //FluentValidation
            services.RegisterFluentValidation();

            //Swagger
            services.AddSwagger();

            //custom-services
            services.AddServices();

            //Authentication
            services.AddAuthentication(configuration);

            //CORS 
            services.AddCorsConfiguration();

            //Excpetion Handlding
            services.AddGlobalExceptionHandler();

            //Hangfire
            services.AddHangfireConfig(configuration);






        }
        public static void AddGlobalExceptionHandler(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptions>();
            services.AddProblemDetails();
        }
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPollService, PollService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IQuestionRepo, QuestionRepo>();
            services.AddScoped<IPollRepo, PollRepo>();
            services.AddScoped<IVoteRepo, VoteRepo>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IVoteResultService, VoteResultService>();
            services.AddScoped<IEmailSender, EmailService>();
            services.AddHttpContextAccessor();


        }

        public static void AddHangfireConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));

            services.AddHangfireServer();
        }

        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            //use configure in case of we don't need any validation - use addOptions in case of having validation in JWtOptions Class
            //services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();


            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
            });

            var JwtSetting = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

            var Key = JwtSetting?.Key;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key!)),
                    ValidateLifetime = true,

                    ValidateIssuer = true,
                    ValidIssuer = JwtSetting?.Issuer,

                    ValidateAudience = true,
                    ValidAudience = JwtSetting?.Audience,
                };

            });

            services.Configure<Infrastructure.Setting.MailSetting>(configuration.GetSection(nameof(Infrastructure.Setting.MailSetting)));
        }

        public static void DbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SurveyDbContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SurveyDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void MapsterConfiguration(this IServiceCollection services)
        {
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            var assembly = Assembly.Load("Survey.Infrastructure");
            mappingConfig.Scan(assembly);
            services.AddSingleton<IMapper>(new Mapper(mappingConfig));
        }

        public static void RegisterFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.Load("Survey.Infrastructure"));
            services.AddFluentValidationAutoValidation();
        }

        public static void AddSwagger(this IServiceCollection service)
        {
            service.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();

        }

        public static void AddCorsConfiguration(this IServiceCollection services)
        {
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy",
            //        builder =>
            //        {
            //            builder.AllowAnyOrigin()
            //                   .AllowAnyMethod()
            //                   .AllowAnyHeader();
            //        });
            //});


            //for default cors Policy
            services.AddCors(options => options.AddDefaultPolicy(
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }
            ));
        }
    }
}
