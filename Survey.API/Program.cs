using Hangfire;
using Serilog;
using Survey.API.Helper;
using Hangfire.Dashboard;
using HangfireBasicAuthenticationFilter;

namespace Survey.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            RegisterServices.Register(builder.Services, builder.Configuration);

            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });

            builder.Services.AddResponseCaching();

            var app = builder.Build();

            //Configure Migration 'Update DataBase'

            var db = app.Services.CreateScope().ServiceProvider.GetRequiredService<SurveyDbContext>();

            var Logger = app.Services.GetRequiredService<ILogger<Program>>();

            try
            {

                db.Database.Migrate();
                Logger.LogInformation("Database Migrated Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHangfireDashboard();
            }

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();

            app.UseHangfireDashboard("/Jobs", new DashboardOptions
            {
                Authorization =
                [
                    new HangfireCustomBasicAuthenticationFilter
                    {
                        User = app.Configuration.GetValue<string>("HangfireSettings:UserName"),
                        Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")

                    }
                ],
                DashboardTitle = " Survey Dashboard"
            });

            //app.UseCors("CorsPolicy");
            //for default cors Policy
            app.UseCors();

            app.UseAuthorization();

            app.UseResponseCaching();

            app.MapControllers();

            // app.UseMiddleware<HandleExceptions>();
            app.UseExceptionHandler();
            app.Run();
        }
    }
}
