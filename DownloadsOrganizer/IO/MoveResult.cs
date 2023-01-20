using DownloadsOrganizer.Data;

namespace DownloadsOrganizer.IO;

public class MoveResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }

    public List<MovedObject> FilesMoved { get; }
    public List<MovedObject> FilesNotMoved { get; }
    public List<MovedObject> FoldersMoved { get; }
    public List<MovedObject> FoldersNotMoved { get; }

    public MoveResult()
    {
        FilesMoved = new();
        FilesNotMoved = new();
        
        FoldersMoved = new();
        FoldersNotMoved = new();
    }

    public void MoveFinished(bool success, string? errorMessage = null)
    {
        Success = success;
        ErrorMessage = errorMessage;
    }
}
