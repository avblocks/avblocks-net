/*
 *  Copyright (c) 2013-2025 Primo Software. All Rights Reserved.
 *
 *  Use of this source code is governed by a MIT license
 *  that can be found in the LICENSE file in the root of the source
 *  tree.  
*/
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

            bool decodeResult = DecodeH264Stream(opt);

            Library.Shutdown();

            return decodeResult ? (int)ExitCodes.Success : (int)ExitCodes.DecodingError;
        }

        static bool DecodeH264Stream(Options opt)
        {
            // delete output file if exists
            DeleteFile(opt.OutputFile);

            // create output directory
            string outputDir = Path.GetDirectoryName(opt.OutputFile);
            if (!string.IsNullOrEmpty(outputDir))
                Directory.CreateDirectory(outputDir);

            // Create an input socket from file
            MediaSocket inSocket = new MediaSocket();
            inSocket.File = opt.InputFile;

            // Create an output socket with one YUV 4:2:0 video pin
            VideoStreamInfo outStreamInfo = new VideoStreamInfo();
            outStreamInfo.StreamType = StreamType.UncompressedVideo;
            outStreamInfo.ColorFormat = ColorFormat.YUV420;
            outStreamInfo.ScanType = ScanType.Progressive;

            MediaPin outPin = new MediaPin();
            outPin.StreamInfo = outStreamInfo;

            MediaSocket outSocket = new MediaSocket();
            outSocket.StreamType = StreamType.UncompressedVideo;
            outSocket.Pins.Add(outPin);

            // Create Transcoder
            using (var transcoder = new Transcoder())
            {
                transcoder.AllowDemoMode = true;
                transcoder.Inputs.Add(inSocket);
                transcoder.Outputs.Add(outSocket);

                if (!transcoder.Open())
                {
                    PrintError("Transcoder open", transcoder.Error);
                    return false;
                }

                int inputIndex;
                MediaSample yuvFrame = new MediaSample();
                int frameCounter = 0;

                using (FileStream outfile = File.OpenWrite(opt.OutputFile))
                {
                    while (transcoder.Pull(out inputIndex, yuvFrame))
                    {
                        // Each call to Transcoder.Pull returns a raw YUV 4:2:0 frame
                        outfile.Write(yuvFrame.Buffer.Start, yuvFrame.Buffer.DataOffset, yuvFrame.Buffer.DataSize);
                        ++frameCounter;
                    }

                    PrintError("Transcoder pull", transcoder.Error);
                }

                transcoder.Close();

                Console.WriteLine("Frames decoded: {0}", frameCounter);
                Console.WriteLine("Output file: {0}", opt.OutputFile);
            }

            return true;
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
            DecodingError = 2,
        }
    }
}
