namespace DownloadsOrganizer.Data;

public class CategorizedFolder
{
    public CategorizedFolder(SourceFolder sourceFolder)
    {
        SourceFolder = sourceFolder;

        ContainedFiles = new();
    }

    public SourceFolder SourceFolder { get; }

    public List<CategorizedFile> ContainedFiles { get; set; }

    public Category Category { get; set; }
    public bool HasCategory { get => Category != null; }

    public string FolderPath { get => SourceFolder.FolderPath; }
    public string FolderName { get => SourceFolder.FolderName; }
}
