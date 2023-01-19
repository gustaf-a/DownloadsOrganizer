namespace DownloadsOrganizer.IO;

public interface IDirectoryReader
{
    public string[] GetFiles(string path);
    public string[] GetDirectories(string path);
}
