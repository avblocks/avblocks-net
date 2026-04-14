using System;
using System.IO;
using CommandLine;
using CommandLine.Text;

namespace CliSample
{
    class Options
    {
        // video_crop command line options
        [Option('i', "input", HelpText = "MP4 input file.")]
        public string InputFile { get; set; }

        [Option('o', "output", HelpText = "MP4 output file.")]
        public string OutputFile { get; set; }

        [Option("crop-left", HelpText = "Pixels to crop from left.")]
        public int CropLeft { get; set; }

        [Option("crop-right", HelpText = "Pixels to crop from right.")]
        public int CropRight { get; set; }

        [Option("crop-top", HelpText = "Pixels to crop from top.")]
        public int CropTop { get; set; }

        [Option("crop-bottom", HelpText = "Pixels to crop from bottom.")]
        public int CropBottom { get; set; }

        public bool Error { get; private set; }

        void ResetOptions()
        {
            InputFile = null;
            OutputFile = null;
            CropLeft = 0;
            CropRight = 0;
            CropTop = 0;
            CropBottom = 0;
            Error = false;
        }

        void SetOptions(Options other)
        {
            InputFile = other.InputFile;
            OutputFile = other.OutputFile;
            CropLeft = other.CropLeft;
            CropRight = other.CropRight;
            CropTop = other.CropTop;
            CropBottom = other.CropBottom;
            Error = other.Error;
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

        void SetDefaultOptions()
        {
            string exeDir = GetExeDir();
            InputFile = Path.Combine(exeDir, "../../assets/vid/big_buck_bunny_trailer.vid.mp4");
            OutputFile = Path.Combine(exeDir, "../../output/video_crop/cropped.mp4");
            CropLeft = 60;
            CropRight = 60;
            CropTop = 0;
            CropBottom = 0;

            string outputDir = Path.Combine(exeDir, "../../output/video_crop");
            Directory.CreateDirectory(outputDir);
            OutputFile = Path.Combine(outputDir, "cropped.mp4");
        }

        void PrintDefaults()
        {
            Console.WriteLine("Using defaults:");
            Console.WriteLine(" --input " + InputFile);
            Console.WriteLine(" --output " + OutputFile);
            Console.WriteLine(" --crop-left " + CropLeft);
            Console.WriteLine(" --crop-right " + CropRight);
            Console.WriteLine(" --crop-top " + CropTop);
            Console.WriteLine(" --crop-bottom " + CropBottom);
            Console.WriteLine();
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

            Console.WriteLine($"Crop left: {CropLeft}");
            Console.WriteLine($"Crop right: {CropRight}");
            Console.WriteLine($"Crop top: {CropTop}");
            Console.WriteLine($"Crop bottom: {CropBottom}");

            return res;
        }

        static string GetExeDir()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetDirectoryName(path);
        }
    }
}
