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

            // Set license information. Without this AVBlocks runs in Demo mode.
            // Library.SetLicense("<license-string>");

            bool encodeResult = Encode(opt);

            Library.Shutdown();

            return encodeResult ? (int)ExitCodes.Success : (int)ExitCodes.EncodingError;
        }

        static MediaSocket CreateInputSocket()
        {
            MediaSocket socket = new MediaSocket();
            socket.StreamType = StreamType.LPCM;

            MediaPin pin = new MediaPin();
            AudioStreamInfo asi = new AudioStreamInfo();
            asi.StreamType = StreamType.LPCM;
            asi.Channels = 2;
            asi.SampleRate = 48000;
            asi.BitsPerSample = 16;

            pin.StreamInfo = asi;
            socket.Pins.Add(pin);

            return socket;
        }

        static MediaSocket CreateOutputSocket(string outputFile)
        {
            MediaSocket socket = new MediaSocket();
            socket.StreamType = StreamType.Aac;
            socket.StreamSubType = StreamSubType.AacAdts;
            socket.File = outputFile;

            MediaPin pin = new MediaPin();
            AudioStreamInfo asi = new AudioStreamInfo();
            asi.StreamType = StreamType.Aac;
            asi.StreamSubType = StreamSubType.AacAdts;

            // You can change the sampling rate and the number of the channels
            // asi.Channels = 1;
            // asi.SampleRate = 44100;

            pin.StreamInfo = asi;
            socket.Pins.Add(pin);

            return socket;
        }

        static Transcoder CreateWavReader(string inputFile)
        {
            // input socket
            // it will automatically detect stream info from the file
            MediaSocket wavInputSocket = new MediaSocket();
            wavInputSocket.File = inputFile;

            // output stream info
            AudioStreamInfo pcmAsi = new AudioStreamInfo();
            pcmAsi.StreamType = StreamType.LPCM;
            pcmAsi.Channels = 2;
            pcmAsi.SampleRate = 48000;
            pcmAsi.BitsPerSample = 16;

            // output pin
            MediaPin pcmPin = new MediaPin();
            pcmPin.StreamInfo = pcmAsi;

            // output socket - we need LPCM stream type
            MediaSocket pcmOutputSocket = new MediaSocket();
            pcmOutputSocket.StreamType = StreamType.LPCM;
            pcmOutputSocket.Pins.Add(pcmPin);

            // create transcoder
            Transcoder wavReader = new Transcoder();
            wavReader.AllowDemoMode = true;
            wavReader.Inputs.Add(wavInputSocket);
            wavReader.Outputs.Add(pcmOutputSocket);

            return wavReader;
        }

        static bool Encode(Options opt)
        {
            // transcoder will fail if output exists (by design)
            DeleteFile(opt.OutputFile);

            // Create output directory if needed
            string outputDir = Path.GetDirectoryName(opt.OutputFile);
            if (!string.IsNullOrEmpty(outputDir))
                Directory.CreateDirectory(outputDir);

            // Create WAV reader transcoder
            using (Transcoder wavReader = CreateWavReader(opt.InputFile))
            {
                if (!wavReader.Open())
                {
                    PrintError("WAV Reader open", wavReader.Error);
                    return false;
                }

                // Create encoder transcoder
                using (Transcoder encoder = new Transcoder())
                {
                    encoder.AllowDemoMode = true;
                    encoder.Inputs.Add(CreateInputSocket());
                    encoder.Outputs.Add(CreateOutputSocket(opt.OutputFile));

                    if (!encoder.Open())
                    {
                        PrintError("Encoder open", encoder.Error);
                        wavReader.Close();
                        return false;
                    }

                    // Push encoding loop
                    int wavOutputIndex = 0;
                    MediaSample pcmSample = new MediaSample();

                    bool wavEos = false;
                    while (!wavEos)
                    {
                        // Get PCM sample from WAV reader
                        if (wavReader.Pull(out wavOutputIndex, pcmSample))
                        {
                            // Push PCM sample to encoder
                            if (!encoder.Push(0, pcmSample))
                            {
                                PrintError("Encoder push", encoder.Error);
                                wavReader.Close();
                                encoder.Close();
                                return false;
                            }
                        }
                        else
                        {
                            // No more PCM data from WAV reader
                            ErrorInfo error = wavReader.Error;
                            if (error.Facility == ErrorFacility.Codec &&
                                error.Code == (int)CodecError.EOS)
                            {
                                // Push null to signal EOS to encoder
                                encoder.Push(0, null);
                                wavEos = true;
                            }
                            else
                            {
                                PrintError("WAV Reader pull", error);
                                wavReader.Close();
                                encoder.Close();
                                return false;
                            }
                        }
                    }

                    wavReader.Close();
                    encoder.Close();
                }
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
            EncodingError = 2,
        }
    }
}
