using DownloadsOrganizer.Data;
using DownloadsOrganizer.Categorization.FileCategorization;
using Microsoft.Extensions.Configuration;

namespace DownloadsOrganizerUnitTests.Categorization.FileCategorization;

public class FileCategorizerTests
{
    private readonly IConfiguration _configuration;

    public FileCategorizerTests()
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream);
        writer.Write(
            @"{
                    ""Categorization"": {
                        ""Categories"": [
                            {
                                ""Name"": ""Ebooks"",
                                ""FileExtensions"": [""epub"",""mobi"",""pdf"",""odt""]
                            },
                            {
                                ""Name"": ""Audiobooks"",
                                ""FileExtensions"": [""mp3"",""m4a"",""m4b""]
                            }
                        ]
                    }
                }");

        writer.Flush();
        memoryStream.Position = 0;

        _configuration = new ConfigurationBuilder()
            .AddJsonStream(memoryStream)
            .Build();
    }

    [Fact]
    public void Categorize_WithFileExtension_ReturnsEmptyCategorizedData()
    {
        // Arrange
        var categorizer = new FileCategorizer(_configuration);

        var sourceFile = new SourceFile($"C:\\Test\\test");

        // Act
        var categorizedFile = categorizer.Categorize(sourceFile);
        
        // Assert
        Assert.Null(categorizedFile.Category);
        Assert.False(categorizedFile.HasCategory);
    }

    [Theory]
    [InlineData("test.epub", "Ebooks")]
    [InlineData("test.mobi", "Ebooks")]
    [InlineData("test.pdf", "Ebooks")]
    [InlineData("test.odt", "Ebooks")]
    [InlineData("test.mp3", "Audiobooks")]
    [InlineData("test.m4a", "Audiobooks")]
    [InlineData("test.m4b", "Audiobooks")]
    public void Categorize_WithKnownFileExtension_GivesCorrectCategory(string fileName, string expectedCategory)
    {
        // Arrange
        var categorizer = new FileCategorizer(_configuration);

        var sourceFile = new SourceFile($"C:\\Test\\{fileName}");

        // Act
        var categorizedFile = categorizer.Categorize(sourceFile);

        // Assert
        Assert.True(categorizedFile.HasCategory);
        Assert.NotNull(categorizedFile.Category);
        Assert.Equal(expectedCategory, categorizedFile.Category.Name);
    }

    [Theory]
    [InlineData("test.word")]
    [InlineData("test.xyz")]
    [InlineData("test.mp333")]
    [InlineData("test.exe")]
    [InlineData("test.epub2")]
    [InlineData("test.ebub")]
    public void Categorize_WithUnknownFileExtension_GivesNoCategory(string fileName)
    {
        // Arrange
        var categorizer = new FileCategorizer(_configuration);

        var sourceFile = new SourceFile($"C:\\Test\\{fileName}");

        // Act
        var categorizedFile = categorizer.Categorize(sourceFile);
        
        // Assert
        Assert.False(categorizedFile.HasCategory);
        Assert.Null(categorizedFile.Category);
    }
}
