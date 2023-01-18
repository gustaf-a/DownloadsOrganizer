using DownloadsOrganizer.Categorization;
using DownloadsOrganizer.Categorization.FileCategorization;
using DownloadsOrganizer.Categorization.FolderCategorization;
using DownloadsOrganizer.Data;
using Moq;

namespace DownloadsOrganizerUnitTests.Categorization;

public class CategorizationHandlerTests
{
    [Fact]
    public void Categorize_WithEmptySourceData_ReturnsEmptyCategorizedData()
    {
        // Arrange
        var sourceData = new SourceData(@"C:\test");

        var fileCategorizer = new Mock<IFileCategorizer>();
        var folderCategorizer = new Mock<IFolderCategorizer>();

        var categorizationHandler = new CategorizationHandler(fileCategorizer.Object, folderCategorizer.Object);

        var categorizedData = categorizationHandler.Categorize(sourceData);

        Assert.NotNull(categorizedData);
        Assert.Empty(categorizedData.CategorizedFiles);
        Assert.Empty(categorizedData.CategorizedFolders);
    }

    [Fact]
    public void Categorize_WithOneFile_ReturnsCategorizedDataWithOneCategorizedFile()
    {
        // Arrange
        var sourceFile = new SourceFile(@"C:\test\file.txt");

        var sourceData = new SourceData(@"C:\test")
        {
            SourceFiles = new List<SourceFile> { sourceFile }
        };

        var fileCategorizer = new Mock<IFileCategorizer>();
        fileCategorizer.Setup(f => f.Categorize(It.IsAny<SourceFile>()))
                       .Returns(new CategorizedFile(sourceFile));

        var folderCategorizer = new Mock<IFolderCategorizer>();

        var categorizationHandler = new CategorizationHandler(fileCategorizer.Object, folderCategorizer.Object);

        // Act
        var categorizedData = categorizationHandler.Categorize(sourceData);

        // Assert
        Assert.NotNull(categorizedData);
        Assert.Single(categorizedData.CategorizedFiles);
        Assert.Empty(categorizedData.CategorizedFolders);
    }

    [Fact]
    public void Categorize_WithOneFolder_ReturnsCategorizedDataWithOneCategorizedFolder()
    {
        // Arrange
        var sourceFolder = new SourceFolder(@"C:\test\folder");

        var sourceData = new SourceData(@"C:\test")
        {
            SourceFolders = new List<SourceFolder> { sourceFolder }
        };

        var fileCategorizer = new Mock<IFileCategorizer>();

        var folderCategorizer = new Mock<IFolderCategorizer>();
        folderCategorizer.Setup(f => f.Categorize(It.IsAny<SourceFolder>()))
                         .Returns(new CategorizedFolder(sourceFolder));

        var categorizationHandler = new CategorizationHandler(fileCategorizer.Object, folderCategorizer.Object);
        
        // Act
        var categorizedData = categorizationHandler.Categorize(sourceData);

        // Assert
        Assert.NotNull(categorizedData);
        Assert.Empty(categorizedData.CategorizedFiles);
        Assert.Single(categorizedData.CategorizedFolders);
    }

}
