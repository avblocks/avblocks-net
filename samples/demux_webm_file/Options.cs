using System;
using System.IO;
using CommandLine;
using CommandLine.Text;

namespace CliSample
{
    class Options
    {
        // demux_webm_file command line options
        [Option('i', "input", HelpText = "Input webm file")]
        public string InputFile { get; set; }

        [Option('o', "output", HelpText = "Output webm filename (without extension)")]
        public string OutputFile { get; set; }

        // The program parses the command line options and sets these properties
        public bool Error { get; private set; }

        void ResetOptions()
        {
            InputFile = null;
            OutputFile = null;
            Error = false;
        }

        void SetDefaultOptions()
        {
            string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            InputFile = Path.Combine(exeDir, "../../assets/mov/big-buck-bunny_trailer_vp8_vorbis.webm");
            OutputFile = Path.Combine(exeDir, "../../output/demux_webm_file/big-buck-bunny_trailer_vp8_vorbis");

            Console.WriteLine("Using default options: ");
            Console.Write(" --input " + InputFile);
            Console.Write(" --output " + OutputFile);
            Console.WriteLine();
        }

        void SetOptions(Options other)
        {
            InputFile = other.InputFile;
            OutputFile = other.OutputFile;
        }

        bool Validate()
        {
            bool res = true;

            Console.Write("Input file: ");
            if (InputFile == null)
            {
                Console.WriteLine("[not set]");
                res = false;
            }
            else
            {
                Console.WriteLine(InputFile);
            }

            Console.Write("Output file: ");
            if (OutputFile == null)
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
            
            return true;
        }
    }
}
