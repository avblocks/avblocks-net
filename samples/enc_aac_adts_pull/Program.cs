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

            // Set license information. Without this AVBlocks runs in Demo mode.
            // Library.SetLicense("<license-string>");

            bool encodeResult = Encode(opt);

            Library.Shutdown();

            return encodeResult ? (int)ExitCodes.Success : (int)ExitCodes.EncodingError;
        }

        static MediaSocket CreateOutputSocket()
        {
            MediaSocket socket = new MediaSocket();
            socket.StreamType = StreamType.Aac;
            socket.StreamSubType = StreamSubType.AacAdts;

            MediaPin pin = new MediaPin();
            socket.Pins.Add(pin);

            AudioStreamInfo asi = new AudioStreamInfo();
            pin.StreamInfo = asi;

            asi.StreamType = StreamType.Aac;
            asi.StreamSubType = StreamSubType.AacAdts;

            // You can change the sampling rate and the number of the channels
            // asi.Channels = 1;
            // asi.SampleRate = 44100;

            return socket;
        }

        static bool Encode(Options opt)
        {
            // transcoder will fail if output exists (by design)
            DeleteFile(opt.OutputFile);

            // Create output directory if needed
            string outputDir = Path.GetDirectoryName(opt.OutputFile);
            if (!string.IsNullOrEmpty(outputDir))
                Directory.CreateDirectory(outputDir);

            using (FileStream outfile = File.Create(opt.OutputFile))
            {
                // create input socket
                MediaSocket inSocket = new MediaSocket();
                inSocket.File = opt.InputFile;

                // create output socket
                MediaSocket outSocket = CreateOutputSocket();

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
