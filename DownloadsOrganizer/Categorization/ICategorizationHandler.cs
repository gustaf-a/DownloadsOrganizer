using DownloadsOrganizer.Data;

namespace DownloadsOrganizer.Categorization;

public interface ICategorizationHandler
{
    public CategorizedData Categorize(SourceData sourceData);
}
