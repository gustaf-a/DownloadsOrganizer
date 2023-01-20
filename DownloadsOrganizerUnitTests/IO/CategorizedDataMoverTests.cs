using DownloadsOrganizer.Data;
using DownloadsOrganizer.IO;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DownloadsOrganizerUnitTests.IO;

public class CategorizedDataMoverTests
{
    private readonly IConfiguration _configuration;

    public CategorizedDataMoverTests()
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream);
        writer.Write(
            @"{
                ""Application"": {
                    ""SourceFolder"": [
                        ""C:\\Users\\TestUser\\Downloads\\Unsorted""
                    ],
                    ""OutputFolder"": ""C:\\Users\\TestUser\\Downloads""
                    }
            }");

        writer.Flush();
        memoryStream.Position = 0;

        _configuration = new ConfigurationBuilder()
            .AddJsonStream(memoryStream)
            .Build();
    }

    [Fact]
    public void MoveData_ReturnsMoveResult()
    {
        // Arrange
        var fileMoverMock = new Mock<IFileMover>();

        var categorizedDataMover = new CategorizedDataMover(_configuration, fileMoverMock.Object);

        var categorizedData = new CategorizedData();

        // Act
        var moveResult = categorizedDataMover.MoveData(categorizedData);

        // Assert
        Assert.NotNull(moveResult);
        Assert.Empty(moveResult.FilesMoved);
        Assert.Empty(moveResult.FilesNotMoved);
        Assert.Empty(moveResult.FoldersMoved);
        Assert.Empty(moveResult.FoldersNotMoved);
    }

    [Fact]
    public void MoveData_MovesAllFiles()
    {
        // Arrange
        var sourceFile1 = new SourceFile("C:\\Users\\TestUser\\Downloads\\Unsorted\\file1.txt");
        var expectedfile1Path = "C:\\Users\\TestUser\\Downloads\\file1.txt";

        var sourceFile2 = new SourceFile("C:\\Users\\TestUser\\Downloads\\Unsorted\\file2.exe");
        var expectedfile2Path = "C:\\Users\\TestUser\\Downloads\\file2.exe";

        var fileMoverMock = new Mock<IFileMover>();
        fileMoverMock.Setup(f => f.MoveFile(sourceFile1.FilePath, expectedfile1Path)).Returns(new MovedObject(sourceFile1.FilePath, expectedfile1Path));
        fileMoverMock.Setup(f => f.MoveFile(sourceFile2.FilePath, expectedfile2Path)).Returns(new MovedObject(sourceFile2.FilePath, expectedfile2Path));

        var categorizedDataMover = new CategorizedDataMover(_configuration, fileMoverMock.Object);

        var categorizedData = new CategorizedData
        {
            CategorizedFiles = new List<CategorizedFile>
            {
                new CategorizedFile(sourceFile1),
                new CategorizedFile(sourceFile2)
            }
        };
        
        // Act
        var moveResult = categorizedDataMover.MoveData(categorizedData);

        // Assert
        Assert.NotNull(moveResult);
        Assert.Empty(moveResult.FoldersMoved);
        Assert.Empty(moveResult.FoldersNotMoved);
        Assert.Empty(moveResult.FilesNotMoved);

        Assert.Equal(2 ,moveResult.FilesMoved.Count);

        Assert.Equal(sourceFile1.FilePath, moveResult.FilesMoved[0].SourcePath);
        Assert.Equal(expectedfile1Path, moveResult.FilesMoved[0].DestinationPath);

        Assert.Equal(sourceFile2.FilePath, moveResult.FilesMoved[1].SourcePath);
        Assert.Equal(expectedfile2Path, moveResult.FilesMoved[1].DestinationPath);
    }

    [Fact]
    public void MoveData_HandlesFailuresToMoveFiles()
    {
        // Arrange
        var sourceFile1 = new SourceFile("C:\\Users\\TestUser\\Downloads\\Unsorted\\file1.txt");
        var expectedfile1Path = "C:\\Users\\TestUser\\Downloads\\file1.txt";

        var sourceFile2 = new SourceFile("C:\\Users\\TestUser\\Downloads\\Unsorted\\file2.exe");
        var expectedfile2Path = "C:\\Users\\TestUser\\Downloads\\file2.exe";

        var fileMoverMock = new Mock<IFileMover>();
        fileMoverMock.Setup(f => f.MoveFile(sourceFile1.FilePath, expectedfile1Path)).Returns(new MovedObject(sourceFile1.FilePath, expectedfile1Path));
        fileMoverMock.Setup(f => f.MoveFile(sourceFile2.FilePath, expectedfile2Path)).Throws<DirectoryNotFoundException>();

        var categorizedDataMover = new CategorizedDataMover(_configuration, fileMoverMock.Object);

        var categorizedData = new CategorizedData
        {
            CategorizedFiles = new List<CategorizedFile>
            {
                new CategorizedFile(sourceFile1),
                new CategorizedFile(sourceFile2)
            }
        };

        // Act
        var moveResult = categorizedDataMover.MoveData(categorizedData);

        // Assert
        Assert.NotNull(moveResult);
        Assert.Empty(moveResult.FoldersMoved);
        Assert.Empty(moveResult.FoldersNotMoved);

        Assert.Equal(1, moveResult.FilesNotMoved.Count);
        Assert.Equal(1, moveResult.FilesMoved.Count);

        Assert.Equal(sourceFile1.FilePath, moveResult.FilesMoved[0].SourcePath);
        Assert.Equal(expectedfile1Path, moveResult.FilesMoved[0].DestinationPath);

        Assert.Equal(sourceFile2.FilePath, moveResult.FilesNotMoved[0].SourcePath);
        Assert.Equal(expectedfile2Path, moveResult.FilesNotMoved[0].DestinationPath);
    }

    [Fact]
    public void MoveData_MovesAllFolders()
    {
        // Arrange
        var sourceFolder1 = new SourceFolder("C:\\Users\\TestUser\\Downloads\\Unsorted\\Book 2");
        var expectedfolder1Path = "C:\\Users\\TestUser\\Downloads\\Book 2";

        var sourceFolder2 = new SourceFolder("C:\\Users\\TestUser\\Downloads\\Unsorted\\Movie 5");
        var expectedfolder2Path = "C:\\Users\\TestUser\\Downloads\\Movie 5";

        var fileMoverMock = new Mock<IFileMover>();
        fileMoverMock.Setup(f => f.MoveDirectory(sourceFolder1.FolderPath, expectedfolder1Path)).Returns(new MovedObject(sourceFolder1.FolderPath, expectedfolder1Path));
        fileMoverMock.Setup(f => f.MoveDirectory(sourceFolder2.FolderPath, expectedfolder2Path)).Returns(new MovedObject(sourceFolder2.FolderPath, expectedfolder2Path));

        var categorizedDataMover = new CategorizedDataMover(_configuration, fileMoverMock.Object);

        var categorizedData = new CategorizedData
        {
            CategorizedFolders = new List<CategorizedFolder>
            {
                new CategorizedFolder(sourceFolder1),
                new CategorizedFolder(sourceFolder2)
            }
        };

        // Act
        var moveResult = categorizedDataMover.MoveData(categorizedData);

        // Assert
        Assert.NotNull(moveResult);
        Assert.Empty(moveResult.FoldersNotMoved);
        Assert.Empty(moveResult.FilesMoved);
        Assert.Empty(moveResult.FilesNotMoved);

        Assert.Equal(2, moveResult.FoldersMoved.Count);

        Assert.Equal(sourceFolder1.FolderPath, moveResult.FoldersMoved[0].SourcePath);
        Assert.Equal(expectedfolder1Path, moveResult.FoldersMoved[0].DestinationPath);

        Assert.Equal(sourceFolder2.FolderPath, moveResult.FoldersMoved[1].SourcePath);
        Assert.Equal(expectedfolder2Path, moveResult.FoldersMoved[1].DestinationPath);
    }

    [Fact]
    public void MoveData_HandlesFailuresToMoveFolders()
    {
        // Arrange
        var sourceFile1 = new SourceFile("C:\\Users\\TestUser\\Downloads\\Unsorted\\file1.txt");
        var expectedfile1Path = "C:\\Users\\TestUser\\Downloads\\file1.txt";

        var sourceFile2 = new SourceFile("C:\\Users\\TestUser\\Downloads\\Unsorted\\file2.exe");
        var expectedfile2Path = "C:\\Users\\TestUser\\Downloads\\file2.exe";

        var fileMoverMock = new Mock<IFileMover>();
        fileMoverMock.Setup(f => f.MoveFile(sourceFile1.FilePath, expectedfile1Path)).Returns(new MovedObject(sourceFile1.FilePath, expectedfile1Path));
        fileMoverMock.Setup(f => f.MoveFile(sourceFile2.FilePath, expectedfile2Path)).Throws<DirectoryNotFoundException>();

        var categorizedDataMover = new CategorizedDataMover(_configuration, fileMoverMock.Object);

        var categorizedData = new CategorizedData
        {
            CategorizedFiles = new List<CategorizedFile>
            {
                new CategorizedFile(sourceFile1),
                new CategorizedFile(sourceFile2)
            }
        };

        // Act
        var moveResult = categorizedDataMover.MoveData(categorizedData);

        // Assert
        Assert.NotNull(moveResult);
        Assert.Empty(moveResult.FoldersMoved);
        Assert.Empty(moveResult.FoldersNotMoved);

        Assert.Equal(1, moveResult.FilesNotMoved.Count);
        Assert.Equal(1, moveResult.FilesMoved.Count);

        Assert.Equal(sourceFile1.FilePath, moveResult.FilesMoved[0].SourcePath);
        Assert.Equal(expectedfile1Path, moveResult.FilesMoved[0].DestinationPath);

        Assert.Equal(sourceFile2.FilePath, moveResult.FilesNotMoved[0].SourcePath);
        Assert.Equal(expectedfile2Path, moveResult.FilesNotMoved[0].DestinationPath);
    }
}
