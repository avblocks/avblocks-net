using System;
using System.IO;
using CommandLine;
using CommandLine.Text;

namespace CliSample
{
    class Options
    {
        // dump_hevc_au command line options
        [Option('i', "input", HelpText = "input file (HEVC/H.265)")]
        public string InputFile { get; set; }

        [Option('o', "output", HelpText = "output directory")]
        public string OutputDir { get; set; }

        public bool Error { get; private set; }

        void ResetOptions()
        {
            InputFile = null;
            OutputDir = null;
            Error = false;
        }

        void SetOptions(Options other)
        {
            InputFile = other.InputFile;
            OutputDir = other.OutputDir;
            Error = other.Error;
        }

        void SetDefaultOptions()
        {
            string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            InputFile = Path.Combine(exeDir, "../../assets/vid/foreman_qcif.h265");
            OutputDir = Path.Combine(exeDir, "../../output/dump_hevc_au");

            Console.WriteLine("Using default options: ");
            Console.WriteLine(" --input " + InputFile);
            Console.WriteLine(" --output " + OutputDir);
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
            if (string.IsNullOrEmpty(OutputDir))
            {
                Console.WriteLine("[not set]");
                res = false;
            }
            else
            {
                Console.WriteLine(OutputDir);
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
