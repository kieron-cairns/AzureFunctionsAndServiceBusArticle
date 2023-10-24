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
    }
}
