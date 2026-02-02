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

            // Set license information. Without this AVBlocks runs in Demo mode.
            // Library.SetLicense("<license-string>");

            bool decodeResult = DecodeVp9File(opt);

            Library.Shutdown();

            return decodeResult ? (int)ExitCodes.Success : (int)ExitCodes.DecodingError;
        }

        static bool DecodeVp9File(Options opt)
        {
            // transcoder will fail if output exists (by design)
            DeleteFile(opt.OutputFile);

            // Create output directory if needed
            string outputDir = System.IO.Path.GetDirectoryName(opt.OutputFile);
            if (!string.IsNullOrEmpty(outputDir))
                System.IO.Directory.CreateDirectory(outputDir);

            using (var mediaInfo = new MediaInfo())
            {
                mediaInfo.Inputs[0].File = opt.InputFile;

                if (!mediaInfo.Open())
                {
                    PrintError("MediaInfo open", mediaInfo.Error);
                    return false;
                }

                // create input socket
                MediaSocket inputSocket = MediaSocket.FromMediaInfo(mediaInfo);
                mediaInfo.Close();

                // create output socket
                MediaSocket outputSocket = CreateOutputSocket(opt);

                // create Transcoder
                using (Transcoder transcoder = new Transcoder())
                {
                    transcoder.AllowDemoMode = true;
                    transcoder.Inputs.Add(inputSocket);
                    transcoder.Outputs.Add(outputSocket);

                    bool res = transcoder.Open();
                    PrintError("Transcoder open", transcoder.Error);
                    if (!res)
                        return false;

                    res = transcoder.Run();
                    PrintError("Transcoder run", transcoder.Error);
                    if (!res)
                        return false;

                    transcoder.Close();
                }
            }

            Console.WriteLine("Output: " + opt.OutputFile);
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

        static MediaSocket CreateOutputSocket(Options opt)
        {
            VideoStreamInfo vsi = new VideoStreamInfo();
            vsi.StreamType = StreamType.UncompressedVideo;
            vsi.ColorFormat = ColorFormat.YUV420;

            MediaPin pin = new MediaPin();
            pin.StreamInfo = vsi;

            MediaSocket socket = new MediaSocket();
            socket.File = opt.OutputFile;
            socket.StreamType = StreamType.UncompressedVideo;
            socket.Pins.Add(pin);

            return socket;
        }

        static void PrintError(string action, ErrorInfo e)
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
            DecodingError = 2,
        }
    }
}
