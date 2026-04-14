using System;
using System.IO;
using CommandLine;
using CommandLine.Text;

namespace CliSample
{
    class Options
    {
        // video_upscale command line options
        [Option('i', "input", HelpText = "MP4 input file.")]
        public string InputFile { get; set; }

        [Option('o', "output", HelpText = "MP4 output file.")]
        public string OutputFile { get; set; }

        [Option('w', "width", HelpText = "Target width (pixels).")]
        public int Width { get; set; }

        [Option('h', "height", HelpText = "Target height (pixels).")]
        public int Height { get; set; }

        public bool Error { get; private set; }

        void ResetOptions()
        {
            InputFile = null;
            OutputFile = null;
            Width = 1920;
            Height = 1080;
            Error = false;
        }

        void SetDefaultOptions()
        {
            string exeDir = GetExeDir();
            InputFile = Path.Combine(exeDir, "../../assets/vid/big_buck_bunny_trailer.vid.mp4");
            OutputFile = Path.Combine(exeDir, "../../output/video_upscale/big_buck_bunny_1080p.mp4");
            Width = 1920;
            Height = 1080;

            Console.WriteLine("Using default options: ");
            Console.Write("video_upscale");
            Console.Write(" --input " + InputFile);
            Console.Write(" --output " + OutputFile);
            Console.WriteLine();
        }

        void SetOptions(Options other)
        {
            InputFile = other.InputFile;
            OutputFile = other.OutputFile;
            Width = other.Width;
            Height = other.Height;
            Error = other.Error;
        }

        void SetOptions(string inputFile, string outputFile, int width, int height)
        {
            InputFile = inputFile;
            OutputFile = outputFile;
            Width = width;
            Height = height;
            Error = false;
        }

        void PrintDefaults()
        {
            Console.WriteLine("Using default options: ");
            Console.Write("video_upscale");
            Console.Write(" --input " + InputFile);
            Console.Write(" --output " + OutputFile);
            Console.WriteLine($" --width {Width}");
            Console.WriteLine($" --height {Height}");
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

            Console.WriteLine($"Width: {Width}");
            Console.WriteLine($"Height: {Height}");

            return res;
        }

        static string GetExeDir()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetDirectoryName(path);
        }
    }
}
