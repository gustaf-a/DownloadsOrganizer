using DownloadsOrganizer.Configuration;
using DownloadsOrganizer.IO;
using DownloadsOrganizer.SourceHandling;
using Moq;

namespace DownloadsOrganizerUnitTests.SourceHandling;

public class SourceHandlerTests
{
    private readonly Mock<IConfigurationHandler> _configurationHandlerMock;

    public SourceHandlerTests()
    {
        _configurationHandlerMock = new Mock<IConfigurationHandler>();
        _configurationHandlerMock.Setup(c => c.ApplicationOptions()).Returns(new ApplicationOptions
        {
            SourceFolder = new string[] { "C:\\Users\\TestUser\\Downloads\\Unsorted" },
            OutputFolder = "C:\\Users\\TestUser\\Downloads"
        });
        _configurationHandlerMock.Setup(c => c.CategorizationOptions()).Returns(new CategorizationOptions
        {
            IgnoreFolderNames = new string[] { "temp", "notTempButAlmost" },
            IgnoreFolderPrefixes = new string[] { "_", "%" },
            IgnoreFileNames = new string[] { "hiddenfile.txt", "DownloadsOrganizer.exe" },
            IgnoreFilePrefixes = new string[] { "&", "#" },
            IgnoreFileExtensions = new string[] { "tmp", "part" }
        });
    }

