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

            bool upscaleResult = UpscaleVideo(opt);

            Library.Shutdown();

            return upscaleResult ? (int)ExitCodes.Success : (int)ExitCodes.EncodeError;
        }

        enum ExitCodes : int
        {
            Success = 0,
            OptionsError = 1,
            EncodeError = 2,
        }

        static bool UpscaleVideo(Options opt)
        {
            if (File.Exists(opt.OutputFile))
                File.Delete(opt.OutputFile);

            using (var transcoder = new Transcoder())
            {
                transcoder.AllowDemoMode = true;

                using (var mediaInfo = new MediaInfo())
                {
                    mediaInfo.Inputs[0].File = opt.InputFile;

                    if (!mediaInfo.Open())
                    {
                        PrintError("Open MediaInfo", mediaInfo.Error);
                        return false;
                    }

                    // Add Inputs
                    {
                        var inputSocket = MediaSocket.FromMediaInfo(mediaInfo);
                        transcoder.Inputs.Add(inputSocket);
                    }
                }

                // Add Outputs
                {
                    var outputSocket = new MediaSocket();
                    outputSocket.File = opt.OutputFile;

                    // Get input socket to determine stream types
                    var inputSocket = transcoder.Inputs[0];

                    // Set output stream type to match input
                    outputSocket.StreamType = inputSocket.StreamType;

                    // Add video pin with resize parameters
                    var inVideoPin = inputSocket.Pins[0];
                    var videoStreamInfo = inVideoPin.StreamInfo as VideoStreamInfo;

                    if (videoStreamInfo != null)
                    {
                        var outVideoPin = new MediaPin();
                        outVideoPin.StreamInfo = (StreamInfo)videoStreamInfo.Clone();

                        // Set output frame size
                        var outVideoStreamInfo = outVideoPin.StreamInfo as VideoStreamInfo;
                        if (outVideoStreamInfo != null)
                        {
                            outVideoStreamInfo.FrameWidth = opt.Width;
                            outVideoStreamInfo.FrameHeight = opt.Height;
                        }

                        // Set resize interpolation method to Cubic
                        outVideoPin.Params.Add(Param.Video.Resize.InterpolationMethod, (int)InterpolationMethod.Cubic);

                        outputSocket.Pins.Add(outVideoPin);
                    }

                    // Add audio pins (copy as-is)
                    foreach (var inAudioPin in inputSocket.Pins)
                    {
                        if (inAudioPin.StreamInfo.MediaType == MediaType.Audio)
                        {
                            var outAudioPin = new MediaPin();
                            outAudioPin.StreamInfo = (StreamInfo)inAudioPin.StreamInfo.Clone();
                            outputSocket.Pins.Add(outAudioPin);
                        }
                    }

                    transcoder.Outputs.Add(outputSocket);
                }

                bool result = transcoder.Open();
                PrintError("Open Transcoder", transcoder.Error);
                if (!result)
                    return false;

                result = transcoder.Run();
                PrintError("Run Transcoder", transcoder.Error);
                if (!result)
                    return false;

                transcoder.Close();
            }

            Console.WriteLine("Output: " + opt.OutputFile);

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
    }
}
