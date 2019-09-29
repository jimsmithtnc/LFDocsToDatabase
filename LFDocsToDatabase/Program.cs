using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;

namespace LFDocsToDatabase
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
              .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts))
              .WithNotParsed<Options>((errs) => HandleParseError(errs));
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            throw new NotImplementedException();
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            if (Directory.Exists(opts.DocumentFolderPath))
            {
                // This path is a directory
                ProcessDirectory(opts.DocumentFolderPath);
            }
            else
            {
                throw new DirectoryNotFoundException(opts.DocumentFolderPath);
            }
        }

        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);
        }

        public static void ProcessFile(string path)
        {
            Console.WriteLine("Processing file '{0}'.", path);
            DataExtractor de = new DataExtractor(path);
            DataDto data = de.GetData();
            DataPersistor.SaveDataToDatabase(data);
            Console.WriteLine("  -- Processed file." + Environment.NewLine);
        }
    }

    internal class Options
    {
        [Option('d', "docfolder", Required = true, HelpText = "Path to folder holding documents to parse.")]
        public string DocumentFolderPath { get; set; }
    }
}
