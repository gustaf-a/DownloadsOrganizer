using DownloadsOrganizer.Data;

namespace DownloadsOrganizer.IO;

public interface ICategorizedDataMover
{
    public MoveResult MoveData(CategorizedData categorizedData);
}
