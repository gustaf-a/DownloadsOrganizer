using DownloadsOrganizer.Data;

namespace DownloadsOrganizer.IO;

public class FileMover : IFileMover
{
    public MovedObject MoveDirectory(string sourcePath, string destinationPath)
    {
        ValidatePaths(sourcePath, destinationPath);

        EnsureDestinationExists(destinationPath);
      
        if (!Directory.Exists(sourcePath))
            throw new DirectoryNotFoundException(sourcePath);

        Directory.Move(sourcePath, destinationPath);

        return new MovedObject(sourcePath, destinationPath);
    }

    public MovedObject MoveFile(string sourcePath, string destinationPath)
    {
        ValidatePaths(sourcePath, destinationPath);

        EnsureDestinationExists(destinationPath);
        
        if (!File.Exists(sourcePath))
            throw new FileNotFoundException(sourcePath);

        File.Move(sourcePath, destinationPath);

        return new MovedObject(sourcePath, destinationPath);
    }

    private void ValidatePaths(string sourcePath, string destinationPath)
    {
        if (string.IsNullOrWhiteSpace(sourcePath))
            throw new ArgumentNullException(sourcePath);

        if (string.IsNullOrWhiteSpace(destinationPath))
            throw new ArgumentNullException(sourcePath);
    }

    private void EnsureDestinationExists(string destinationPath)
    {
        if (!Directory.Exists(destinationPath))
            Directory.CreateDirectory(destinationPath);
    }
}
