using DatabaseSaverWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseSaverWebAPI.Interfaces
{
    public interface IFormSubmissionContext
    {
        DbSet<FormSubmission> QueryHistories { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}