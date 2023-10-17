using System.Text;
using System.Threading.Tasks;
using DatabaseSaverWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FormController : ControllerBase
{
    private readonly IFormRepository _repository;
    private readonly ILogger<FormController> _logger;

    public FormController(IFormRepository repository, ILogger<FormController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpPost("/PostContactFormEntry")]
    public async Task<IActionResult> PostContactFormEntry([FromBody] FormSubmission entry)
    {
        try
        {
           await _repository.AddContactFormEntryAsync(entry);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while adding a contact form entry within the controller method: {ex.Message}");
        }

        return StatusCode(200);
    }

    // Dummy method to represent the "location" URL after creation.
    // You might want to create a proper GET method if needed.
    private IActionResult GetContactFormEntryById(Guid id)
    {
        return NoContent();
    }
}
