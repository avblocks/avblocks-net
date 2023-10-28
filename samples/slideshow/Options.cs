using System.Net;
using System;

using System.Collections;
using System.Collections.Generic;

using System.IO;

using CommandLine;
using CommandLine.Text;

using PrimoSoftware.AVBlocks;

namespace CliSample
{
    class Options
    {
        // slideshow command line options
        [Option('i', "input", HelpText = "Input directory containing images for the slideshow.")]
        public string InputDir { get; set; }

        [Option('o', "output", HelpText = "Output filename (without extension). The extension is added based on the preset.")]
        public string OutputFile { get; set; }

        [Option('p', "preset", HelpText = "output preset id.")]
        public string PresetID { get; set; }

        [Option("presets", HelpText = "list supported input presets")]
        public bool ListPresets { get; set; }

        public string FileExtension { get; private set; }

        // The program parses the command line options and sets these properties
        public bool Error { get; private set; }

        void ResetOptions()
        {
            InputDir = null;
            OutputFile = null;
            PresetID = null;
            FileExtension = null;            
            Error = false;
        }

        void SetDefaultOptions()
        {
            string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            InputDir = Path.Combine(exeDir, "../../assets/img");
            OutputFile = Path.Combine(exeDir, "../../output/slideshow/cube");

            PresetID = Preset.Video.Generic.MP4.Base_H264_AAC; // "mp4.h264.aac"
            
            Console.WriteLine("Using default options: ");
            Console.Write(" --input " + InputDir);
            Console.Write(" --output " + OutputFile);
            Console.Write(" --preset " + PresetID);
            Console.WriteLine();
        }

        void SetOptions(Options other)
        {
            InputDir = other.InputDir;
            OutputFile = other.OutputFile;
            PresetID = other.PresetID;
            FileExtension = other.FileExtension;
            ListPresets = other.ListPresets;

        }

        bool Validate()
        {
            bool res = true;

            Console.Write("Input dir: ");
            if (InputDir == null)
            {
                Console.WriteLine("[not set]");
                res = false;
            }
            else
            {
                Console.WriteLine(InputDir);
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

            if(ListPresets)
            {
                PrintPresets();
                return false;
            }

            if (!Validate())
            {
                var helpText = HelpText.AutoBuild(parserResult, h => h, e => e);
                Console.WriteLine($"Usage:\n{helpText}");

                Error = true;
                return false;
            }

            // get filename extension from preset descriptor
            PresetDescriptor preset = GetPresetByName(PresetID);
            if (preset == null)
            {
                Console.WriteLine("\nPreset not found!");
                Error = true;
                return false;
            }                        

            FileExtension = preset.FileExtension;
            
            return true;
        }

        static PresetDescriptor GetPresetByName(string presetName)
        {
            foreach (var preset in AvbPresets)
            {
                if (preset.Name.Equals(presetName, StringComparison.InvariantCultureIgnoreCase))
                    return preset;
            }
            return null;
        }

        private static PresetDescriptor[] AvbPresets = new PresetDescriptor[]
        {
            // video presets
             new PresetDescriptor(Preset.Video.DVD.NTSC_16x9_MP2,       "mpg"),
             new PresetDescriptor(Preset.Video.DVD.NTSC_16x9_PCM,       "mpg"),
             new PresetDescriptor(Preset.Video.DVD.NTSC_4x3_MP2,        "mpg"),
	         new PresetDescriptor(Preset.Video.DVD.NTSC_4x3_PCM,  	    "mpg"),
	         new PresetDescriptor(Preset.Video.DVD.PAL_16x9_MP2,      	"mpg"),
	         new PresetDescriptor(Preset.Video.DVD.PAL_4x3_MP2,       	"mpg"),
	         new PresetDescriptor(Preset.Video.iPad.H264_576p,			"mp4"),
	         new PresetDescriptor(Preset.Video.iPad.H264_720p,			"mp4"),
	         new PresetDescriptor(Preset.Video.iPad.MPEG4_480p,       	"mp4"),
	         new PresetDescriptor(Preset.Video.iPhone.H264_480p,		"mp4"),
	         new PresetDescriptor(Preset.Video.iPhone.MPEG4_480p,		"mp4"),
	         new PresetDescriptor(Preset.Video.iPod.H264_240p,			"mp4"),
	         new PresetDescriptor(Preset.Video.iPod.MPEG4_240p,			"mp4"),
             new PresetDescriptor(Preset.Video.Generic.MP4.Base_H264_AAC,"mp4"),
	         new PresetDescriptor(Preset.Video.AppleLiveStreaming.H264_480p,  "ts"),
	         new PresetDescriptor(Preset.Video.AppleLiveStreaming.H264_720p,  "ts"),
	         new PresetDescriptor(Preset.Video.AndroidPhone.H264_360p,	"mp4"),
	         new PresetDescriptor(Preset.Video.AndroidPhone.H264_720p,  "mp4"),
	         new PresetDescriptor(Preset.Video.AndroidTablet.H264_720p,	"mpg"),
	         new PresetDescriptor(Preset.Video.AndroidTablet.WebM_VP8_720p, "webm"),
	         new PresetDescriptor(Preset.Video.VCD.NTSC,  			        "mpg"),
	         new PresetDescriptor(Preset.Video.VCD.PAL,  			        "mpg"),
	         new PresetDescriptor(Preset.Video.Generic.WebM.Base_VP8_Vorbis,  	"webm"),
        };

        static void PrintPresets()
        {
            Console.WriteLine();
            Console.WriteLine("PRESETS");
            Console.WriteLine("-----------");
            
            foreach (var preset in AvbPresets)
                Console.WriteLine("{0,-30} .{1}", preset.Name, preset.FileExtension);

            Console.WriteLine();
        }
    }

    class PresetDescriptor
    {
        public PresetDescriptor(string name, string fileExtension)
        {
            Name = name;
            FileExtension = fileExtension;
        }

        public string Name;
        public string FileExtension;
    }
}
