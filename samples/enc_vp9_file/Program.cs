using System;
using PrimoSoftware.AVBlocks;

namespace CliSample
{
    class Program
    {
        static int Main(string[] args)
        {
            var opt = new Options();

            if (!opt.Prepare(args))
                return opt.Error ? (int)ExitCodes.OptionsError : (int)ExitCodes.Success;

            Library.Initialize();

            // Set license information. To run AVBlocks in demo mode, comment the next line out
            // Library.SetLicense("<license-string>");

            bool encodeResult = Encode(opt);

            Library.Shutdown();

            return encodeResult ? (int)ExitCodes.Success : (int)ExitCodes.EncodingError;
        }

        static bool Encode(Options opt)
        {
            DeleteFile(opt.OutputFile);

            // Create output directory if needed
            string outputDir = System.IO.Path.GetDirectoryName(opt.OutputFile);
            if (!string.IsNullOrEmpty(outputDir))
                System.IO.Directory.CreateDirectory(outputDir);

            MediaSocket inSocket = CreateInputSocket(opt);
            MediaSocket outSocket = CreateOutputSocket(opt);

            // create Transcoder
            using (Transcoder transcoder = new Transcoder())
            {
                transcoder.AllowDemoMode = true;
                transcoder.Inputs.Add(inSocket);
                transcoder.Outputs.Add(outSocket);

                bool res = transcoder.Open();
                PrintStatus("Transcoder open", transcoder.Error);
                if (!res)
                    return false;

                res = transcoder.Run();
                PrintStatus("Transcoder run", transcoder.Error);
                if (!res)
                    return false;

                transcoder.Close();
                PrintStatus("Transcoder close", transcoder.Error);
            }

            return true;
        }

        static void DeleteFile(string filename)
        {
            try
            {
                if (System.IO.File.Exists(filename))
                    System.IO.File.Delete(filename);
            }
            catch { }
        }

        static MediaSocket CreateInputSocket(Options opt)
        {
            MediaSocket socket = new MediaSocket();
            socket.StreamType = StreamType.UncompressedVideo;
            socket.File = opt.InputFile;

            MediaPin pin = new MediaPin();
            socket.Pins.Add(pin);
            VideoStreamInfo vsi = new VideoStreamInfo();
            pin.StreamInfo = vsi;

            vsi.StreamType = StreamType.UncompressedVideo;
            vsi.ScanType = ScanType.Progressive;

            vsi.FrameWidth = opt.Width;
            vsi.FrameHeight = opt.Height;
            vsi.ColorFormat = opt.Color.Id;
            vsi.FrameRate = opt.Fps;

            return socket;
        }

        static MediaSocket CreateOutputSocket(Options opt)
        {
            MediaSocket socket = new MediaSocket();
            socket.File = opt.OutputFile;
            socket.StreamType = StreamType.Ivf;

            MediaPin pin = new MediaPin();
            socket.Pins.Add(pin);
            VideoStreamInfo vsi = new VideoStreamInfo();
            pin.StreamInfo = vsi;

            vsi.StreamType = StreamType.Vp9;
            
            return socket;
        }

        static void PrintStatus(string action, ErrorInfo e)
        {
            if (action != null)
            {
                Console.Write("{0}: ", action);
            }

            if (ErrorFacility.Success == e.Facility)
            {
                Console.WriteLine("Success");
                return;
            }
            else
            {
                Console.WriteLine("{0}, facility:{1} code:{2} hint:{3}", e.Message ?? "", e.Facility, e.Code, e.Hint ?? "");
            }
        }

        enum ExitCodes : int
        {
            Success = 0,
            OptionsError = 1,
            EncodingError = 2,
        }
    }
}
