using System;
using System.IO;
using CommandLine;
using CommandLine.Text;

namespace CliSample
{
    class Options
    {
        // enc_g711_alaw_file command line options
        [Option('i', "input", HelpText = "input WAV file")]
        public string InputFile { get; set; }

        [Option('o', "output", HelpText = "output G.711 A-law WAV file")]
        public string OutputFile { get; set; }

        public bool Error { get; private set; }

        void ResetOptions()
        {
            InputFile = null;
            OutputFile = null;
            Error = false;
        }

        void SetOptions(Options other)
        {
            InputFile = other.InputFile;
            OutputFile = other.OutputFile;
            Error = other.Error;
        }

        void SetDefaultOptions()
        {
            string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            InputFile = Path.Combine(exeDir, "../../assets/aud/express-dictate_8000_s16_1ch_pcm.wav");
            OutputFile = Path.Combine(exeDir, "../../output/enc_g711_alaw_file/express-dictate_g711_alaw.wav");

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
