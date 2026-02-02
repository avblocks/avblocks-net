using System;
using System.IO;
using System.Linq;
using System.Reflection;

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

            bool encodeResult = WebMMux(opt);

            Library.Shutdown();

            return encodeResult ? (int)ExitCodes.Success : (int)ExitCodes.EncodeError;
        }

        enum ExitCodes : int
        {
            Success = 0,
            OptionsError = 1,
            EncodeError = 2,
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

        static bool WebMMux(Options opt)
        {
            try { File.Delete(opt.OutputFile); }
            catch (Exception) { }

            // Create output directory if needed
            string outputDir = Path.GetDirectoryName(opt.OutputFile);
            if (!string.IsNullOrEmpty(outputDir))
                Directory.CreateDirectory(outputDir);

            using (var transcoder = new Transcoder())
            {

                // Transcoder demo mode must be enabled, 
                // in order to use the production release for testing (without a valid license)
                transcoder.AllowDemoMode = true;

                MediaSocket outputSocket = new MediaSocket();
                outputSocket.File = opt.OutputFile;
                outputSocket.StreamType = StreamType.WebM;

                // audio
                for (int i = 0; i < (int)opt.AudioFiles.Count(); i++)
                {
                    MediaPin outputPin = new MediaPin();
                    AudioStreamInfo asi = new AudioStreamInfo();
                    asi.StreamType = StreamType.Vorbis;
                    outputPin.StreamInfo = asi;

                    outputSocket.Pins.Add(outputPin);

                    MediaSocket inputSocket = new MediaSocket();
                    inputSocket.File = opt.AudioFiles.ElementAt(i);
                    inputSocket.StreamType = StreamType.WebM;
                    transcoder.Inputs.Add(inputSocket);

                    Console.WriteLine("Muxing audio input: {0}", opt.AudioFiles.ElementAt(i));
                }

                // video
                for (int i = 0; i < (int)opt.VideoFiles.Count(); i++)
                {
                    MediaPin outputPin = new MediaPin();
                    VideoStreamInfo vsi = new VideoStreamInfo();
                    vsi.StreamType = StreamType.Vp8;
                    outputPin.StreamInfo = vsi;

                    outputSocket.Pins.Add(outputPin);

                    MediaSocket inputSocket = new MediaSocket();
                    inputSocket.File = opt.VideoFiles.ElementAt(i);
                    inputSocket.StreamType = StreamType.WebM;
                    transcoder.Inputs.Add(inputSocket);

                    Console.WriteLine("Muxing video input: {0}", opt.VideoFiles.ElementAt(i));
                }

                transcoder.Outputs.Add(outputSocket);

                if (!transcoder.Open())
                {
                    PrintError("Open Transcoder", transcoder.Error);
                    return false;
                }

                if (!transcoder.Run())
                {
                    PrintError("Run Transcoder", transcoder.Error);
                    return false;
                }
                 
                transcoder.Close();

                Console.WriteLine("Output file: {0}", opt.OutputFile);

                return true;
            }
        }
    }
}
