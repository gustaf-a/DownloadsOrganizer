using DownloadsOrganizer.Configuration;
using DownloadsOrganizer.Data;

namespace DownloadsOrganizer.Categorization.CategoriesHolder;

public class CategoriesHolder : ICategoriesHolder
{
    private const string _emptyCategoryName = "Empty";
    private const string _unknownCategoryName = "Unknown";

    private readonly List<Category> _categories;

    private readonly Category _emptyCategory;

    private readonly Category _unknownCategory;

    public CategoriesHolder(IConfigurationHandler configurationHandler)
    {
        _categories = configurationHandler.CategorizationOptions().Categories.ToList();

        _emptyCategory = new Category
        {
            Name = _emptyCategoryName
        };

        _unknownCategory = new Category
        {
            Name = _unknownCategoryName
        };
    }

    public List<Category> GetCategories()
        => _categories;

    public static bool IsEmpty(Category category) => _emptyCategoryName.Equals(category.Name);
    public Category GetEmptyCategory()
        => _emptyCategory;

    public static bool IsUnknown(Category category) => _unknownCategoryName.Equals(category.Name);
    public Category GetUnknownCategory()
        => _unknownCategory;
}
