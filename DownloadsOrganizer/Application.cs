using DownloadsOrganizer.Categorization;
using DownloadsOrganizer.IO;
using DownloadsOrganizer.SourceHandling;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DownloadsOrganizer;

public class Application : IHostedService
{
    private readonly ICategorizationHandler _categorizationHandler;
    private readonly ICategorizedDataMover _categorizedDataMover;
    private readonly ISourceHandler _sourceHandler;

    public Application(ISourceHandler sourceHandler,
                        ICategorizationHandler categorizationHandler,
                        ICategorizedDataMover categorizedDataMover)
    {
        Log.Information("Starting application");

        _categorizationHandler = categorizationHandler;
        _categorizedDataMover = categorizedDataMover;
        _sourceHandler = sourceHandler;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        //------- Create list of changes to make ------------
        Log.Information("Getting source files");
        var allDataSources = _sourceHandler.GetSourceData();

        Log.Information("Categorizing source files");
        var categorizedData = _categorizationHandler.Categorize(allDataSources);

        //------- Move files ------------
        Log.Information("Moving files");
        var moveResult = _categorizedDataMover.MoveData(categorizedData);

        //------- Show results ------------
        RecordResults(moveResult);
    }

    private static void RecordResults(MoveResult moveResult)
    {
        Log.Information("Results");

        Log.Information("Files moved: {0}", moveResult.FilesMoved);
        Log.Information("Folders created: {0}", moveResult.FoldersMoved);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
