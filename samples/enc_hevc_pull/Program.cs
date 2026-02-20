using System;
using System.IO;
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
            string outputDir = Path.GetDirectoryName(opt.OutputFile);
            if (!string.IsNullOrEmpty(outputDir))
                Directory.CreateDirectory(outputDir);

            using (FileStream outfile = File.Create(opt.OutputFile))
            {
                MediaSocket inSocket = CreateInputSocket(opt);
                MediaSocket outSocket = CreateOutputSocket(opt);

                // create Transcoder
                using (Transcoder transcoder = new Transcoder())
                {
                    transcoder.AllowDemoMode = true;
                    transcoder.Inputs.Add(inSocket);
                    transcoder.Outputs.Add(outSocket);

                    if (!transcoder.Open())
                    {
                        PrintError("Transcoder open", transcoder.Error);
                        return false;
                    }

                    // encode by pulling encoded samples
                    int outputIndex = 0;
                    MediaSample sample = new MediaSample();

                    while (transcoder.Pull(out outputIndex, sample))
                    {
                        outfile.Write(sample.Buffer.Start, sample.Buffer.DataOffset, sample.Buffer.DataSize);
                    }

                    ErrorInfo error = transcoder.Error;
                    PrintError("Transcoder pull", error);

                    bool success = false;
                    if (error.Facility == ErrorFacility.Codec &&
                        error.Code == (int)CodecError.EOS)
                    {
                        // ok
                        success = true;
                    }

                    transcoder.Close();

                    return success;
                }
            }
        }

        static void DeleteFile(string filename)
        {
            try
            {
                if (File.Exists(filename))
                    File.Delete(filename);
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
            socket.StreamType = StreamType.H265;
            socket.StreamSubType = StreamSubType.HevcAnnexB;

            MediaPin pin = new MediaPin();
            socket.Pins.Add(pin);
            VideoStreamInfo vsi = new VideoStreamInfo();
            pin.StreamInfo = vsi;

            vsi.StreamType = StreamType.H265;
            vsi.StreamSubType = StreamSubType.HevcAnnexB;
            
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
            EncodingError = 2,
        }
    }
}
