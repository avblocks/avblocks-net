using System;
using System.IO;
using CliSample;
using PrimoSoftware.AVBlocks;

namespace AudioUpsample
{
    class Program
    {
        static int Main(string[] args)
        {
            Options opt = new Options();
            
            if (!opt.Prepare(args))
                return opt.Error ? (int)ExitCodes.OptionsError : (int)ExitCodes.Success;

            Library.Initialize();

            bool result = upsample(opt);

            Library.Shutdown();

            return result ? (int)ExitCodes.Success : (int)ExitCodes.EncodingError;
        }

        static bool upsample(Options opt)
        {
            // create input socket
            MediaSocket inSocket = new MediaSocket();
            inSocket.File = opt.InputFile;

            // create output socket with 48 KHz sample rate
            MediaSocket outSocket = CreateOutputSocket(opt);

            // create Transcoder
            using (Transcoder transcoder = new Transcoder())
            {
                transcoder.AllowDemoMode = true;
                transcoder.Inputs.Add(inSocket);
                transcoder.Outputs.Add(outSocket);

                // transcoder will fail if output exists (by design)
                string outputFile = opt.OutputFile;
                if (File.Exists(outputFile))
                {
                    File.Delete(outputFile);
                }

                if (!transcoder.Open())
                {
                    PrintError("Transcoder open", transcoder.Error);
                    return false;
                }

                if (!transcoder.Run())
                {
                    PrintError("Transcoder run", transcoder.Error);
                    return false;
                }

                transcoder.Close();
            }

            return true;
        }

        static MediaSocket CreateOutputSocket(Options opt)
        {
            // create stream info to describe the output audio stream
            AudioStreamInfo asi = new AudioStreamInfo();

            asi.StreamType = StreamType.MpegAudio;
            asi.StreamSubType = StreamSubType.MpegAudioLayer3;

            // Set the output sampling rate to 48 KHz (48000 Hz)
            // This will trigger resampling from the input sample rate (e.g., 44.1 KHz)
            asi.SampleRate = 48000;

            // create a pin using the stream info 
            MediaPin pin = new MediaPin();
            pin.StreamInfo = asi;

            // finally create a socket for the output container format which is MP3 in this case
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
