using Converter.Service.Extensions;
using Converter.Service.Model;
using Xunit;

namespace Converter.Service.Tests.Extensions
{
    public class FileConverterTests
    {
        [Fact]
        public void ToFileModels_ShouldConvertDictionaryToEnumerableOfFileModel()
        {
            // Arrange
            var inputDictionary = new Dictionary<Guid, string>
            {
                { Guid.NewGuid(), "File1.txt" },
                { Guid.NewGuid(), "File2.pdf" },
                { Guid.NewGuid(), "File3.docx" }
            };

            // Act
            IEnumerable<FileModel> result = inputDictionary.ToFileModels();

            // Assert
            Assert.Equal(inputDictionary.Count, result.Count());

            foreach (var fileModel in result)
            {
                Assert.Contains(fileModel.Id, inputDictionary.Keys);
                Assert.Equal(inputDictionary[fileModel.Id], fileModel.Name);
            }
        }

        [Fact]
        public void ToFileModels_WithEmptyDictionary_ShouldReturnEmptyEnumerable()
        {
            // Arrange
            var emptyDictionary = new Dictionary<Guid, string>();

            // Act
            IEnumerable<FileModel> result = emptyDictionary.ToFileModels();

            // Assert
            Assert.Empty(result);
        }
    }
}