using System;
using System.Linq;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Repositories.Migrations.TableData.Dict
{
    public static class CategoryFavouriteLevelTable
    {
        public static void Initialize(IDatabaseContext context)
        {
            AddCategoryFavouriteLevel(context, "Popular", 100);
            AddCategoryFavouriteLevel(context, "Very popular", 1000);
            AddCategoryFavouriteLevel(context, "Extreme popular", 10000);
        }

        private static void AddCategoryFavouriteLevel(IDatabaseContext context, string name, int favouriteLevel)
        {
            if (!context.CategoryFavouriteLevels.Any(x => x.Name == name))
            {
                context.CategoryFavouriteLevels.Add(new CategoryFavouriteLevel { Name = name, FavouriteLevel = favouriteLevel, CreationDate = DateTime.UtcNow, Version = 1 });
            }
        }
    }
}

