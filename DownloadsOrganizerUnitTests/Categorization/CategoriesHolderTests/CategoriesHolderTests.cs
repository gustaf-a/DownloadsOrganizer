using Microsoft.Extensions.Configuration;
using DownloadsOrganizer.Categorization.CategoriesHolder;

namespace DownloadsOrganizerUnitTests.Categorization.CategoriesHolderTests;

public class CategoriesHolderTests
{
    private readonly IConfiguration _configuration;

    public CategoriesHolderTests()
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
    public void GetCategories_ReturnsCategories()
    {
        // Arrange
        var categoriesHolder = new CategoriesHolder(_configuration);

        // Act
        var categories = categoriesHolder.GetCategories();

        // Assert
        Assert.NotEmpty(categories);

        Assert.Equal(2, categories.Count);
    }

    [Fact]
    public void GetEmptyCategory_ReturnsEmptyCategory()
    {
        // Arrange
        var categoriesHolder = new CategoriesHolder(_configuration);

        // Act
        var emptyCategory = categoriesHolder.GetEmptyCategory();

        // Assert
        Assert.NotNull(emptyCategory);
        Assert.Equal("Empty", emptyCategory.Name);
    }

    [Fact]
    public void GetUnknownCategory_ReturnsUnknownCategory()
    {
        // Arrange
        var categoriesHolder = new CategoriesHolder(_configuration);

        // Act
        var unknownCategory = categoriesHolder.GetUnknownCategory();

        // Assert
        Assert.NotNull(unknownCategory);
        Assert.Equal("Unknown", unknownCategory.Name);
    }
}
