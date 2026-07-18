using System;
using PrimoSoftware.AVBlocks;

namespace CliSample
{
    class Program
    {
        static int Main(string[] args)
        {
            Library.Initialize();

            // Set license information. To run AVBlocks in demo mode, comment the next line out
            // Library.SetLicense("<license-string>");

            var inputFile = "Wildlife_h264_aac.mp4";
            var outputFile = "Wildlife_h265_aac.mp4";

            using (var inputInfo = new MediaInfo())
            {
                inputInfo.Inputs[0].File = inputFile;

                if (inputInfo.Open())
                {
                    var inputSocket = MediaSocket.FromMediaInfo(inputInfo);

                    // Start with same output as the input, which is MP4 / H.264 + AAC
                    var outputSocket = (MediaSocket)inputSocket.Clone();

                    // Get the output video stream info
                    var outVideoStream = (VideoStreamInfo)outputSocket.Pins[0].StreamInfo;

                    // Change the video stream type to H.265 (HEVC)
                    // and the stream subtype to HEVC Annex B
                    outVideoStream.StreamType = StreamType.H265;
                    outVideoStream.StreamSubType = StreamSubType.HevcAnnexB;

                    // Input is H.264/AVC at 700 kbps.
                    // With H.265/HEVC we can use lower bitrate, e.g. 500 kbps.
                    outVideoStream.Bitrate = 500000;

                    // Change the output file name to Wildlife_h265_aac.mp4
                    outputSocket.File = outputFile;

                    // Create Transcoder and configure it with
                    // the input and output sockets
                    using (var transcoder = new Transcoder())
                    {
                        transcoder.Inputs.Add(inputSocket);
                        transcoder.Outputs.Add(outputSocket);

                        // Allow demo mode for the transcoder when
                        // using the demo version of the library
                        transcoder.AllowDemoMode = true;

                        // Run the transcoder
                        if (transcoder.Open())
                        {
                            transcoder.Run();
                            transcoder.Close();
                        }
                        else
                        {
                            Console.Error.WriteLine("transcoder.Open() failed: {0}", transcoder.Error.Message);
                        }
                    }
                }
                else
                {
                    Console.Error.WriteLine("inputInfo.Open() failed: {0}", inputInfo.Error.Message);
                }
            }

            Library.Shutdown();
            return 0;
        }
    }
}
