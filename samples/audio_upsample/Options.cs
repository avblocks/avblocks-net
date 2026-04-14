using System;
using System.IO;
using System.Reflection;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace CliSample
{
    class Options
    {
        // audio_upsample command line options
        [Option('i', "input", HelpText = "input MP3 file")]
        public string InputFile { get; set; }

        [Option('o', "output", HelpText = "output MP3 file")]
        public string OutputFile { get; set; }

        public bool Error { get; private set; }
        public bool Help { get; set; }

        void ResetOptions()
        {
            InputFile = null;
            OutputFile = null;
            Error = false;
            Help = false;
        }

        void SetOptions(Options other)
        {
            InputFile = other.InputFile;
            OutputFile = other.OutputFile;
            Error = other.Error;
            Help = other.Help;
        }

        void SetDefaultOptions()
        {
            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            InputFile = Path.Combine(exeDir, "../../assets/aud/Hydrate-Kenny_Beltrey.mp3");

            string outputDir = Path.Combine(exeDir, "../../output/audio_upsample");
            Directory.CreateDirectory(outputDir);
            
            string fileName = Path.GetFileName(InputFile);
            OutputFile = Path.Combine(outputDir, fileName + "-48khz.mp3");

            Console.WriteLine("Using default options: ");
            Console.WriteLine(" --input " + InputFile);
            Console.WriteLine(" --output " + OutputFile);
            Console.WriteLine();
        }

        bool Validate()
        {
            bool res = true;

            Console.Write("--input: ");
            if (string.IsNullOrEmpty(InputFile))
            {
                Console.WriteLine("[not set]");
                res = false;
            }
            else
            {
                Console.WriteLine(InputFile);
            }

            Console.Write("--output: ");
            if (string.IsNullOrEmpty(OutputFile))
            {
                Console.WriteLine("[not set]");
                res = false;
            }
            else
            {
                Console.WriteLine(OutputFile);
            }

            return res;
        }

        public bool Prepare(string[] args)
        {
            ResetOptions();

            if (args.Length == 0)
            {
                SetDefaultOptions();
                return true;
            }

            bool parseError = false;
            var parserResult = Parser.Default.ParseArguments<Options>(args).WithNotParsed(_ => { parseError = true; });
            if (parseError)
            {
                Error = true;
                return false;
            }

            SetOptions(parserResult.Value);

            if (!Validate())
            {
                var helpText = HelpText.AutoBuild(parserResult, h => h, e => e);
                Console.WriteLine($"Usage:\n{helpText}");

                Error = true;
                return false;
            }

            Error = false;
            return true;
        }
    }
}
