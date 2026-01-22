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

            // Set license information. Without this AVBlocks runs in Demo mode.
            // Library.SetLicense("<license-string>");

            bool encodeResult = Encode(opt);

            Library.Shutdown();

            return encodeResult ? (int)ExitCodes.Success : (int)ExitCodes.EncodingError;
        }

        static bool Encode(Options opt)
        {
            // transcoder will fail if output exists (by design)
            DeleteFile(opt.OutputFile);

            // Create output directory if needed
            string outputDir = System.IO.Path.GetDirectoryName(opt.OutputFile);
            if (!string.IsNullOrEmpty(outputDir))
                System.IO.Directory.CreateDirectory(outputDir);

            // create input socket
            MediaSocket inSocket = new MediaSocket();
            inSocket.File = opt.InputFile;

            // create output socket
            MediaSocket outSocket = CreateOutputSocket(opt);

            // create Transcoder
            using (Transcoder transcoder = new Transcoder())
            {
                transcoder.AllowDemoMode = true;
                transcoder.Inputs.Add(inSocket);
                transcoder.Outputs.Add(outSocket);

                bool res = transcoder.Open();
                PrintError("Transcoder open", transcoder.Error);
                if (!res)
                    return false;

                res = transcoder.Run();
                PrintError("Transcoder run", transcoder.Error);
                if (!res)
                    return false;

                transcoder.Close();
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

        static MediaSocket CreateOutputSocket(Options opt)
        {
            MediaSocket socket = new MediaSocket();
            socket.File = opt.OutputFile;
            socket.StreamType = StreamType.Wave;

            MediaPin pin = new MediaPin();
            socket.Pins.Add(pin);

            AudioStreamInfo asi = new AudioStreamInfo();
            pin.StreamInfo = asi;

            asi.StreamType = StreamType.MuLawPCM;
            // G.711 μ-law typically uses 8000 Hz sample rate and mono channel
            asi.SampleRate = 8000;
            asi.Channels = 1;

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
