namespace DownloadsOrganizer.Data;

public class CategorizedData
{
    public CategorizedData()
    {
        CategorizedFiles = new();
        CategorizedFolders = new();
    }

    public List<CategorizedFile> CategorizedFiles { get; set; }
    public List<CategorizedFolder> CategorizedFolders { get; set; }
}
