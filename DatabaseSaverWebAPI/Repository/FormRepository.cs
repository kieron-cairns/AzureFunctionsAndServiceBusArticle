using System.Threading.Tasks;
using DatabaseSaverWebAPI.Data;
using DatabaseSaverWebAPI.Interfaces;
using DatabaseSaverWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class FormRepository : IFormRepository
{
    private readonly IFormSubmissionContext _submissionContext;
    private readonly ILogger<FormRepository> _logger;

    public FormRepository(IFormSubmissionContext submissionContext, ILogger<FormRepository> logger)
    {
        _submissionContext = submissionContext;
        _logger = logger;
    }

    public async Task AddContactFormEntryAsync(FormSubmission entry)
    {
        _submissionContext.FormSubmissions.Add(entry);
        await _submissionContext.SaveChangesAsync();
    }
}
