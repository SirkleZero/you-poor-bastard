using System;
using System.IO;
using NDesk.Options;

namespace YouPoorBastard
{
    public class CommandLineArgs
    {
        private readonly string[] args;

        public CommandLineArgs(string[] args)
        {
            this.args = args;
        }

        public OptionSet Options { get; private set; }
        public string DatabasePath { get; private set; }
        public string Username { get; private set; }
        public string ExportPath { get; private set; }
        public bool ShowHelp { get; private set; }

        public void Initialize(TextWriter writer)
        {
            this.Options = new OptionSet()
            {
                { "p|path=", "The directory where your Visual Source Safe database exists.", arg => this.DatabasePath = arg },
                { "u|user=", "(optional) The name of a specific user account that you want to crack.", arg => this.Username = arg },
                { "e|export=", "The full path to the file where you want the export to be saved.", arg => this.ExportPath = arg },
                { "h|help", "Gets you much needed help.", arg => this.ShowHelp = arg != null }
            };

            try
            {
                this.Options.Parse(this.args);
            }
            catch (OptionException)
            {
                writer.WriteLine(Program.HelpInformationMessage);
                return;
            }
        }
    }
}