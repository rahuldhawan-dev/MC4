using System;
using exporter.Commands;
using Oakton;

namespace exporter
{
    class Program
    {
        public enum ExitCodes
        {
            Success = 0,
            OutputDirectoryDoesNotExist,
            InputScriptDirectoryDoesNotExist
        }

        static int Main(string[] args)
        {
            return CommandExecutor.ExecuteCommand<ExportCommand>(args);
        }

        public static void Exit(ExitCodes exitCode)
        {
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
            Environment.Exit((int)exitCode);
        }
    }
}
