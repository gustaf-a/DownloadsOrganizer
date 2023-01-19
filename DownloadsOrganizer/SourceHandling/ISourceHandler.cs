using DownloadsOrganizer.Data;

namespace DownloadsOrganizer.SourceHandling;

public interface ISourceHandler
{
    public SourceData GetSourceData();
}
