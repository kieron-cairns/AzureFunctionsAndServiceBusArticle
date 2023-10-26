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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _repository.AddContactFormEntryAsync(entry);
            return StatusCode(200); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while adding a contact form entry within the controller method: {ex.Message}");
            return StatusCode(500, "An error occurred while processing your request:" );
        }
    }
}
