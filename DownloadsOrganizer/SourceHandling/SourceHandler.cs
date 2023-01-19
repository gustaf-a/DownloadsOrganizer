using DownloadsOrganizer.Configuration;
using DownloadsOrganizer.Data;
using DownloadsOrganizer.IO;
using Microsoft.Extensions.Configuration;

namespace DownloadsOrganizer.SourceHandling;

public class SourceHandler : ISourceHandler
{
    private readonly ApplicationOptions _applicationOptions;
    private readonly CategorizationOptions _categorizationOptions;

    private readonly IDirectoryReader _directoryReader;

    public SourceHandler(IConfiguration configuration, IDirectoryReader directoryReader)
    {
        _applicationOptions = configuration.GetSection(ApplicationOptions.Application).Get<ApplicationOptions>();
        _categorizationOptions = configuration.GetSection(CategorizationOptions.Categorization).Get<CategorizationOptions>();

        if ((_applicationOptions.SourceFolder.Length == 0)
            || string.IsNullOrWhiteSpace(_applicationOptions.OutputFolder))
            throw new ArgumentNullException(nameof(_applicationOptions));

        _directoryReader = directoryReader;
    }

    public SourceData GetSourceData()
    {
        var sourceData = new SourceData(_applicationOptions.SourceFolder.ToList());

        foreach (var rootPath in sourceData.RootPaths)
        {
            AddFiles(sourceData, rootPath);

            AddFolders(sourceData, rootPath);
        }

        return sourceData;
    }

    private void AddFiles(SourceData sourceData, string rootPath)
    {
        var files = _directoryReader.GetFiles(rootPath);

        foreach (var file in files)
        {
            if (FileShouldBeIgnored(file))
                continue;

            sourceData.SourceFiles.Add(new SourceFile(file));
        }
    }

    private bool FileShouldBeIgnored(string file)
    {
        var fileName = Path.GetFileName(file);
        var fileExtension = Path.GetExtension(file)[1..];

        return _categorizationOptions.IgnoreFileNames.Contains(fileName)
            || _categorizationOptions.IgnoreFilePrefixes.Any(prefix => fileName.StartsWith(prefix))
            || _categorizationOptions.IgnoreFileExtensions.Contains(fileExtension);
    }

    private void AddFolders(SourceData sourceData, string rootPath)
    {
        var folders = _directoryReader.GetDirectories(rootPath);

        foreach (var folder in folders)
        {
            if (FolderShouldBeIgnored(folder))
                continue;

            sourceData.SourceFolders.Add(new SourceFolder(folder));
        }
    }

    private bool FolderShouldBeIgnored(string folder)
    {
        var folderName = Path.GetFileName(folder);

        return _categorizationOptions.IgnoreFolderNames.Contains(folderName)
            || _categorizationOptions.IgnoreFolderPrefixes.Any(prefix => folderName.StartsWith(prefix));
    }
}
