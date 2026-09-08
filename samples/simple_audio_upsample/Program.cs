using System;
using System.IO;
using PrimoSoftware.AVBlocks;

namespace SimpleAudioUpsample
{
    class Program
    {
        static int Main(string[] args)
        {
           // Hardcoded input and output file paths (no command line parsing)
            // Paths are relative to the working directory from which the executable is run
            string inputPath = "kahvi011_kennybeltrey-hydrate.mp3";
            string outputPath = "kahvi011_kennybeltrey-hydrate_48Khz.mp3";

            Library.Initialize();

            bool result = Upsample(inputPath, outputPath);

            Library.Shutdown();

            return result ? (int)ExitCodes.Success : (int)ExitCodes.EncodingError;
        }

        static bool Upsample(string inputPath, string outputPath)
        {
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Delete output file if it exists (transcoder will fail otherwise)
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            // Create MediaInfo to probe the input file
            using (var mediaInfo = new MediaInfo())
            {
                mediaInfo.Inputs[0].File = inputPath;

                if (!mediaInfo.Open())
                {
                    PrintError("Open MediaInfo", mediaInfo.Error);
                    return false;
                }

                // Create input socket from probed media info
                var inputSocket = MediaSocket.FromMediaInfo(mediaInfo);

                // Clone the input socket to start with identical output configuration
                var outputSocket = (MediaSocket)inputSocket.Clone();
                outputSocket.File = outputPath;

                // Get the output audio pin and set sample rate to 48 KHz
                if (outputSocket.Pins.Count > 0)
                {
                    var outAudioPin = outputSocket.Pins[0];
                    var outAudioStream = outAudioPin.StreamInfo as AudioStreamInfo;
                    if (outAudioStream != null)
                    {
                        outAudioStream.SampleRate = 48000;
                    }
                }

                // Create Transcoder and configure it
                using (var transcoder = new Transcoder())
                {
                    transcoder.AllowDemoMode = true;
                    transcoder.Inputs.Add(inputSocket);
                    transcoder.Outputs.Add(outputSocket);

                    if (!transcoder.Open())
                    {
                        PrintError("Transcoder open", transcoder.Error);
                        return false;
                    }

                    if (!transcoder.Run())
                    {
                        PrintError("Transcoder run", transcoder.Error);
                        transcoder.Close();
                        return false;
                    }

                    transcoder.Close();
                }
            }

            return true;
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
            EncodingError = 1,
        }
    }
}
