using DownloadsOrganizer.Configuration;
using DownloadsOrganizer.Data;
using DownloadsOrganizer.IO;

namespace DownloadsOrganizer.SourceHandling;

public class SourceHandler : ISourceHandler
{
    private readonly ApplicationOptions _applicationOptions;
    private readonly CategorizationOptions _categorizationOptions;

    private readonly IDirectoryReader _directoryReader;

    public SourceHandler(IConfigurationHandler configurationHandler, IDirectoryReader directoryReader)
    {
        _applicationOptions = configurationHandler.ApplicationOptions();
        _categorizationOptions = configurationHandler.CategorizationOptions();

        _directoryReader = directoryReader;
    }

    public SourceData GetSourceData()
    {
        if ((_applicationOptions.SourceFolder.Length == 0)
            || string.IsNullOrWhiteSpace(_applicationOptions.OutputFolder))
            throw new ArgumentNullException(nameof(_applicationOptions));

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

            var sourceFolder = new SourceFolder(folder);

            AddContainedFiles(sourceFolder);

            AddContainedFolders(sourceFolder);

            sourceData.SourceFolders.Add(sourceFolder);
        }
    }

    private void AddContainedFolders(SourceFolder sourceFolder)
    {
        var containedFolders = _directoryReader.GetDirectories(sourceFolder.FolderPath);
        if (containedFolders.Length > 0)
        {
            foreach (var containedFolder in containedFolders)
            {
                if (FolderShouldBeIgnored(containedFolder))
                {
                    sourceFolder.HasIgnoredContent = true;
                    continue;
                }

                sourceFolder.ContainedFolders.Add(new SourceFolder(containedFolder));
            }
        }
    }

    private void AddContainedFiles(SourceFolder sourceFolder)
    {
        var containedFiles = _directoryReader.GetFiles(sourceFolder.FolderPath);
        if (containedFiles.Length > 0)
        {
            foreach (var file in containedFiles)
            {
                if (FileShouldBeIgnored(file))
                {
                    sourceFolder.HasIgnoredContent = true;
                    continue;
                }

                sourceFolder.ContainedFiles.Add(new SourceFile(file));
            }
        }
    }

    private bool FolderShouldBeIgnored(string folder)
    {
        var folderName = Path.GetFileName(folder);

        return _categorizationOptions.IgnoreFolderNames.Contains(folderName)
            || _categorizationOptions.IgnoreFolderPrefixes.Any(prefix => folderName.StartsWith(prefix));
    }
}
