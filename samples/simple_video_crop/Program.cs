using System;
using System.IO;
using PrimoSoftware.AVBlocks;

namespace SimpleVideoCrop
{
    class Program
    {
        static int Main(string[] args)
        {
           // Hardcoded input and output file paths (no command line parsing)
            // Paths are relative to the working directory from which the executable is run
            string inputPath = "AndrewConnolly_2014.mp4";
            string outputPath = "AndrewConnolly_2014_cropped.mp4";

            Library.Initialize();

            bool result = CropVideo(inputPath, outputPath);

            Library.Shutdown();

            return result ? (int)ExitCodes.Success : (int)ExitCodes.EncodeError;
        }

        static bool CropVideo(string inputPath, string outputPath)
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

                // Get the output video pin
                if (outputSocket.Pins.Count < 1)
                {
                    Console.WriteLine("No video pin found in input");
                    return false;
                }

                var outVideoPin = outputSocket.Pins[0];
                var outVideoStream = outVideoPin.StreamInfo as VideoStreamInfo;
                if (outVideoStream == null)
                {
                    Console.WriteLine("Output pin is not a video stream");
                    return false;
                }

                // Set the new frame width and height to 640 x 480
                outVideoStream.FrameWidth = 640;
                outVideoStream.FrameHeight = 480;

                // Set the display ratio to 4:3
                outVideoStream.DisplayRatioWidth = 4;
                outVideoStream.DisplayRatioHeight = 3;

                // Set Crop.Left and Crop.Right
                // The input video is 854x480.
                // To make it 640x480 we have to cut (854 - 640) / 2 pixels from each side.
                int cropAmount = (854 - 640) / 2; // 107
                outVideoPin.Params.Add(Param.Video.Crop.Left, cropAmount);
                outVideoPin.Params.Add(Param.Video.Crop.Right, cropAmount);

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

        static void PrintError(string action, ErrorInfo error)
        {
            if (action != null)
            {
                Console.Write("{0}: ", action);
            }

            if (ErrorFacility.Success == error.Facility)
            {
                Console.WriteLine("Success");
                return;
            }
            else
            {
                Console.WriteLine("{0}, facility:{1} code:{2} hint:{3}", error.Message ?? "", error.Facility, error.Code, error.Hint ?? "");
            }
        }

        enum ExitCodes : int
        {
            Success = 0,
            EncodeError = 2,
        }
    }
}
