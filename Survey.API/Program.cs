
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Survey.API.Helper;
using Survey.Infrastructure.Core;

namespace Survey.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            RegisterServices.Register(builder.Services, builder.Configuration);

            var app = builder.Build();

            //Configure Migration 'Update DataBase'

            var db = app.Services.CreateScope().ServiceProvider.GetRequiredService<SurveyDbContext>();

            var Logger = app.Services.GetRequiredService<ILogger<Program>>();

            try {

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
            }

            app.UseHttpsRedirection();

            //app.UseCors("CorsPolicy");
            //for default cors Policy
            app.UseCors();

            app.UseAuthorization();

            app.MapControllers();

            // app.UseMiddleware<HandleExceptions>();
            app.UseExceptionHandler();
            app.Run();
        }
    }
}
