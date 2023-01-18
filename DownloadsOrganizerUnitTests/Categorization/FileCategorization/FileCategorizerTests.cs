using DownloadsOrganizer.Categorization.CategoriesHolder;
using DownloadsOrganizer.Categorization.FileCategorization;
using DownloadsOrganizer.Data;
using Moq;

namespace DownloadsOrganizerUnitTests.Categorization.FileCategorization;

public class FileCategorizerTests
{
    [Fact]
    public void Categorize_WithFileExtension_ReturnsEmptyCategorizedData()
    {
        // Arrange
        var categoriesHolderMock = new Mock<ICategoriesHolder>();
        categoriesHolderMock.Setup(c => c.GetCategories()).Returns(new List<Category>());

        var categorizer = new FileCategorizer(categoriesHolderMock.Object);

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
    [InlineData("test.m4a", "Audiobooks")]
    [InlineData("test.m4b", "Audiobooks")]
    public void Categorize_WithKnownFileExtension_GivesCorrectCategory(string fileName, string expectedCategory)
    {
        // Arrange
        var categoriesHolderMock = new Mock<ICategoriesHolder>();
        categoriesHolderMock.Setup(c => c.GetCategories()).Returns(new List<Category>
        {
            new Category(){ Name = "Ebooks", FileExtensions = new List<string> { "epub", "mobi", "pdf" }},
            new Category(){ Name = "Audiobooks", FileExtensions = new List<string> { "m4a", "m4b" }},
        });

        var categorizer = new FileCategorizer(categoriesHolderMock.Object);

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
        var categoriesHolderMock = new Mock<ICategoriesHolder>();
        categoriesHolderMock.Setup(c => c.GetCategories()).Returns(new List<Category>
        {
            new Category(){ Name = "Ebooks", FileExtensions = new List<string> { "epub", "mobi", "pdf" }},
            new Category(){ Name = "Audiobooks", FileExtensions = new List<string> { "m4a", "m4b" }},
        });

        var categorizer = new FileCategorizer(categoriesHolderMock.Object);

        var sourceFile = new SourceFile($"C:\\Test\\{fileName}");

        // Act
        var categorizedFile = categorizer.Categorize(sourceFile);

        // Assert
        Assert.False(categorizedFile.HasCategory);
        Assert.Null(categorizedFile.Category);
    }
}
