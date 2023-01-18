using DownloadsOrganizer.Categorization.CategoriesHolder;
using DownloadsOrganizer.Data;

namespace DownloadsOrganizer.Categorization.FileCategorization;

public class FileCategorizer : IFileCategorizer
{
    private readonly ICategoriesHolder _categoriesHolder;

    public FileCategorizer(ICategoriesHolder categoriesHolder)
    {
        _categoriesHolder = categoriesHolder;
    }

    public CategorizedFile Categorize(SourceFile file)
    {
        var categorizedFile = new CategorizedFile(file);

        foreach (var category in _categoriesHolder.GetCategories())
            if (category.FileExtensions.Contains(file.FileExtension))
            {
                categorizedFile.Category = category;
                return categorizedFile;
            }

        categorizedFile.Category = _categoriesHolder.GetUnknownCategory();

        return categorizedFile;
    }
}
