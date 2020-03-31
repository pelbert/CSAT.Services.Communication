using FTDNA.Database.Scripter;

namespace MigrationConsole
{
    class Program
    {
        static int Main(string[] args)
        {
            return DatabaseScripter.Execute(args);
        }
    }
}
