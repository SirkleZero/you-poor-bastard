using System;
using System.Collections.Generic;
using NDesk.Options;
using YouPoorBastard.Model;

namespace YouPoorBastard
{
    class Program
    {
        static void Main(string[] args)
        {
            string databasePath = string.Empty;
            string username = string.Empty;
            bool showHelp = false;

            var os = new OptionSet()
            {
                { "p|path=", "The directory where your Visual Source Safe database exists.", arg => databasePath = arg },
                { "u|user=", "(optional) The name of a specific user account that you want to crack.", arg => username = arg},
                { "h|help", "Gets you much needed help.", arg => showHelp = arg != null }
            };

            List<string> extra;
            try
            {
                extra = os.Parse(args);
            }
            catch (OptionException)
            {
                Console.WriteLine("Try 'YouPoorBastard.exe --help' for more information.");
                return;
            }

            if (showHelp)
            {
                ShowHelp(os);
                return;
            }

            if (string.IsNullOrEmpty(databasePath))
            {
                Console.WriteLine("You must specify a visual source safe database path. Try 'YouPoorBastard.exe --help' for more information.");
                return;
            }

            var dictionary = new WordList();
            var db = new Database(databasePath);
            if (string.IsNullOrEmpty(username))
            {
                // crack all the users
                var users = db.GetUsers();

                foreach (var user in users)
                {
                    PrintPasswords(user, dictionary);
                }
            }
            else
            {
                // crack a specific user
                var user = db.GetUser(username);
                PrintPasswords(user, dictionary);
            }

#if DEBUG
            Console.ReadLine();
#endif
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
    }
}