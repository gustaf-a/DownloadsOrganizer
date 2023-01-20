using DownloadsOrganizer.Configuration;
using DownloadsOrganizer.Data;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace DownloadsOrganizer.IO;

public class CategorizedDataMover : ICategorizedDataMover
{
    private readonly ApplicationOptions _applicationOptions;

    private readonly IFileMover _fileMover;

    public CategorizedDataMover(IConfiguration configuration, IFileMover fileMover)
    {
        _applicationOptions = configuration.GetSection(ApplicationOptions.Application).Get<ApplicationOptions>();

        _fileMover = fileMover;
    }

    public MoveResult MoveData(CategorizedData categorizedData)
    {
        var moveResult = new MoveResult();

        MoveFiles(categorizedData, moveResult);

        MoveFolders(categorizedData, moveResult);

        return moveResult;
    }

    private void MoveFiles(CategorizedData categorizedData, MoveResult moveResult)
    {
        foreach (var file in categorizedData.CategorizedFiles)
        {
            try
            {
                moveResult.FilesMoved.Add(_fileMover.MoveFile(file.FilePath, GetDestinationPath(file.FileName)));
            }
            catch (Exception e)
            {
                Log.Error(e, $"Failed to move {file.FilePath} to {_applicationOptions.OutputFolder}: {e.Message}.");
                moveResult.FilesNotMoved.Add(new MovedObject(file.FilePath, GetDestinationPath(file.FileName)));
            }
        }
    }

    private void MoveFolders(CategorizedData categorizedData, MoveResult moveResult)
    {
        foreach (var folder in categorizedData.CategorizedFolders)
        {
            try
            {
                moveResult.FoldersMoved.Add(_fileMover.MoveDirectory(folder.FolderPath, GetDestinationPath(folder.FolderName)));
            }
            catch (Exception e)
            {
                Log.Error(e, $"Failed to move {folder.FolderPath} to {_applicationOptions.OutputFolder}: {e.Message}.");
                moveResult.FoldersNotMoved.Add(new MovedObject(folder.FolderPath, GetDestinationPath(folder.FolderName)));
            }
        }
    }

    private string GetDestinationPath(string name)
    {
        return Path.Combine(_applicationOptions.OutputFolder, name);
    }
}
