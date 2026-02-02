using System;
using System.IO;
using CommandLine;
using CommandLine.Text;

namespace CliSample
{
    class Options
    {
        // dec_vp9_file command line options
        [Option('i', "input", HelpText = "input VP9/IVF file")]
        public string InputFile { get; set; }

        [Option('o', "output", HelpText = "output YUV file")]
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
            InputFile = Path.Combine(exeDir, "../../assets/vid/foreman_qcif_vp9.ivf");
            OutputFile = Path.Combine(exeDir, "../../output/dec_vp9_file/foreman_qcif.yuv");

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
