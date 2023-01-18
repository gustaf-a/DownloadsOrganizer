using DownloadsOrganizer.Data;
using DownloadsOrganizer.Categorization.CategoriesHolder;
using DownloadsOrganizer.Categorization.FileCategorization;
using DownloadsOrganizer.Categorization.FolderCategorization;
using Moq;

namespace DownloadsOrganizerUnitTests.Categorization.FolderCategorization;

public class FolderCategorizerTests
{
    [Fact]
    public void Categorize_FolderWithoutContainedFiles_ReturnsEmptyCategory()
    {
        // Arrange
        var fileCategorizerMock = new Mock<IFileCategorizer>();
        
        var categoriesHolderMock = new Mock<ICategoriesHolder>();
        categoriesHolderMock.Setup(c => c.GetEmptyCategory()).Returns(new Category() { Name = "Empty" });

        var categorizer = new FolderCategorizer(fileCategorizerMock.Object, categoriesHolderMock.Object);

        var sourceFolder = new SourceFolder(string.Empty);

        // Act
        var categorizedFolder = categorizer.Categorize(sourceFolder);

        // Assert
        Assert.NotNull(categorizedFolder);
        Assert.True(categorizedFolder.HasCategory);
        Assert.Equal("Empty", categorizedFolder.Category.Name);
    }

    [Fact]
    public void Categorize_FolderWithOneFileType_ReturnsThatCategory()
    {
        // Arrange
        var sourceFile = new SourceFile("Bad Book - Bad Author.epub");

        var sourceFolder = new SourceFolder(string.Empty)
        {
            ContainedFiles = new List<SourceFile> { sourceFile }
        };

        var expectedCategory = new Category() { Name = "Ebooks" };

        var fileCategorizerMock = new Mock<IFileCategorizer>();
        fileCategorizerMock.Setup(f => f.Categorize(It.IsAny<SourceFile>()))
                           .Returns(new CategorizedFile(sourceFile) { Category = expectedCategory });

        var categoriesHolderMock = new Mock<ICategoriesHolder>();

        var categorizer = new FolderCategorizer(fileCategorizerMock.Object, categoriesHolderMock.Object);

        // Act
        var categorizedFolder = categorizer.Categorize(sourceFolder);

        // Assert
        Assert.NotNull(categorizedFolder);
        Assert.True(categorizedFolder.HasCategory);
        Assert.Equal("Ebooks", categorizedFolder.Category.Name);
    }

    [Fact]
    public void Categorize_FolderWithMultipleFileTypes_ReturnsCategoryWithMostOccurences()
    {
        // Arrange
        var sourceFile1 = new SourceFile("Bad Book - Bad Author.epub");
        var sourceFile2 = new SourceFile("Bad Book - Bad Author - Part 1.mp3");
        var sourceFile3 = new SourceFile("Bad Book - Bad Author - part 2.mp3");
        var sourceFile4 = new SourceFile("Bad Book - Bad Author - part 3.mp3");

        var sourceFolder = new SourceFolder(string.Empty)
        {
            ContainedFiles = new List<SourceFile> { sourceFile1, sourceFile2, sourceFile3, sourceFile4 }
        };
        
        var ebooksCategory = new Category() { Name = "Ebooks" };
        var audiobookCategory = new Category() { Name = "Audiobooks" };

        var fileCategorizerMock = new Mock<IFileCategorizer>();
        fileCategorizerMock.Setup(f => f.Categorize(It.IsAny<SourceFile>()))
                           .Returns(new CategorizedFile(sourceFile1) { Category = audiobookCategory });
        fileCategorizerMock.Setup(f => f.Categorize(sourceFile1))
                   .Returns(new CategorizedFile(sourceFile1) { Category = ebooksCategory });

        var categoriesHolderMock = new Mock<ICategoriesHolder>();

        var categorizer = new FolderCategorizer(fileCategorizerMock.Object, categoriesHolderMock.Object);

        // Act
        var categorizedFolder = categorizer.Categorize(sourceFolder);

        // Assert
        Assert.NotNull(categorizedFolder);
        Assert.True(categorizedFolder.HasCategory);
        Assert.Equal("Audiobooks", categorizedFolder.Category.Name);
    }

    [Fact]
    public void Categorize_FolderWithMultipleFileTypesSameOccurences_ReturnsUnknownCategory()
    {
        // Arrange
        var sourceFile1 = new SourceFile("Bad Book - Bad Author.epub");
        var sourceFile2 = new SourceFile("Bad Book - Bad Author.mp3");

        var sourceFolder = new SourceFolder(string.Empty)
        {
            ContainedFiles = new List<SourceFile> { sourceFile1, sourceFile2 }
        };

        var ebooksCategory = new Category() { Name = "Ebooks" };
        var audiobookCategory = new Category() { Name = "Audiobooks" };

        var fileCategorizerMock = new Mock<IFileCategorizer>();
        fileCategorizerMock.Setup(f => f.Categorize(sourceFile1))
                   .Returns(new CategorizedFile(sourceFile1) { Category = ebooksCategory });
        fileCategorizerMock.Setup(f => f.Categorize(sourceFile2))
                           .Returns(new CategorizedFile(sourceFile1) { Category = audiobookCategory });

        var categoriesHolderMock = new Mock<ICategoriesHolder>();
        categoriesHolderMock.Setup(c => c.GetUnknownCategory()).Returns(new Category() { Name = "Unknown" });

        var categorizer = new FolderCategorizer(fileCategorizerMock.Object, categoriesHolderMock.Object);

        // Act
        var categorizedFolder = categorizer.Categorize(sourceFolder);

        // Assert
        Assert.NotNull(categorizedFolder);
        Assert.True(categorizedFolder.HasCategory);
        Assert.Equal("Unknown", categorizedFolder.Category.Name);
    }
}
