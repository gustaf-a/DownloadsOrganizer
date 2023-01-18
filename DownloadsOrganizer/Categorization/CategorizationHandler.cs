using DownloadsOrganizer.Categorization.FileCategorization;
using DownloadsOrganizer.Categorization.FolderCategorization;
using DownloadsOrganizer.Data;

namespace DownloadsOrganizer.Categorization;

public class CategorizationHandler : ICategorizationHandler
{
    private readonly IFileCategorizer _fileCategorizer;
    private readonly IFolderCategorizer _folderCategorizer;

    public CategorizationHandler(IFileCategorizer fileCategorizer, IFolderCategorizer folderCategorizer)
    {
        _fileCategorizer = fileCategorizer;
        _folderCategorizer = folderCategorizer;
    }

    public CategorizedData Categorize(SourceData sourceData)
    {
        var categorizedData = new CategorizedData();

        foreach (var file in sourceData.SourceFiles)
            categorizedData.CategorizedFiles.Add(_fileCategorizer.Categorize(file));

        foreach (var folder in sourceData.SourceFolders)
            categorizedData.CategorizedFolders.Add(_folderCategorizer.Categorize(folder));
        
        return categorizedData;
    }
}
