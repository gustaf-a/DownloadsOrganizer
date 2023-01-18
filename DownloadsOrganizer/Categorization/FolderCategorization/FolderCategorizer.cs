using DownloadsOrganizer.Categorization.CategoriesHolder;
using DownloadsOrganizer.Categorization.FileCategorization;
using DownloadsOrganizer.Data;

namespace DownloadsOrganizer.Categorization.FolderCategorization;

public class FolderCategorizer : IFolderCategorizer
{
    private readonly IFileCategorizer _fileCategorizer;

    private readonly ICategoriesHolder _categoriesHolder;

    public FolderCategorizer(IFileCategorizer fileCategorizer, ICategoriesHolder categoriesHolder)
    {
        _categoriesHolder = categoriesHolder;
        _fileCategorizer = fileCategorizer;
    }

    public CategorizedFolder Categorize(SourceFolder sourceFolder)
    {
        var categorizedFolder = new CategorizedFolder(sourceFolder);

        if (sourceFolder.ContainedFiles.Count == 0)
        {
            categorizedFolder.Category = _categoriesHolder.GetEmptyCategory();
            return categorizedFolder;
        }

        CategorizeAllFiles(categorizedFolder, sourceFolder.ContainedFiles);

        categorizedFolder.Category = GetFolderCategory(categorizedFolder);

        return categorizedFolder;
    }

    private void CategorizeAllFiles(CategorizedFolder categorizedFolder, List<SourceFile> containedFiles)
    {
        foreach (var file in containedFiles)
            categorizedFolder.ContainedFiles.Add(_fileCategorizer.Categorize(file));
    }

    private Category GetFolderCategory(CategorizedFolder categorizedFolder)
    {
        var categoriesByOccurence = GetCategoryOccurences(categorizedFolder.ContainedFiles);

        return GetDominantCategory(categoriesByOccurence);
    }

    private static Dictionary<Category, int> GetCategoryOccurences(List<CategorizedFile> categorizedFiles)
       => categorizedFiles.GroupBy(file => file.Category)
                           .Select(group => new
                           {
                               Category = group.Key,
                               Occurences = group.Count()
                           })
                           .OrderByDescending(group => group.Occurences)
                           .ToDictionary(c => c.Category, c => c.Occurences);

    private Category GetDominantCategory(Dictionary<Category, int> categoriesByOccurence)
    {
        if (categoriesByOccurence.Count == 0)
            throw new Exception("No categories found.");

        if (categoriesByOccurence.Count == 1)
            return categoriesByOccurence.Keys.First();

        if (OneCategoryIsMostCommon(categoriesByOccurence))
            return MostCommonCategory(categoriesByOccurence);

        return _categoriesHolder.GetUnknownCategory();
    }

    private static bool OneCategoryIsMostCommon(Dictionary<Category, int> categoriesByOccurence)
        => categoriesByOccurence.First().Value > categoriesByOccurence.Values.ElementAt(1);

    private static Category MostCommonCategory(Dictionary<Category, int> categoriesByOccurence)
        => categoriesByOccurence.Keys.First();

   

}
