using DatabaseSaverWebAPI.Interfaces;
using DatabaseSaverWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseSaverWebAPI.Data
{
    public class FormSubmissionContext : DbContext, IFormSubmissionContext
    {
        public DbSet<FormSubmission> FormSubmissions { get; set; }

        public FormSubmissionContext(DbContextOptions<FormSubmissionContext> options) : base(options)
        {
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }


    }
}
