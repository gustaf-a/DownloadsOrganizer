namespace DownloadsOrganizer.Data;

public class SourceFile
{
    public SourceFile(string filePath)
    {
        FilePath = filePath;
    }

    public string FilePath { get; }

    public string FileName { get => Path.GetFileName(FilePath); }

    public string FileExtension
    {
        get
        {
            var extension = Path.GetExtension(FilePath);

            return extension[0] == '.' ? extension[1..] : extension;
        }
    }
}