    [Fact]
    public void GetSourceData_ReturnsSourceData()
    {
        // Arrange
        var directoryReaderMock = new Mock<IDirectoryReader>();
        
        var sourceHandler = new SourceHandler(_configurationHandlerMock.Object, directoryReaderMock.Object);

        // Act
        var result = sourceHandler.GetSourceData();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetSourceData_ReturnsFiles_WhenFoundInFolder()
    {
        // Arrange
        var files = new[] {
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\file1.txt",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\file2.txt",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\file3.exe",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\file4.exe"
        };

        var directoryReaderMock = new Mock<IDirectoryReader>();
        directoryReaderMock.Setup(dr => dr.GetFiles(It.IsAny<string>())).Returns(files);

        var sourceHandler = new SourceHandler(_configurationHandlerMock.Object, directoryReaderMock.Object);

        // Act
        var result = sourceHandler.GetSourceData();

        // Assert
        Assert.Empty(result.SourceFolders);
        Assert.Equal(4, result.SourceFiles.Count);
        Assert.Equal(files[0], result.SourceFiles[0].FilePath);
        Assert.Equal(files[1], result.SourceFiles[1].FilePath);
        Assert.Equal(files[2], result.SourceFiles[2].FilePath);
        Assert.Equal(files[3], result.SourceFiles[3].FilePath);
    }

    [Fact]
    public void GetSourceData_IgnoresFileNamesToIgnore()
    {
        // Arrange
        var files = new[] {
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\file1.txt",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\hiddenfile.txt",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\DownloadsOrganizer.exe",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\file2.exe"
        };

        var directoryReaderMock = new Mock<IDirectoryReader>();
        directoryReaderMock.Setup(dr => dr.GetFiles(It.IsAny<string>())).Returns(files);

        var sourceHandler = new SourceHandler(_configurationHandlerMock.Object, directoryReaderMock.Object);

        // Act
        var result = sourceHandler.GetSourceData();

        // Assert
        Assert.Empty(result.SourceFolders);
        Assert.Equal(2, result.SourceFiles.Count);
        Assert.Equal(files[0], result.SourceFiles[0].FilePath);
        Assert.Equal(files[3], result.SourceFiles[1].FilePath);
    }

    [Fact]
    public void GetSourceData_IgnoresFilePrefixesToIgnore()
    {
        // Arrange
        var files = new[] {
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\file1.txt",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\&file2.txt",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\#file3.exe",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\file4.exe"
        };

        var directoryReaderMock = new Mock<IDirectoryReader>();
        directoryReaderMock.Setup(dr => dr.GetFiles(It.IsAny<string>())).Returns(files);

        var sourceHandler = new SourceHandler(_configurationHandlerMock.Object, directoryReaderMock.Object);

        // Act
        var result = sourceHandler.GetSourceData();

        // Assert
        Assert.Empty(result.SourceFolders);
        Assert.Equal(2, result.SourceFiles.Count);
        Assert.Equal(files[0], result.SourceFiles[0].FilePath);
        Assert.Equal(files[3], result.SourceFiles[1].FilePath);
    }

    [Fact]
    public void GetSourceData_IgnoresFileExtensionsToIgnore()
    {
        // Arrange
        var files = new[] {
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\file1.txt",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\file2.tmp",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\file3.part",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\file4.exe"
        };

        var directoryReaderMock = new Mock<IDirectoryReader>();
        directoryReaderMock.Setup(dr => dr.GetFiles(It.IsAny<string>())).Returns(files);

        var sourceHandler = new SourceHandler(_configurationHandlerMock.Object, directoryReaderMock.Object);

        // Act
        var result = sourceHandler.GetSourceData();

        // Assert
        Assert.Empty(result.SourceFolders);
        Assert.Equal(2, result.SourceFiles.Count);
        Assert.Equal(files[0], result.SourceFiles[0].FilePath);
        Assert.Equal(files[3], result.SourceFiles[1].FilePath);
    }

    [Fact]
    public void GetSourceData_ReturnsFolders_WhenFoundInFolder()
    {
        // Arrange
        var directories = new[] {
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\Bad book",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\Bad movie"
        };

        var directoryReaderMock = new Mock<IDirectoryReader>();
        directoryReaderMock.Setup(dr => dr.GetDirectories(It.IsAny<string>())).Returns(directories);

        var sourceHandler = new SourceHandler(_configurationHandlerMock.Object, directoryReaderMock.Object);

        // Act
        var result = sourceHandler.GetSourceData();

        // Assert
        Assert.Empty(result.SourceFiles);
        Assert.Equal(2, result.SourceFolders.Count);
        Assert.Equal(directories[0], result.SourceFolders[0].Path);
        Assert.Equal(directories[1], result.SourceFolders[1].Path);
    }

    [Fact]
    public void GetSourceData_IgnoreFolderNamesToIgnore()
    {
        // Arrange
        var directories = new[] {
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\Bad book",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\temp",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\notTempButAlmost",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\Bad movie"
        };

        var directoryReaderMock = new Mock<IDirectoryReader>();
        directoryReaderMock.Setup(dr => dr.GetDirectories(It.IsAny<string>())).Returns(directories);

        var sourceHandler = new SourceHandler(_configurationHandlerMock.Object, directoryReaderMock.Object);

        // Act
        var result = sourceHandler.GetSourceData();

        // Assert
        Assert.Empty(result.SourceFiles);
        Assert.Equal(2, result.SourceFolders.Count);
        Assert.Equal(directories[0], result.SourceFolders[0].Path);
        Assert.Equal(directories[3], result.SourceFolders[1].Path);
    }

    [Fact]
    public void GetSourceData_IgnoreFolderPrefixesToIgnore()
    {
        // Arrange
        var directories = new[] {
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\Bad book",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\_Ok book",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\%Great movie",
            "C:\\Users\\TestUser\\Downloads\\Unsorted\\Bad movie"
        };

        var directoryReaderMock = new Mock<IDirectoryReader>();
        directoryReaderMock.Setup(dr => dr.GetDirectories(It.IsAny<string>())).Returns(directories);

        var sourceHandler = new SourceHandler(_configurationHandlerMock.Object, directoryReaderMock.Object);

        // Act
        var result = sourceHandler.GetSourceData();

        // Assert
        Assert.Empty(result.SourceFiles);
        Assert.Equal(2, result.SourceFolders.Count);
        Assert.Equal(directories[0], result.SourceFolders[0].Path);
        Assert.Equal(directories[3], result.SourceFolders[1].Path);
    }
}
