using DatabaseSaverWebAPI.Interfaces;
using DatabaseSaverWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSaverWebAPITests
{
    public class FormRepositoryTests
    {
        [Fact]
        public async Task AddContactFormEntryAsync_WhenCalled_AddsEntry()
        {
            // Arrange
            var mockSet = new Mock<DbSet<FormSubmission>>();

            var mockContext = new Mock<IFormSubmissionContext>();
            mockContext.Setup(m => m.FormSubmissions).Returns(mockSet.Object);

            var mockLogger = new Mock<ILogger<FormRepository>>();

            var repository = new FormRepository(mockContext.Object, mockLogger.Object);

            var formSubmission = new FormSubmission();

            // Act
            await repository.AddContactFormEntryAsync(formSubmission);

            // Assert
            mockSet.Verify(m => m.Add(It.IsAny<FormSubmission>()), Times.Once());
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task AddContactFormEntryAsync_WhenExceptionOccurs_LoggerReceivesError()
        {
            // Arrange
            var mockSet = new Mock<DbSet<FormSubmission>>();

            var mockContext = new Mock<IFormSubmissionContext>();
            mockContext.Setup(m => m.FormSubmissions).Returns(mockSet.Object);
            mockContext.Setup(m => m.SaveChangesAsync(default)).Throws(new Exception("Test exception"));

            var mockLogger = new Mock<ILogger<FormRepository>>();
            mockLogger.Setup(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                )
            );

            var repository = new FormRepository(mockContext.Object, mockLogger.Object);

            var formSubmission = new FormSubmission();

            // Act
            await repository.AddContactFormEntryAsync(formSubmission);

            // Assert
            mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ),
                Times.Once
            );
        }

    }

}
