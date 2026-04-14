using System;
using System.IO;
using CommandLine;
using CommandLine.Text;
using PrimoSoftware.AVBlocks;

namespace CliSample
{
    class Options
    {
        // video_pad command line options
        [Option('i', "input", HelpText = "MP4 input file.")]
        public string InputFile { get; set; }

        [Option('o', "output", HelpText = "MP4 output file.")]
        public string OutputFile { get; set; }

        [Option('w', "width", HelpText = "Target width (pixels).")]
        public int Width { get; set; }

        [Option('h', "height", HelpText = "Target height (pixels).")]
        public int Height { get; set; }

        [Option('l', "left", HelpText = "Left padding (pixels).")]
        public int PadLeft { get; set; }

        [Option('r', "right", HelpText = "Right padding (pixels).")]
        public int PadRight { get; set; }

        [Option('t', "top", HelpText = "Top padding (pixels).")]
        public int PadTop { get; set; }

        [Option('b', "bottom", HelpText = "Bottom padding (pixels).")]
        public int PadBottom { get; set; }

        [Option('c', "color", HelpText = "Padding color (ARGB32, default: 0xFF000000 - black).")]
        public uint PadColor { get; set; }

        public bool Error { get; private set; }

        void ResetOptions()
        {
            InputFile = null;
            OutputFile = null;
            Width = 1920;
            Height = 1080;
            PadLeft = 0;
            PadRight = 0;
            PadTop = 0;
            PadBottom = 0;
            PadColor = 0xFF000000;
            Error = false;
        }

        void SetDefaultOptions()
        {
            string exeDir = GetExeDir();
            InputFile = Path.Combine(exeDir, "../../assets/vid/big_buck_bunny_trailer.vid.mp4");
            OutputFile = Path.Combine(exeDir, "../../output/video_pad/big_buck_bunny_padded.mp4");
            Width = 1920;
            Height = 1080;
            PadLeft = 0;
            PadRight = 0;
            PadTop = 0;
            PadBottom = 0;
            PadColor = 0xFF000000;

            Console.WriteLine("Using default options: ");
            Console.Write("video_pad");
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
            PadLeft = other.PadLeft;
            PadRight = other.PadRight;
            PadTop = other.PadTop;
            PadBottom = other.PadBottom;
            PadColor = other.PadColor;
            Error = other.Error;
        }

        void SetOptions(string inputFile, string outputFile, int width, int height, int padLeft, int padRight, int padTop, int padBottom, uint padColor)
        {
            InputFile = inputFile;
            OutputFile = outputFile;
            Width = width;
            Height = height;
            PadLeft = padLeft;
            PadRight = padRight;
            PadTop = padTop;
            PadBottom = padBottom;
            PadColor = padColor;
            Error = false;
        }

        void PrintDefaults()
        {
            Console.WriteLine("Using default options: ");
            Console.Write("video_pad");
            Console.Write(" --input " + InputFile);
            Console.Write(" --output " + OutputFile);
            Console.WriteLine($" --width {Width}");
            Console.WriteLine($" --height {Height}");
            Console.WriteLine($" --left {PadLeft}");
            Console.WriteLine($" --right {PadRight}");
            Console.WriteLine($" --top {PadTop}");
            Console.WriteLine($" --bottom {PadBottom}");
            Console.WriteLine($" --color {PadColor:X8}");
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
            Console.WriteLine($"Left padding: {PadLeft}");
            Console.WriteLine($"Right padding: {PadRight}");
            Console.WriteLine($"Top padding: {PadTop}");
            Console.WriteLine($"Bottom padding: {PadBottom}");
            Console.WriteLine($"Color: {PadColor:X8}");

            return res;
        }

        static string GetExeDir()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetDirectoryName(path);
        }
    }
}
