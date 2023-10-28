using System.ComponentModel;
using System.Xml.Serialization;
using System.Net;
using System;
using System.IO;
using CommandLine;
using CommandLine.Text;
using PrimoSoftware.AVBlocks;

namespace CliSample
{
    class Options
    {
        // re-encode command line options
        [Option('i', "input", HelpText = "input file")]
        public string InputFile { get; set; }

        [Option('o', "output", HelpText = "output file")]
        public string OutputFile { get; set; }

        [Option('a', "audio", Required = false, HelpText = "Force audio re-encoding.")]
        public bool ForceAudio { get; set; }

        [Option('v', "video", Required = false, HelpText = "Force video re-encoding.")]
        public bool ForceVideo { get; set; }

        // The program parses the command line options and sets these properties
        public bool Error { get; private set; }

        void ResetOptions()
        {
            Error = false;
            InputFile = null;
            OutputFile = null;
            ForceAudio = false;
            ForceVideo = false;
        }

        void SetDefaultOptions()
        {
            string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            InputFile = Path.Combine(exeDir, "../../assets/mov/big_buck_bunny_trailer.mp4");
            OutputFile = Path.Combine(exeDir, "../../output/reencode/big_buck_bunny_trailer.mp4");
            ForceAudio = false;            
            ForceVideo = false;            

            Console.WriteLine("Using default options: ");
            Console.Write("reencode");
            Console.Write(" --input " + InputFile);
            Console.Write(" --output " + OutputFile);
            if (ForceAudio) Console.Write(" --audio");
            if (ForceVideo) Console.Write(" --video");
            Console.WriteLine();
        }

        void SetOptions(Options other)
        {
            InputFile = other.InputFile;
            OutputFile = other.OutputFile;
            ForceAudio = other.ForceAudio;
            ForceVideo = other.ForceVideo;
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

            Console.WriteLine("Re-encode audio forced: " + (ForceAudio ? "yes" : "no"));
            Console.WriteLine("Re-encode video forced: " + (ForceVideo ? "yes" : "no"));

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
