using DownloadsOrganizer.Configuration;
using DownloadsOrganizer.Data;
using Microsoft.Extensions.Configuration;

namespace DownloadsOrganizer.Categorization.FileCategorization;

public class FileCategorizer : IFileCategorizer
{
    private readonly CategorizationOptions _categorizationOptions;

    public FileCategorizer(IConfiguration configuration)
    {
        _categorizationOptions = configuration.GetSection(CategorizationOptions.Categorization)
                                              .Get<CategorizationOptions>();

        if (_categorizationOptions == null)
            throw new ArgumentNullException(nameof(_categorizationOptions));
    }

    public CategorizedFile Categorize(SourceFile file)
    {
        var categorizedFile = new CategorizedFile(file);

        foreach (var category in _categorizationOptions.Categories)
            if (category.FileExtensions.Contains(file.FileExtension))
            {
                categorizedFile.Category = category;
                break;
            }

        return categorizedFile;
    }
}
