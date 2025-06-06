﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Survey.Domain.Entities;
using Survey.Infrastructure.Core.Configuration;
using System.Reflection;
using System.Security.Claims;

namespace Survey.Infrastructure.Core
{
    public class SurveyDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public SurveyDbContext(DbContextOptions<SurveyDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            this._HttpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new PollsConfiguration());

            //Apply All Configuration for all models in the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //change the behavior of the cascade delete as global 

            var foreignKeys = modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in foreignKeys)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries<Audit>();
            var currentUserId = _HttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            foreach (var entity in entities)
            {
                if(entity.State == EntityState.Added)
                {
                    entity.Property(e => e.CreatedByUserId).CurrentValue = currentUserId!;
                }
                else if(entity.State == EntityState.Modified)
                {
                    entity.Property(e => e.UpdatedByUserId).CurrentValue = currentUserId!;
                    entity.Property(e => e.UpdatedAt).CurrentValue = DateTime.UtcNow;
                }

            }
            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer>  Answers { get; set; }
        public DbSet<Vote>  Votes { get; set; }
        public DbSet<VoteAnswer>  VoteAnswers { get; set; }
    }
}
