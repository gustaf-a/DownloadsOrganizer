using DownloadsOrganizer.Data;

namespace DownloadsOrganizerUnitTests.Data;

public class SourceFolderTests
{
    [Fact]
    public void GetFolderName_ReturnsFolderName()
    {
        // Arrange
        var sourceFolder = new SourceFolder("C:\\Downloads\\Folder");

        // Act
        var folderName = sourceFolder.FolderName;

        // Assert
        Assert.Equal("Folder", folderName);
    }
}
