using System;
using System.IO;
using CommandLine;
using CommandLine.Text;
using PrimoSoftware.AVBlocks;

namespace CliSample
{
    class Options
    {
        // video_framerate command line options
        [Option('i', "input", HelpText = "MP4 input file.")]
        public string InputFile { get; set; }

        [Option('o', "output", HelpText = "MP4 output file.")]
        public string OutputFile { get; set; }

        [Option('f', "frame-rate", HelpText = "Target frame rate (fps).")]
        public double FrameRate { get; set; }

        public bool Error { get; private set; }

        void ResetOptions()
        {
            InputFile = null;
            OutputFile = null;
            FrameRate = 30.0;
            Error = false;
        }

        void SetDefaultOptions()
        {
            string exeDir = GetExeDir();
            InputFile = Path.Combine(exeDir, "../../assets/vid/big_buck_bunny_trailer.vid.mp4");
            OutputFile = Path.Combine(exeDir, "../../output/video_framerate/big_buck_bunny_30fps.mp4");
            FrameRate = 30.0;

            Console.WriteLine("Using default options: ");
            Console.Write("video_framerate");
            Console.Write(" --input " + InputFile);
            Console.Write(" --output " + OutputFile);
            Console.WriteLine();
        }

        void SetOptions(Options other)
        {
            InputFile = other.InputFile;
            OutputFile = other.OutputFile;
            FrameRate = other.FrameRate;
            Error = other.Error;
        }

        void SetOptions(string inputFile, string outputFile, double frameRate)
        {
            InputFile = inputFile;
            OutputFile = outputFile;
            FrameRate = frameRate;
            Error = false;
        }

        void PrintDefaults()
        {
            Console.WriteLine("Using default options: ");
            Console.Write("video_framerate");
            Console.Write(" --input " + InputFile);
            Console.Write(" --output " + OutputFile);
            Console.WriteLine($" --frame-rate {FrameRate}");
            Console.WriteLine();
        }

        public bool Prepare(string[] args)
        {
            ResetOptions();

            if (args == null || args.Length == 0)
            {
                SetDefaultOptions();
                PrintDefaults();
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
            if (InputFile == null)
            {
                Console.WriteLine("[not set]");
                res = false;
            }
            else
            {
                Console.WriteLine(OutputFile);
            }

            Console.WriteLine($"Frame rate: {FrameRate}");

            return res;
        }

        static string GetExeDir()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetDirectoryName(path);
        }
    }
}
