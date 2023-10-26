using Castle.Core.Logging;
using DatabaseSaverWebAPI.Interfaces;
using DatabaseSaverWebAPI.Models;
using DatabaseSaverWebAPITests.ControllerTestUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Text;

namespace DatabaseSaverWebAPITests
{
    public class FormControllerTests
    {

        private readonly Mock<IFormRepository> _mockFormRepository;
        private readonly Mock<ILogger<FormController>> _mockLogger;
        private readonly FormController _formController;

        public FormControllerTests()
        {
            _mockFormRepository = new Mock<IFormRepository>();
            _mockLogger = new Mock<ILogger<FormController>>(); // Create the mock
            _formController = new FormController(_mockFormRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task PostContactFormEntry_WhenCalled_ReturnsStatusCode200()
        {
            // Arrange
            _mockFormRepository.Setup(repo => repo.AddContactFormEntryAsync(It.IsAny<FormSubmission>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _formController.PostContactFormEntry(new FormSubmission());

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

      
        [Fact]
        public async Task PostContactFormEntry_DuplicatePrimaryKey_LogsError()
        {
            // Arrange
            var mockRepository = new Mock<IFormRepository>();
            var mockLogger = new Mock<ILogger<FormController>>();
            var controller = new FormController(mockRepository.Object, mockLogger.Object);

            var duplicateGuid = Guid.NewGuid();
            var duplicateDateTime = DateTime.UtcNow;

            var entry = new FormSubmission
            {
                // Initialize the entry with the necessary properties
                Id = duplicateGuid,
                Name = "string",
                Email = "user@example.com",
                Phone = "string",
                Message = "string",
                Submitted = duplicateDateTime,
                IsContactFormSubmit = true,
                IsNewsLetterSubmit = false
            };

            _mockFormRepository
                .Setup(r => r.AddContactFormEntryAsync(It.IsAny<FormSubmission>()))
                .ThrowsAsync(new DuplicatePrimaryKeyException("Violation of PRIMARY KEY constraint 'PK_FormSubmissions'. Cannot insert duplicate key in object 'dbo.FormSubmissions'. The duplicate key value is (3fa85f64-5717-4562-b3fc-2c963f66afa6)."));

            // Act
            var result = await _formController.PostContactFormEntry(entry);

            // Assert
            _mockLogger.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while adding a contact form entry within the controller method")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

        }

        [Fact]
        public async Task PostContactFormEntry_Returns400ForInvalidModel()
        {
            // Arrange
            var invalidModel = new FormSubmission
            {
                // Don't set the Name property to simulate it being invalid
                Email = "user@example.com",
                Phone = "string",
                Message = "string",
                Submitted = DateTime.UtcNow,
                IsContactFormSubmit = true,
                IsNewsLetterSubmit = true
            };

            _formController.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = await _formController.PostContactFormEntry(invalidModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
    }
}