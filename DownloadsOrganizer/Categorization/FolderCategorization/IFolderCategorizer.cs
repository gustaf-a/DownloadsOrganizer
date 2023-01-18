using DownloadsOrganizer.Data;

namespace DownloadsOrganizer.Categorization.FolderCategorization;

public interface IFolderCategorizer
{
    public CategorizedFolder Categorize(SourceFolder categorizedFolder);
}
