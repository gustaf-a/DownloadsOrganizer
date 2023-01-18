using DownloadsOrganizer.Data;

namespace DownloadsOrganizer.Categorization.FileCategorization;

public interface IFileCategorizer
{
    public CategorizedFile Categorize(SourceFile file);
}
