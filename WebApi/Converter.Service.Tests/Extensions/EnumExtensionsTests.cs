using Converter.Service.Converters;
using Converter.Service.Extensions;
using Xunit;

namespace Converter.Service.Tests.Extensions
{
    public class EnumExtensionsTests
    {
        [Theory]
        [InlineData(".html", ConverterType.Html)]
        [InlineData(".pdf", ConverterType.Pdf)]
        [InlineData(".docx", ConverterType.Docx)]
        public void GetDescription_ShouldReturnCorrectDescription(string expectedDescription, Enum enumValue)
        {
            // Act
            string actualDescription = enumValue.GetDescription();

            // Assert
            Assert.Equal(expectedDescription, actualDescription);
        }

        [Theory]
        [InlineData(".html", ConverterType.Html)]
        [InlineData(".pdf", ConverterType.Pdf)]
        [InlineData(".docx", ConverterType.Docx)]
        public void ToConverterType_ShouldReturnCorrectConverterType(string fileExtension, ConverterType expectedConverterType)
        {
            // Act
            ConverterType actualConverterType = fileExtension.ToConverterType();

            // Assert
            Assert.Equal(expectedConverterType, actualConverterType);
        }

        [Fact]
        public void ToConverterType_WithUnknownFileExtension_ShouldThrowArgumentException()
        {
            // Arrange
            string unknownExtension = "UnknownExtension";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => unknownExtension.ToConverterType());
        }
    }
}