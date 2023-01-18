namespace DownloadsOrganizer.Data;

public class CategorizedFile
{
    public CategorizedFile(SourceFile sourceFile)
    {
        SourceFile = sourceFile;
    }

    public SourceFile SourceFile;

    public Category? Category { get; set; }
    public bool HasCategory => Category != null;

    public string FileName { get => SourceFile.FileName; }

    public string FilePath { get => SourceFile.FilePath; }

    public string FileExtension { get => SourceFile.FileExtension; }
}
