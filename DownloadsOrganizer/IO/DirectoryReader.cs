namespace DownloadsOrganizer.IO;

public class DirectoryReader : IDirectoryReader
{
    public string[] GetDirectories(string path)
    {
        ValidateFolderPath(path);

        return Directory.GetDirectories(path);
    }

    public string[] GetFiles(string path)
    {
        ValidateFolderPath(path);

        return Directory.GetFiles(path);
    }

    private static void ValidateFolderPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException(nameof(path));

        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException(path);
    }
}
