using System;
using System.IO;
using CommandLine;
using CommandLine.Text;
using PrimoSoftware.AVBlocks;

namespace CliSample
{
    class Options
    {
        [Option('i', "input", HelpText = "input directory containing H.265 Access Unit files")]
        public string InputDir { get; set; }

        [Option('o', "output", HelpText = "output YUV file")]
        public string OutputFile { get; set; }

        [Option('c', "color", HelpText = "output color format (e.g. yuv420, yv12, nv12)")]
        public string ColorName { get; set; }

        [Option("colors", HelpText = "list supported color formats")]
        public bool ListColors { get; set; }

        public ColorFormat ColorId { get; private set; }
        public bool Error { get; private set; }

        static readonly ColorDescriptor[] Colors = {
            new ColorDescriptor(ColorFormat.YV12,    "yv12",    "Planar Y, V, U (4:2:0) (note V,U order!)"),
            new ColorDescriptor(ColorFormat.NV12,    "nv12",    "Planar Y, merged U->V (4:2:0)"),
            new ColorDescriptor(ColorFormat.YUY2,    "yuy2",    "Composite Y->U->Y->V (4:2:2)"),
            new ColorDescriptor(ColorFormat.UYVY,    "uyvy",    "Composite U->Y->V->Y (4:2:2)"),
            new ColorDescriptor(ColorFormat.YUV411,  "yuv411",  "Planar Y, U, V (4:1:1)"),
            new ColorDescriptor(ColorFormat.YUV420,  "yuv420",  "Planar Y, U, V (4:2:0)"),
            new ColorDescriptor(ColorFormat.YUV422,  "yuv422",  "Planar Y, U, V (4:2:2)"),
            new ColorDescriptor(ColorFormat.YUV444,  "yuv444",  "Planar Y, U, V (4:4:4)"),
            new ColorDescriptor(ColorFormat.Y411,    "y411",    "Composite Y, U, V (4:1:1)"),
            new ColorDescriptor(ColorFormat.Y41P,    "y41p",    "Composite Y, U, V (4:1:1)"),
            new ColorDescriptor(ColorFormat.BGR32,   "bgr32",   "Composite B->G->R"),
            new ColorDescriptor(ColorFormat.BGRA32,  "bgra32",  "Composite B->G->R->A"),
            new ColorDescriptor(ColorFormat.BGR24,   "bgr24",   "Composite B->G->R"),
            new ColorDescriptor(ColorFormat.BGR565,  "bgr565",  "Composite B->G->R, 5 bit per B & R, 6 bit per G"),
            new ColorDescriptor(ColorFormat.BGR555,  "bgr555",  "Composite B->G->R->A, 5 bit per component, 1 bit per A"),
            new ColorDescriptor(ColorFormat.BGR444,  "bgr444",  "Composite B->G->R->A, 4 bit per component"),
            new ColorDescriptor(ColorFormat.GRAY,    "gray",    "Luminance component only"),
            new ColorDescriptor(ColorFormat.YUV420A, "yuv420a", "Planar Y, U, V, Alpha (4:2:0)"),
            new ColorDescriptor(ColorFormat.YUV422A, "yuv422a", "Planar Y, U, V, Alpha (4:2:2)"),
            new ColorDescriptor(ColorFormat.YUV444A, "yuv444a", "Planar Y, U, V, Alpha (4:4:4)"),
            new ColorDescriptor(ColorFormat.YVU9,    "yvu9",    "Planar Y, V, U, 9 bits per sample")
        };

        void ResetOptions()
        {
            InputDir = null;
            OutputFile = null;
            ColorName = null;
            ColorId = ColorFormat.YUV420;
            ListColors = false;
            Error = false;
        }

        void SetOptions(Options other)
        {
            InputDir = other.InputDir;
            OutputFile = other.OutputFile;
            ColorName = other.ColorName;
            ListColors = other.ListColors;
            Error = other.Error;
        }

        void SetDefaultOptions()
        {
            string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            InputDir = Path.Combine(exeDir, "../../assets/vid/foreman_qcif.h265.au");
            OutputFile = Path.Combine(exeDir, "../../output/dec_hevc_au/foreman_qcif.yuv");
            ColorId = ColorFormat.YUV420;

            Console.WriteLine("Using default options: ");
            Console.WriteLine(" --input " + InputDir);
            Console.WriteLine(" --output " + OutputFile);
            Console.WriteLine();
        }

        bool ParseColorFormat()
        {
            if (string.IsNullOrEmpty(ColorName))
            {
                ColorId = ColorFormat.YUV420;
                return true;
            }

            foreach (var color in Colors)
            {
                if (color.Name.Equals(ColorName, StringComparison.OrdinalIgnoreCase))
                {
                    ColorId = color.Id;
                    return true;
                }
            }
            return false;
        }

        public static void PrintColors()
        {
            Console.WriteLine("\nCOLORS");
            Console.WriteLine("---------");
            foreach (var color in Colors)
            {
                Console.WriteLine("{0,-20} {1}", color.Name, color.Description);
            }
            Console.WriteLine();
        }

        bool Validate()
        {
            bool res = true;

            Console.Write("--input: ");
            if (string.IsNullOrEmpty(InputDir))
            {
                Console.WriteLine("[not set]");
                res = false;
            }
            else
            {
                Console.WriteLine(InputDir);
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

            if (!ParseColorFormat())
            {
                Console.WriteLine("Invalid color format: " + ColorName);
                res = false;
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

            if (ListColors)
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

            Error = false;
            return true;
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
