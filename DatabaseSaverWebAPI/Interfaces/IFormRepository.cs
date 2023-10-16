using DatabaseSaverWebAPI.Models;

public interface IFormRepository
{
    public Task AddContactFormEntryAsync(FormSubmission entry);
}