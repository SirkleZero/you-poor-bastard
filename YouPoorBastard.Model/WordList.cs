using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace YouPoorBastard.Model
{
    public class WordList
    {
        private List<string> words = new List<string>();

        public WordList()
        {
            this.Initialize();
        }

        public WordList(string dictionaryFile)
        {
            if (string.IsNullOrEmpty(dictionaryFile))
            {
                throw new ArgumentOutOfRangeException("dictionaryFile");
            }
            if (!File.Exists(dictionaryFile))
            {
                throw new DirectoryNotFoundException("The path specified by the value 'dictionaryFile' was not found");
            }

            this.Initialize(dictionaryFile);
        }

        public IEnumerable<string> Words
        {
            get
            {
                return this.words;
            }
        }

        #region private methods

        private void Initialize()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = new StreamReader(assembly.GetManifestResourceStream("YouPoorBastard.Model.WordList.txt")))
            {
                this.Initialize(stream);
            }

        }

        private void Initialize(string dictionaryFile)
        {
            using (var stream = File.OpenText(dictionaryFile))
            {
                this.Initialize(stream);
            }
        }

        private void Initialize(StreamReader stream)
        {
            string line;
            while ((line = stream.ReadLine()) != null)
            {
                this.words.Add(line);
            }
        }

        #endregion
    }
}