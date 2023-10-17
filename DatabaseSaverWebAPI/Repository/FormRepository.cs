using System.Threading.Tasks;
using DatabaseSaverWebAPI.Data;
using DatabaseSaverWebAPI.Interfaces;
using DatabaseSaverWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class FormRepository : IFormRepository
{
    private readonly IFormSubmissionContext _submissionContext;
    private readonly ILogger _logger;

    public FormRepository(IFormSubmissionContext submissionContext, ILogger logger)
    {
        _submissionContext = submissionContext;
        _logger = logger;
    }

    public async Task AddContactFormEntryAsync(FormSubmission entry)
    {
        try
        {
            _submissionContext.FormSubmissions.Add(entry);
            await _submissionContext.SaveChangesAsync();
        }
        catch(Exception ex) {

            _logger.LogError($"An error occurred while adding a contact form entry within the repository method: {ex.Message}");
        }
    }
}
