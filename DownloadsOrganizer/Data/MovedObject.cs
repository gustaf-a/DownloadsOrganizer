namespace DownloadsOrganizer.Data;

public class MovedObject
{
    public string SourcePath { get; }
    public string DestinationPath { get; }

    public MovedObject(string sourcePath, string destinationPath)
    {
        SourcePath = sourcePath;
        DestinationPath = destinationPath;
    }
}
