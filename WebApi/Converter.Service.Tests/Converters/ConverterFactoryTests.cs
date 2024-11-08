using Converter.Service.Converters;
using Converter.Service.Converters.Interfaces;
using Converter.Service.Repositories.Interfaces;
using Converter.Service.Settings;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Converter.Service.Tests.Converters
{
    public class ConverterFactoryTests
    {
        [Fact]
        public void CreateConverter_WithValidFileExtension_ShouldReturnHtmlToPdfConverter()
        {
            // Arrange
            var fileRepositoryMock = new Mock<IFileRepository>();
            var settingsMock = new Mock<IOptions<ApplicationSettings>>();
            var converterFactory = new ConverterFactory(fileRepositoryMock.Object, settingsMock.Object);

            const string validFileExtension = ".html";

            // Act
            IConverter result = converterFactory.CreateConverter(validFileExtension);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<HtmlToPdfConverter>(result);
        }

        [Fact]
        public void CreateConverter_WithInvalidFileExtension_ShouldThrowNotSupportedException()
        {
            // Arrange
            var fileRepositoryMock = new Mock<IFileRepository>();
            var settingsMock = new Mock<IOptions<ApplicationSettings>>();
            var converterFactory = new ConverterFactory(fileRepositoryMock.Object, settingsMock.Object);

            const string invalidFileExtension = ".txt";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => converterFactory.CreateConverter(invalidFileExtension));
        }

        [Fact]
        public void CreateConverter_WithNullFileExtension_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fileRepositoryMock = new Mock<IFileRepository>();
            var settingsMock = new Mock<IOptions<ApplicationSettings>>();
            var converterFactory = new ConverterFactory(fileRepositoryMock.Object, settingsMock.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => converterFactory.CreateConverter(null));
        }
    }
}