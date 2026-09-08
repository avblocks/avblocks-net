using System;
using PrimoSoftware.AVBlocks;

class Program
{
    static void Main(string[] args)
    {
        Library.Initialize();

        string inputPath = "AndrewConnolly_2014.mp4";
        string outputPath = "AndrewConnolly_2014_1080p.mp4";

        try
        {
            using (var mediaInfo = new MediaInfo())
            {
                mediaInfo.Inputs[0].File = inputPath;
                if (!mediaInfo.Open())
                {
                    PrintError("Open MediaInfo", mediaInfo.Error);
                    return;
                }

                var inputSocket = MediaSocket.FromMediaInfo(mediaInfo);

                var outputSocket = (MediaSocket)inputSocket.Clone();
                outputSocket.File = outputPath;

                var outVideoPin = outputSocket.Pins[0];

                var outVideoStreamInfo = outVideoPin.StreamInfo as VideoStreamInfo;
                if (outVideoStreamInfo != null)
                {
                    outVideoStreamInfo.FrameWidth = 1920;
                    outVideoStreamInfo.FrameHeight = 1080;
                }

                outVideoPin.Params.Add(Param.Video.Resize.InterpolationMethod, (int)InterpolationMethod.Cubic);

                using (var transcoder = new Transcoder())
                {
                    transcoder.AllowDemoMode = true;

                    transcoder.Inputs.Add(inputSocket);
                    transcoder.Outputs.Add(outputSocket);

                    if (transcoder.Open())
                    {
                        transcoder.Run();
                        transcoder.Close();
                    }
                    else
                    {
                        PrintError("Transcoder Open", transcoder.Error);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }

        Library.Shutdown();
    }

    static void PrintError(string action, ErrorInfo error)
    {
        if (action != null)
            Console.Write("{0}: ", action);

        if (ErrorFacility.Success == error.Facility)
        {
            Console.WriteLine("Success");
            return;
        }
        else
        {
            Console.WriteLine("{0}, facility:{1} code:{2} hint:{3}",
                error.Message ?? "", error.Facility, error.Code, error.Hint ?? "");
        }
    }
}
