using System;
using System.IO;
using ManyConsole;
using TreeHouse.Database;

namespace TreehouseDatabaseCLI.Commands
{
    public abstract class BaseCommand : ConsoleCommand
    {
        public string FileLocation { get; set; }
        protected BaseCommand(string name, string shortDesc, string longDesc = null)
        {
            IsCommand(name, shortDesc);

            if(!string.IsNullOrWhiteSpace(longDesc)) HasLongDescription(longDesc);

            HasOption("f|file=", "The full path of the file.", p => FileLocation = p);
        }
        protected abstract void Run();
        public override int Run(string[] remainingArguments)
        {
            try
            {
                Run();
                return 0;
            }
            catch(Exception e)
            {
                var originalColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(e.Message);
                Console.ForegroundColor = originalColor;
                return 2;
            }
        }

        public TreeHouseContext CreateConnection()
        {
            var fileToUse = string.IsNullOrWhiteSpace(FileLocation) ? "TreeHouse.sqlite" : FileLocation;
            if(!File.Exists(fileToUse)) throw new Exception("Cannot find database file.");

            return new TreeHouseContext(fileToUse);
        }
    }
}
