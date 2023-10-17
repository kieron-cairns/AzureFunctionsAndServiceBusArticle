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
        public async Task PostContactFormEntry_RepositoryThrowsError_LoggerReceivesError()
        {
            // Arrange
            var mockSet = new Mock<DbSet<FormSubmission>>();

            var mockContext = new Mock<IFormSubmissionContext>();
            mockContext.Setup(m => m.FormSubmissions).Returns(mockSet.Object);
            mockContext.Setup(m => m.SaveChangesAsync(default)).Throws(new Exception("Test exception"));

            var mockLogger = new Mock<ILogger<FormRepository>>();

            // Setup for the underlying Log method that the LogError extension calls
            mockLogger.Setup(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)
                )
            ).Verifiable();

            var repository = new FormRepository(mockContext.Object, mockLogger.Object);

            var formSubmission = new FormSubmission();

            // Act
            await repository.AddContactFormEntryAsync(formSubmission);

            // Assert
            mockLogger.Verify();
        }

    }
}