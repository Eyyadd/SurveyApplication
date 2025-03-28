using Microsoft.EntityFrameworkCore;
using Survey.Domain.Entities;
using Survey.Infrastructure.Core.Configuration;
using System.Reflection;

namespace Survey.Infrastructure.Core
{
    public class SurveyDbContext : DbContext
    {
        public SurveyDbContext(DbContextOptions<SurveyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new PollsConfiguration());

            //Apply All Configuration for all models in the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Poll> Polls { get; set; }
    }
}
