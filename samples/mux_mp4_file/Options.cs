using System;
using System.IO;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using CommandLine;
using CommandLine.Text;

using PrimoSoftware.AVBlocks;

namespace CliSample
{
    class Options
    {
        // mux_mp4_file options
        [Option('a', "audio", HelpText = "input AAC files. Could be a list of files.")]
        public IEnumerable<string> AudioFiles { get; set; }

        [Option('v', "video", HelpText = "input H264 files. Could be a list of files.")]
        public IEnumerable<string> VideoFiles { get; set; }

        [Option('o', "output", HelpText = "output file")]
        public string OutputFile { get; set; }
        
        // The program parses the command line options and sets these properties
        public bool Error { get; private set; }

        void ResetOptions()
        {
            AudioFiles = null;
            VideoFiles = null;
            OutputFile = null;
            Error = false;
        }

        void SetDefaultOptions()
        {
            string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string videoFile = Path.Combine(exeDir, "../../assets/vid/big_buck_bunny_trailer.vid.mp4");
            VideoFiles = new List<string> { videoFile };

            string audioFile = Path.Combine(exeDir, "../../assets/aud/big_buck_bunny_trailer.aud.mp4");
            AudioFiles = new List<string> { audioFile };

            OutputFile = Path.Combine(exeDir, "../../output/mux_mp4_file/big_buck_bunny_trailer.mp4");

            Console.WriteLine("Using default options: ");
            Console.Write("mux_mp4_file");
            Console.Write(" --audio " + AudioFiles.First());
            Console.Write(" --video " + VideoFiles.First());
            Console.Write(" --output " + OutputFile);
            Console.WriteLine();
        }


        void SetOptions(Options other)
        {
            AudioFiles = other.AudioFiles;
            VideoFiles = other.VideoFiles;
            OutputFile = other.OutputFile;
        }

        bool Validate()
        {
            bool res = true;

            Console.WriteLine("Audio files: ");
            if ((AudioFiles != null) && (AudioFiles.Count() > 0))
            {
                foreach (string s in AudioFiles)
                {
                    Console.WriteLine("   " + s);
                }
            }
            else
            {
                Console.WriteLine("[not set]");
                res = false;
            }

            Console.WriteLine("Video files: ");
            if ((VideoFiles != null) && (VideoFiles.Count() > 0))
            {
                foreach (string s in VideoFiles)
                {
                    Console.WriteLine("   " + s);
                }
            }
            else
            {
                Console.WriteLine("[not set]");
                res = false;
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
