using System;
using System.Collections.Generic;
using NDesk.Options;
using YouPoorBastard.Model;

namespace YouPoorBastard
{
    class Program
    {
        public const string HelpInformationMessage = "Try 'YouPoorBastard.exe --help' for more information.";
        public const string ExportSuccessMessage = "Data exported to '{0}'";

        static void Main(string[] args)
        {
            var arguments = new CommandLineArgs(args);
            arguments.Initialize(Console.Out);

            if (arguments.ShowHelp)
            {
                Program.ShowHelp(arguments.Options);
                return;
            }

            if (string.IsNullOrEmpty(arguments.DatabasePath))
            {
                Console.WriteLine(string.Concat("You must specify a visual source safe database path. " + HelpInformationMessage));
                return;
            }

            if (!string.IsNullOrEmpty(arguments.Username))
            {
                Program.CrackSingle(arguments.DatabasePath, arguments.Username, arguments.ExportPath);
            }
            else
            {
                Program.CrackAll(arguments.DatabasePath, arguments.ExportPath);
            }
#if DEBUG
            Console.ReadLine();
#endif
        }

        private static void CrackSingle(string databasePath, string username, string exportPath)
        {
            var dictionary = new WordList();
            try
            {
                var db = new Database(databasePath);
                if (string.IsNullOrEmpty(exportPath))
                {
                    var user = db.GetUser(username);
                    if (user == null)
                    {
                        Console.WriteLine(string.Format("The username '{0}' was not found.", username));
                        return;
                    }
                    Program.PrintPasswords(user, dictionary);
                }
                else
                {
                    db.Export(exportPath, dictionary, username);
                    Program.WriteExportSuccessMessage(exportPath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(string.Format("Unable to crack passwords. The error message was: '{0}'", e.Message));
                Console.WriteLine();
                return;
            }
        }

        private static void CrackAll(string databasePath, string exportPath)
        {
            var dictionary = new WordList();
            try
            {
                var db = new Database(databasePath);
                if (string.IsNullOrEmpty(exportPath))
                {
                    var users = db.GetUsers();
                    foreach (var user in users)
                    {
                        Program.PrintPasswords(user, dictionary);
                    }
                }
                else
                {
                    db.Export(exportPath, dictionary);
                    Program.WriteExportSuccessMessage(exportPath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(string.Format("Unable to crack passwords. The error message was: '{0}'", e.Message));
                Console.WriteLine();
                return;
            }
        }

        private static void PrintPasswords(User user, WordList dictionary)
        {
            var passwords = user.Password.Crack(dictionary);
            foreach (var password in passwords)
            {
                Console.WriteLine(string.Format("{0}: {1}", user.Username, password));
            }
        }

        private static void ShowHelp(OptionSet optionSet)
        {
            Console.WriteLine();
            Console.WriteLine("Crack passwords of visual source safe users.");
            Console.WriteLine();
            Console.WriteLine("Usage: YouPoorBastard.exe [OPTIONS]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            optionSet.WriteOptionDescriptions(Console.Out);

            Console.WriteLine();
            Console.WriteLine("Examples:");

            Console.WriteLine("Crack passwords for all users.");
            Console.WriteLine("YouPoorBastard.exe -p 'C:\\SomeDataDirectory\\SomeVSSDirectory\\'");

            Console.WriteLine("");
            Console.WriteLine("Crack passwords for user 'jdoe'");
            Console.WriteLine("YouPoorBastard.exe -p 'C:\\SomeDataDirectory\\SomeVSSDirectory\\' -u jdoe");
#if DEBUG
            Console.ReadLine();
#endif
        }

        private static void WriteExportSuccessMessage(string exportPath)
        {
            Console.WriteLine();

            Console.WriteLine(string.Format(Program.ExportSuccessMessage, exportPath));
#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}