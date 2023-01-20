using DownloadsOrganizer.Data;

namespace DownloadsOrganizer.IO;

public interface IFileMover
{
    public MovedObject MoveFile(string sourcePath, string destinationPath);
    public MovedObject MoveDirectory(string sourcePath, string destinationPath);
}
