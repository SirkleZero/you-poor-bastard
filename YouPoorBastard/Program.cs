using System;
using System.Configuration;
using YouPoorBastard.Model;

namespace YouPoorBastard
{
    class Program
    {
        static void Main(string[] args)
        {
            var dictionary = new WordList();
            var db = new Database(ConfigurationManager.AppSettings["databasePath"]);
            var users = db.GetUsers();

            foreach (var user in users)
            {
                var passwords = user.Password.Crack(dictionary);

                foreach (var password in passwords)
                {
                    Console.WriteLine(string.Format("{0}: {1}", user.Username, password));
                }
            }

            Console.ReadLine();
        }
    }
}