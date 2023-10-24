using DatabaseSaverWebAPI.Interfaces;
using DatabaseSaverWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace DatabaseSaverWebAPITests
{
    public class FormControllerTests
    {
        [Fact]
        public async Task PostContactFormEntry_WhenCalled_ReturnsStatusCode200()
        {
            // Arrange
            var mockRepository = new Mock<IFormRepository>();
            mockRepository.Setup(repo => repo.AddContactFormEntryAsync(It.IsAny<FormSubmission>()))
                .Returns(Task.CompletedTask);

            var mockLogger = new Mock<ILogger<FormController>>();

            var controller = new FormController(mockRepository.Object, mockLogger.Object);

            // Act
            var result = await controller.PostContactFormEntry(new FormSubmission());

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

            mockRepository
                .Setup(r => r.AddContactFormEntryAsync(It.IsAny<FormSubmission>()))
                .ThrowsAsync(new DuplicatePrimaryKeyException("Violation of PRIMARY KEY constraint 'PK_FormSubmissions'. Cannot insert duplicate key in object 'dbo.FormSubmissions'. The duplicate key value is (3fa85f64-5717-4562-b3fc-2c963f66afa6)."));

            // Act
            var result = await controller.PostContactFormEntry(entry);

            // Assert
            mockLogger.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while adding a contact form entry within the controller method")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

        }
    }
}

public class DuplicatePrimaryKeyException : Exception
{
    public DuplicatePrimaryKeyException(string message) : base(message)
    {
    }
}