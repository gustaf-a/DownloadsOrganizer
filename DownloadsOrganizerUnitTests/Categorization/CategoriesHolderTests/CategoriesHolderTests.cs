using DownloadsOrganizer.Categorization.CategoriesHolder;
using DownloadsOrganizer.Configuration;
using Moq;
using DownloadsOrganizer.Data;

namespace DownloadsOrganizerUnitTests.Categorization.CategoriesHolderTests;

public class CategoriesHolderTests
{
    private readonly Mock<IConfigurationHandler> _configurationHandlerMock;

    public CategoriesHolderTests()
    {
        _configurationHandlerMock = new Mock<IConfigurationHandler>();
        _configurationHandlerMock.Setup(c => c.CategorizationOptions()).Returns(new CategorizationOptions
        {
            Categories = new Category[]
            {
                new Category
                {
                    Name = "Ebooks",
                    FileExtensions = new List<string> { "epub", "mobi", "pdf", "odt" }
                },
                new Category
                {
                    Name = "Audiobooks",
                    FileExtensions = new List<string> { "mp3", "m4a", "m4b" }
                }
            },
        });
    }

    [Fact]
    public void GetCategories_ReturnsCategories()
    {
        // Arrange
        var categoriesHolder = new CategoriesHolder(_configurationHandlerMock.Object);

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
        var categoriesHolder = new CategoriesHolder(_configurationHandlerMock.Object);

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
        var categoriesHolder = new CategoriesHolder(_configurationHandlerMock.Object);

        // Act
        var unknownCategory = categoriesHolder.GetUnknownCategory();

        // Assert
        Assert.NotNull(unknownCategory);
        Assert.Equal("Unknown", unknownCategory.Name);
    }
}
