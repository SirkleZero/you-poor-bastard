using System;
using System.Configuration;
using YouPoorBastard.Model;
using NDesk.Options;
using System.Collections.Generic;

namespace YouPoorBastard
{
    class Program
    {
        static void Main(string[] args)
        {
        //    var p = new OptionSet() {
        //    { "n|name=", "the {NAME} of someone to greet.",
        //      v => names.Add (v) },
        //    { "r|repeat=", 
        //        "the number of {TIMES} to repeat the greeting.\n" + 
        //            "this must be an integer.",
        //      (int v) => repeat = v },
        //    { "v", "increase debug message verbosity",
        //      v => { if (v != null) ++verbosity; } },
        //    { "h|help",  "show this message and exit", 
        //      v => show_help = v != null },
        //};
            string databasePath = string.Empty;
            string username = string.Empty;
            bool showHelp = false;

            var os = new OptionSet()
            {
                { "p|path=", "The {PATH} to the visual source safe database you want to crack.", arg => databasePath = arg },
                { "u|user=", "The name of a specific user that you want to crack.", arg => username = arg},
                { "h|help", "Gets you much needed help.", arg => showHelp = arg != null }
            };

            List<string> extra;
            try
            {
                extra = os.Parse(args);
            }
            catch (OptionException)
            {
                Console.WriteLine("Try 'YouPoorBastard --help' for more information.");
                return;
            }

            if (showHelp)
            {
                ShowHelp(os);
                return;
            }

            if (string.IsNullOrEmpty(databasePath))
            {
                Console.WriteLine("You must specify a visual source safe database path. Try 'YouPoorBastard --help' for more information.");
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

            Console.ReadLine();
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
            Console.WriteLine("Usage: YouPoorBastard [OPTIONS]");
            Console.WriteLine("Crack passwords of visual source safe users. If no specific user is specified, crack all users.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            optionSet.WriteOptionDescriptions(Console.Out);
            Console.ReadLine();
        }
    }
}