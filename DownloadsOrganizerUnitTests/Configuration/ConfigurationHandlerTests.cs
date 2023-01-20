using DownloadsOrganizer.Configuration;
using Microsoft.Extensions.Configuration;

namespace DownloadsOrganizerUnitTests.Configuration;

public class ConfigurationHandlerTests
{
    private readonly IConfiguration _configuration;

    public ConfigurationHandlerTests()
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
                    },
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
    public void CategorizationOptions_ReturnsCategorizationOptions()
    {
        // Arrange
        var configurationHandler = new ConfigurationHandler(_configuration);

        // Act
        var categorizationOptions = configurationHandler.CategorizationOptions();

        // Assert
        Assert.NotNull(categorizationOptions);
        Assert.NotNull(categorizationOptions.Categories);
        Assert.NotEmpty(categorizationOptions.Categories);
        Assert.Equal(2, categorizationOptions.Categories.Count());
    }

    [Fact]
    public void ApplicationOptions_ReturnsApplicationOptions()
    {
        // Arrange
        var configurationHandler = new ConfigurationHandler(_configuration);

        // Act
        var applicationOptions = configurationHandler.ApplicationOptions();

        // Assert
        Assert.NotNull(applicationOptions);
        Assert.NotNull(applicationOptions.SourceFolder);
        Assert.NotEmpty(applicationOptions.SourceFolder);
        Assert.Equal(1, applicationOptions.SourceFolder.Count());
        Assert.Equal("C:\\Users\\TestUser\\Downloads\\Unsorted", applicationOptions.SourceFolder.First());
        Assert.Equal("C:\\Users\\TestUser\\Downloads", applicationOptions.OutputFolder);
    }
}
