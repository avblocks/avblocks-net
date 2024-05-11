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

            bool result = Encode(opt);

            Library.Shutdown();

            return result ? (int)ExitCodes.Success : (int)ExitCodes.EncodingError;
        }

        static bool Encode(Options opt)
        {
            DeleteFile(opt.OutputFile);

            MediaSocket inSocket = new MediaSocket();
            inSocket.File = opt.InputFile;

            MediaSocket outSocket = CreateOutputSocket(opt);

            // create Transcoder
            using (Transcoder transcoder = new Transcoder())
            {
                transcoder.AllowDemoMode = true;
                transcoder.Inputs.Add(inSocket);
                transcoder.Outputs.Add(outSocket);

                if(!transcoder.Open())
                {
                    PrintError("Transcoder open", transcoder.Error);
                    return false;

                }

                if(!transcoder.Run())
                {
                    PrintError("Transcoder run", transcoder.Error);
                    return false;
                }

                transcoder.Close();
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

        static MediaSocket CreateOutputSocket(Options opt)
        {
            AudioStreamInfo asi = new AudioStreamInfo();

            asi.StreamType = StreamType.MpegAudio;
            asi.StreamSubType = StreamSubType.MpegAudioLayer3;

            // The default bitrate is 128000. You can set it to 192000, 256000, etc.
            // asi.Bitrate = 192000;

            // You can change the sampling rate and the number of the channels
            // asi.SampleRate = 44100;
            // asi.Channels = 1;

            MediaPin pin = new MediaPin();
            pin.StreamInfo = asi;

            MediaSocket socket = new MediaSocket();
            socket.StreamType = StreamType.MpegAudio;
            socket.StreamSubType = StreamSubType.MpegAudioLayer3;

            socket.Pins.Add(pin);

            // output to a file
            socket.File = opt.OutputFile;

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
