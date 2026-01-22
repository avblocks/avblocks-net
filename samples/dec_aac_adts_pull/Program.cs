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

            bool decodeResult = Decode(opt);

            Library.Shutdown();

            return decodeResult ? (int)ExitCodes.Success : (int)ExitCodes.DecodingError;
        }

        static MediaSocket CreateDecoderOutputSocket()
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

        static Transcoder CreateWavWriter(string outputFile)
        {
            // input stream info, pin and socket
            AudioStreamInfo infmt = new AudioStreamInfo();
            infmt.StreamType = StreamType.LPCM;
            infmt.Channels = 2;
            infmt.SampleRate = 48000;
            infmt.BitsPerSample = 16;

            MediaPin inPin = new MediaPin();
            inPin.StreamInfo = infmt;

            MediaSocket inSocket = new MediaSocket();
            inSocket.StreamType = StreamType.LPCM;
            inSocket.Pins.Add(inPin);

            // output stream info, pin and socket
            AudioStreamInfo outfmt = new AudioStreamInfo();
            outfmt.StreamType = StreamType.LPCM;
            outfmt.Channels = 2;
            outfmt.SampleRate = 48000;
            outfmt.BitsPerSample = 16;

            MediaPin outPin = new MediaPin();
            outPin.StreamInfo = outfmt;

            MediaSocket outSocket = new MediaSocket();
            outSocket.StreamType = StreamType.Wave;
            outSocket.Pins.Add(outPin);

            // set output file
            outSocket.File = outputFile;

            // create transcoder
            Transcoder wavWriter = new Transcoder();
            wavWriter.AllowDemoMode = true;
            wavWriter.Inputs.Add(inSocket);
            wavWriter.Outputs.Add(outSocket);

            return wavWriter;
        }

        static bool Decode(Options opt)
        {
            // transcoder will fail if output exists (by design)
            DeleteFile(opt.OutputFile);

            // Create decoder transcoder
            using (Transcoder decoder = new Transcoder())
            {
                decoder.AllowDemoMode = true;

                MediaSocket inputSocket = new MediaSocket();
                inputSocket.File = opt.InputFile;
                decoder.Inputs.Add(inputSocket);
                decoder.Outputs.Add(CreateDecoderOutputSocket());

                if (!decoder.Open())
                {
                    PrintError("Decoder open", decoder.Error);
                    return false;
                }

                // Create WAV writer transcoder
                using (Transcoder wavWriter = CreateWavWriter(opt.OutputFile))
                {
                    if (!wavWriter.Open())
                    {
                        PrintError("WAV Writer open", wavWriter.Error);
                        decoder.Close();
                        return false;
                    }

                    // Pull-push decoding loop
                    int decoderOutputIndex = 0;
                    MediaSample pcmSample = new MediaSample();

                    bool decoderEos = false;
                    while (!decoderEos)
                    {
                        // Pull PCM sample from decoder
                        if (decoder.Pull(out decoderOutputIndex, pcmSample))
                        {
                            // Push PCM sample to WAV writer
                            if (!wavWriter.Push(0, pcmSample))
                            {
                                PrintError("WAV Writer push", wavWriter.Error);
                                decoder.Close();
                                wavWriter.Close();
                                return false;
                            }

                            continue;
                        }

                        // No more PCM data from decoder
                        ErrorInfo error = decoder.Error;
                        if (error.Facility == ErrorFacility.Codec &&
                            error.Code == (int)CodecError.EOS)
                        {
                            // Push null to signal EOS to WAV writer
                            wavWriter.Push(0, null);
                            decoderEos = true;

                            continue;
                        }

                        PrintError("Decoder pull", error);
                        decoder.Close();
                        wavWriter.Close();
                        return false;
                    }

                    wavWriter.Close();
                }

                decoder.Close();
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
            DecodingError = 2,
        }
    }
}
