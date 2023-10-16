using System.Text;
using System.Threading.Tasks;
using DatabaseSaverWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FormController : ControllerBase
{
    private readonly IFormRepository _repository;

    public FormController(IFormRepository repository)
    {
        _repository = repository;
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
