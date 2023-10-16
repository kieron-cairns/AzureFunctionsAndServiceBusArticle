using System.Threading.Tasks;
using DatabaseSaverWebAPI.Data;
using DatabaseSaverWebAPI.Interfaces;
using DatabaseSaverWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class FormRepository : IFormRepository
{
    private readonly IFormSubmissionContext _submissionContext;

    public FormRepository(IFormSubmissionContext submissionContext)
    {
        _submissionContext = submissionContext;
    }

    public async Task AddContactFormEntryAsync(FormSubmission entry)
    {
        try
        {
            _submissionContext.FormSubmissions.Add(entry);
            await _submissionContext.SaveChangesAsync();
        }
        catch(Exception ex) {

            Console.WriteLine(ex.ToString());

        }
    }
}
