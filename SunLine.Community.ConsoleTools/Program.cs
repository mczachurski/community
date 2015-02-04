using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Design;
using System.IO;
using System.Resources;
using SunLine.Community.Repositories.Migrations;

namespace SunLine.Community.ConsoleTools
{
    public class MainClass
    {
        private const string DbMigrationCommand = "dbmigration";
        private const string DbUpdateCommand = "dbupdate";

        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowHelp();
                return;
            }

            switch (args[0])
            {
                case DbMigrationCommand:

                    if (args.Length != 2)
                    {
                        ShowHelp();
                    }
                    else
                    {
                        GenerateDbMigration(args[1]);
                    }

                    break;
                case DbUpdateCommand:
                    RunDbMigration();
                    break;
                default:
                    ShowHelp();
                    break;
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Community console commands:");
            Console.WriteLine("dbmigration classname - creates new database migration");
            Console.WriteLine("dbupdate - update database");
        }

        private static void GenerateDbMigration(string className)
        {
            Console.Write("Generating migration class......");

            var config = new Configuration();
            var scaffolder = new MigrationScaffolder(config);
            var migration = scaffolder.Scaffold(className);

            File.WriteAllText(migration.MigrationId + ".cs", migration.UserCode);
            File.WriteAllText(migration.MigrationId + ".Designer.cs", migration.DesignerCode);
            using (var writer = new ResXResourceWriter(migration.MigrationId + ".resx"))
            {
                foreach (var resource in migration.Resources)
                {
                    writer.AddResource(resource.Key, resource.Value);
                }
            }

            Console.Write("OK\n");
        }

        private static void RunDbMigration()
        {
            Console.Write("Running migration......");

            var configuration = new Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();

            Console.Write("OK\n");
        }
    }
}
