using Converter.Service.Converters;
using Converter.Service.Exceptions;
using Converter.Service.Model;
using Converter.Service.Repositories.Interfaces;
using Converter.Service.Settings;
using Microsoft.Extensions.Options;
using Moq;
using System.Text;
using Xunit;

namespace Converter.Service.Tests.Converters
{
    public class HtmlToPdfConverterTests
    {
        private readonly IFileRepository _fileRepository;
        private readonly IOptions<ApplicationSettings> _settings;

        public HtmlToPdfConverterTests()
        {
            _fileRepository = new Mock<IFileRepository>().Object;
            _settings = Options.Create(new ApplicationSettings
            {
                FileSettings = new FileSettings
                {
                    BufferSize = 8192,
                    DirectoryPath = "files",
                    DateFormat = "yyyy-MM-dd-HH-mm-ss"
                }
            });
        }

        [Fact]
        public async Task ConvertAsync_ValidHtmlFile_Success()
        {
            // Arrange
            var converter = new HtmlToPdfConverter(_fileRepository, _settings);
            var htmlContent = "<html><body><h1>Hello World!</h1></body></html>";
            FileRequestModel inputFile = new()
            {
                Content = Encoding.UTF8.GetBytes(htmlContent),
                Length = htmlContent.Length,
                FileName = "file.html"
            };

            // Act
            var result = await converter.ConvertAsync(inputFile);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.EndsWith(".pdf", result.Name, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task ConvertAsync_InvalidHtmlFile_ThrowsException()
        {
            // Arrange
            var converter = new HtmlToPdfConverter(_fileRepository, _settings);
            FileRequestModel inputFile = new()
            {
                FileName = "invalid.txt",
                Length = 0,
                Content = Encoding.UTF8.GetBytes("Invalid documnet")
            };

            // Act & Assert
            await Assert.ThrowsAsync<IncorrectInputFileException>(() => converter.ConvertAsync(inputFile));
        }
    }
}