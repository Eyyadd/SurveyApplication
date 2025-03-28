
using FluentValidation;
using Mapster;
using MapsterMapper;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Survey.Domain.Interfaces.IRepository;
using Survey.Infrastructure.implementation.Repository;
using Survey.Infrastructure.implementation.Service;
using Survey.Infrastructure.IService;
using System.Reflection;

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





        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPollService, PollService>();
            services.AddScoped<IGenericRepository<Poll>, GenericRepository<Poll>>();
        }

        public static void DbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SurveyDbContext>(options => options.UseSqlServer(connectionString));
        }

        public static void MapsterConfiguration(this IServiceCollection services)
        {
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());
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
    }
}
