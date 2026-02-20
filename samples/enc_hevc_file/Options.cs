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
        // enc_hevc_file command line options
        [Option('i', "input", HelpText = "input YUV file")]
        public string InputFile { get; set; }

        [Option('o', "output", HelpText = "output HEVC / H.265 file")]
        public string OutputFile { get; set; }

        [Option('r', "rate", HelpText = "input frame rate")]
        public double Fps { get; set; }

        [Option('f', "frame", HelpText = "input frame size")]
        public string FrameSize { get; set; }

        [Option('c', "color", HelpText = "input color space. Use --colors to list all supported input color spaces.")]
        public string ColorFormat { get; set; }

        [Option("colors", HelpText = "list supported input color spaces")]
        public bool ListColors { get; set; }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public ColorDescriptor Color { get; private set; }

        public bool Error { get; private set; }

        void ResetOptions()
        {
            InputFile = null;
            OutputFile = null;
            Fps = 0.0;
            FrameSize = null;
            ColorFormat = null;
            ListColors = false;
            Error = false;
        }

        void SetOptions(Options other)
        {
            InputFile = other.InputFile;
            OutputFile = other.OutputFile;
            Fps = other.Fps;
            FrameSize = other.FrameSize;
            ColorFormat = other.ColorFormat;
            ListColors = other.ListColors;
            Error = other.Error;
        }        

        void SetDefaultOptions()
        {
            string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            InputFile = Path.Combine(exeDir, "../../assets/vid/foreman_qcif.yuv");
            OutputFile = Path.Combine(exeDir, "../../output/enc_hevc_file/foreman_qcif.h265");

            Fps = 30.0;

            Width = 176; Height = 144;
            FrameSize = string.Format("{0}x{1}", Width, Height);

            Color = GetColorById(PrimoSoftware.AVBlocks.ColorFormat.YUV420);

            Console.WriteLine("Using default options: ");
            Console.Write("enc_hevc_file --input " + InputFile);
            Console.Write(" --output " + OutputFile);
            Console.Write(" --rate " + Fps);
            Console.Write(" --frame " + FrameSize);
            Console.Write(" --color " + Color.Name);
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
            if (OutputFile == null)
            {
                Console.WriteLine("[not set]");
                res = false;
            }
            else
            {
                Console.WriteLine(OutputFile);
            }

            Console.Write("Input frame size: ");
            if (!ParseFrameSize())
            {
                Console.WriteLine("[not set / incorrect]");
                res = false;
            }
            else
            {
                Console.WriteLine(FrameSize);
            }

            Console.Write("Input color format: ");
            if (GetColorByName(ColorFormat) == null)
            {
                Console.WriteLine("[not set / incorrect]");
                res = false;
            }
            else
            {
                Color = GetColorByName(ColorFormat);
                Console.WriteLine(ColorFormat);
            }

            Console.Write("Output frame rate: ");
            if (Fps == 0.0)
            {
                Console.WriteLine("[not set]");
                res = false;
            }
            else
            {
                Console.WriteLine(Fps);
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

            if(ListColors)
            {
                PrintColors();
                return false;
            }

            if (!Validate())
            {
                var helpText = HelpText.AutoBuild(parserResult, h => h, e => e);
                Console.WriteLine($"Usage:\n{helpText}");

                Error = true;
                return false;
            }            
            
            return true;
        }

        bool ParseFrameSize()
        {
            if (string.IsNullOrEmpty(FrameSize))
                return false;

            Width = 0;
            Height = 0;

            var parts = FrameSize.Split('x');
            if (parts.Length != 2)
                return false;

            try
            {
                Width = Convert.ToInt32(parts[0]);
                Height = Convert.ToInt32(parts[1]);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        static ColorDescriptor[] Colors = {
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.YV12,	    "yv12",	    "Planar Y, V, U (4:2:0) (note V,U order!)" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.NV12,	    "nv12",	    "Planar Y, merged U->V (4:2:0)" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.YUY2,	    "yuy2",	    "Composite Y->U->Y->V (4:2:2)" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.UYVY,	    "uyvy",	    "Composite U->Y->V->Y (4:2:2)" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.YUV411,	"yuv411",	"Planar Y, U, V (4:1:1)" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.YUV420,	"yuv420",	"Planar Y, U, V (4:2:0)" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.YUV422,	"yuv422",	"Planar Y, U, V (4:2:2)" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.YUV444,	"yuv444",	"Planar Y, U, V (4:4:4)" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.Y411,	    "y411",	    "Composite Y, U, V (4:1:1)" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.Y41P,	    "y41p",	    "Composite Y, U, V (4:1:1)" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.BGR32,	    "bgr32",	"Composite B->G->R" ),
            new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.BGRA32,	"bgra32",	"Composite B->G->R->A" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.BGR24,	    "bgr24",	"Composite B->G->R" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.BGR565,	"bgr565",	"Composite B->G->R, 5 bit per B & R, 6 bit per G" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.BGR555,	"bgr555",	"Composite B->G->R->A, 5 bit per component, 1 bit per A" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.BGR444,	"bgr444",	"Composite B->G->R->A, 4 bit per component" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.GRAY,	    "gray",	    "Luminance component only" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.YUV420A,	"yuv420a",	"Planar Y, U, V, Alpha (4:2:0)" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.YUV422A,	"yuv422a",	"Planar Y, U, V, Alpha (4:2:2)" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.YUV444A,	"yuv444a",	"Planar Y, U, V, Alpha (4:4:4)" ),
	        new ColorDescriptor( PrimoSoftware.AVBlocks.ColorFormat.YVU9,	    "yvu9",	    "Planar Y, V, U, 9 bits per sample" ) 
        };

        static void PrintColors()
        {
            Console.WriteLine("\nCOLORS");
            Console.WriteLine("---------");
            foreach (var color in Colors)
            {
                Console.WriteLine("{0,-20} {1}", color.Name, color.Description);
            }
            Console.WriteLine();
        }

        static ColorDescriptor GetColorByName(string colorName)
        {
            foreach (var color in Colors)
            {
                if (color.Name.Equals(colorName, StringComparison.InvariantCultureIgnoreCase))
                    return color;
            }
            return null;
        }

        static ColorDescriptor GetColorById(ColorFormat colorId)
        {
            foreach (var color in Colors)
            {
                if (color.Id == colorId)
                    return color;
            }
            return null;
        }

    }

    class ColorDescriptor
    {
        public ColorDescriptor(ColorFormat id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public ColorFormat Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
