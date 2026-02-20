/*
 *  Copyright (c) 2013-2026 Primo Software. All Rights Reserved.
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

            bool decodeResult = DecodeAUs(opt);

            Library.Shutdown();

            return decodeResult ? (int)ExitCodes.Success : (int)ExitCodes.DecodingError;
        }

        static bool DecodeAUs(Options opt)
        {
            // delete output file if exists
            DeleteFile(opt.OutputFile);

            // create output directory
            string outputDir = Path.GetDirectoryName(opt.OutputFile);
            if (!string.IsNullOrEmpty(outputDir))
                Directory.CreateDirectory(outputDir);

            string firstAuFile = BuildAuPath(opt, 0);
            if (!File.Exists(firstAuFile))
            {
                Console.WriteLine("First AU file not found: " + firstAuFile);
                return false;
            }

            using (var transcoder = new Transcoder())
            {
                transcoder.AllowDemoMode = true;

                // configure transcoder using first AU file
                if (!ConfigureTranscoder(transcoder, firstAuFile, opt))
                    return false;

                if (!transcoder.Open())
                {
                    PrintError("Transcoder open", transcoder.Error);
                    return false;
                }

                // process all AU files
                for (int i = 0; ; i++)
                {
                    string auFile = BuildAuPath(opt, i);
                    if (!File.Exists(auFile))
                        break;

                    var sample = new MediaSample();
                    sample.Buffer = new MediaBuffer(File.ReadAllBytes(auFile));

                    if (!transcoder.Push(0, sample))
                    {
                        PrintError("Transcoder push", transcoder.Error);
                        return false;
                    }
                }

                if (!transcoder.Flush())
                {
                    PrintError("Transcoder flush", transcoder.Error);
                    return false;
                }

                transcoder.Close();
            }

            Console.WriteLine("Output file: " + opt.OutputFile);
            return true;
        }

        static bool ConfigureTranscoder(Transcoder transcoder, string auFile, Options opt)
        {
            using (var mediaInfo = new MediaInfo())
            {
                mediaInfo.Inputs[0].File = auFile;

                if (!mediaInfo.Open())
                {
                    PrintError("MediaInfo open", mediaInfo.Error);
                    return false;
                }

                // create input socket from media info
                MediaSocket inSocket = MediaSocket.FromMediaInfo(mediaInfo);
                inSocket.File = null;
                inSocket.Stream = null;

                // create output socket
                MediaSocket outSocket = CreateOutputSocket(opt);

                transcoder.Inputs.Add(inSocket);
                transcoder.Outputs.Add(outSocket);
            }

            return true;
        }

        static string BuildAuPath(Options opt, int index)
        {
            string pattern = "au_{0:0000}.h265";
            string path = Path.Combine(opt.InputDir, string.Format(pattern, index));
            return path;
        }

        static MediaSocket CreateOutputSocket(Options opt)
        {
            MediaSocket socket = new MediaSocket();
            socket.File = opt.OutputFile;
            socket.StreamType = StreamType.UncompressedVideo;

            MediaPin pin = new MediaPin();
            socket.Pins.Add(pin);

            VideoStreamInfo vsi = new VideoStreamInfo();
            pin.StreamInfo = vsi;

            vsi.StreamType = StreamType.UncompressedVideo;
            vsi.ColorFormat = opt.ColorId;
            vsi.ScanType = ScanType.Progressive;

            return socket;
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
