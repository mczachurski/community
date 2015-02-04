using System;
using System.Linq;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Repositories.Migrations.TableData.Dict
{
    public static class CategoryTable
    {
        public static void Initialize(IDatabaseContext context)
        {
            AddCategory(context, "Technology");
            AddCategory(context, "Business");
            AddCategory(context, "Design");
            AddCategory(context, "Food");
            AddCategory(context, "Marketing");
            AddCategory(context, "News");
            AddCategory(context, "Fashion");
            AddCategory(context, "Startups");
            AddCategory(context, "Photography");
            AddCategory(context, "Gaming");
            AddCategory(context, "Do It Yourself");
            AddCategory(context, "Beauty");
            AddCategory(context, "Comics");
            AddCategory(context, "Cars");
            AddCategory(context, "Education");
            AddCategory(context, "Science");
            AddCategory(context, "Finance");
            AddCategory(context, "Movie");
            AddCategory(context, "Travel");
            AddCategory(context, "Developing");
            AddCategory(context, "Hardware");
            AddCategory(context, "Software");
            AddCategory(context, "World");
            AddCategory(context, "Politics");
            AddCategory(context, "Health");
            AddCategory(context, "Arts");
            AddCategory(context, "Sports");
        }

        private static void AddCategory(IDatabaseContext context, string name)
        {
            if (!context.Categories.Any(x => x.Name == name))
            {
                context.Categories.Add(new Category { Name = name, CreationDate = DateTime.UtcNow, Version = 1 });
            }
        }
    }
}
