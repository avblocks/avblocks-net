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

            // Set license information. To run AVBlocks in demo mode, comment the next line out
            // Library.SetLicense("<license-string>");

            bool result = Encode(opt);

            Library.Shutdown();

            return result ? (int)ExitCodes.Success : (int)ExitCodes.EncodingError;
        }

        static VideoStreamInfo CreateYuvVideoInfo(Options opt)
        {
            var vsi = new VideoStreamInfo();
            vsi.StreamType = StreamType.UncompressedVideo;
            vsi.FrameRate = opt.Fps;
            vsi.FrameWidth = opt.Width;
            vsi.FrameHeight = opt.Height;
            vsi.ColorFormat = opt.Color.Id;
            vsi.ScanType = ScanType.Progressive;
            return vsi;
        }

        static MediaSocket CreateYuvReaderSocket(Options opt)
        {
            // Create YUV input socket with file
            var inSocket = new MediaSocket();
            inSocket.StreamType = StreamType.UncompressedVideo;
            inSocket.File = opt.InputFile;

            var inPin = new MediaPin();
            inPin.StreamInfo = CreateYuvVideoInfo(opt);
            inSocket.Pins.Add(inPin);

            // Create YUV output socket (no file — we pull frames manually)
            var outSocket = new MediaSocket();
            outSocket.StreamType = StreamType.UncompressedVideo;

            var outPin = new MediaPin();
            outPin.StreamInfo = CreateYuvVideoInfo(opt);
            outSocket.Pins.Add(outPin);

            return inSocket;
        }

        static MediaSocket CreateH264EncoderInputSocket(Options opt)
        {
            var socket = new MediaSocket();
            socket.StreamType = StreamType.UncompressedVideo;

            var pin = new MediaPin();
            pin.StreamInfo = CreateYuvVideoInfo(opt);
            socket.Pins.Add(pin);

            return socket;
        }

        static MediaSocket CreateH264EncoderOutputSocket(Options opt)
        {
            var socket = new MediaSocket();
            socket.File = opt.OutputFile;
            socket.StreamType = StreamType.H264;
            socket.StreamSubType = StreamSubType.AvcAnnexB;

            var pin = new MediaPin();
            var vsi = new VideoStreamInfo();
            vsi.StreamType = StreamType.H264;
            vsi.StreamSubType = StreamSubType.AvcAnnexB;
            vsi.FrameRate = opt.Fps;
            vsi.FrameWidth = opt.Width;
            vsi.FrameHeight = opt.Height;
            vsi.ColorFormat = opt.Color.Id;
            vsi.ScanType = ScanType.Progressive;
            pin.StreamInfo = vsi;
            socket.Pins.Add(pin);

            return socket;
        }

        static bool EncodeH264Stream(Transcoder yuvReader, Transcoder h264Encoder)
        {
            int inputIndex = 0;
            MediaSample yuvFrame = new MediaSample();

            // Pull raw YUV frames from the reader and push them to the encoder
            while (true)
            {
                if (!yuvReader.Pull(out inputIndex, yuvFrame))
                    break;

                if (!h264Encoder.Push(0, yuvFrame))
                {
                    PrintError("H264 encoder push", h264Encoder.Error);
                    return false;
                }
            }

            // Check for EOS from the YUV reader
            var error = yuvReader.Error;
            if (ErrorFacility.Codec != error.Facility ||
                (int)CodecError.EOS != error.Code)
            {
                PrintError("YUV reader pull", error);
                return false;
            }

            // Flush the encoder to finalize the output
            if (!h264Encoder.Flush())
            {
                PrintError("H264 encoder flush", h264Encoder.Error);
                return false;
            }

            return true;
        }

        static bool Encode(Options opt)
        {
            DeleteFile(opt.OutputFile);

            // Create YUV reader transcoder (reads from file, pulls frames)
            var yuvInSocket = CreateYuvReaderSocket(opt);
            var yuvOutSocket = new MediaSocket();
            yuvOutSocket.StreamType = StreamType.UncompressedVideo;

            var yuvOutPin = new MediaPin();
            yuvOutPin.StreamInfo = CreateYuvVideoInfo(opt);
            yuvOutSocket.Pins.Add(yuvOutPin);

            using (var yuvReader = new Transcoder())
            {
                yuvReader.AllowDemoMode = true;
                yuvReader.Inputs.Add(yuvInSocket);
                yuvReader.Outputs.Add(yuvOutSocket);

                bool res = yuvReader.Open();
                PrintError("YUV reader open", yuvReader.Error);
                if (!res)
                    return false;

                // Create H.264 encoder transcoder (pushes frames)
                var h264InSocket = CreateH264EncoderInputSocket(opt);
                var h264OutSocket = CreateH264EncoderOutputSocket(opt);

                using (var h264Encoder = new Transcoder())
                {
                    h264Encoder.AllowDemoMode = true;
                    h264Encoder.Inputs.Add(h264InSocket);
                    h264Encoder.Outputs.Add(h264OutSocket);

                    bool encRes = h264Encoder.Open();
                    PrintError("H264 encoder open", h264Encoder.Error);
                    if (!encRes)
                    {
                        yuvReader.Close();
                        return false;
                    }

                    encRes = EncodeH264Stream(yuvReader, h264Encoder);
                    if (!encRes)
                    {
                        h264Encoder.Close();
                        yuvReader.Close();
                        return false;
                    }

                    h264Encoder.Close();
                    PrintError("H264 encoder close", h264Encoder.Error);
                }

                yuvReader.Close();
                PrintError("YUV reader close", yuvReader.Error);
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
