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
        /*
        Access Unit: one picture (VPS/SPS/PPS parameters + all slices)
        The elementary stream may contain optional access unit delimiters (AUD).
        A picture may consist of one or more slices.

        HEVC NAL unit header (2 bytes):
          forbidden_zero_bit    (1 bit)
          nal_unit_type         (6 bits)
          nuh_layer_id          (6 bits)
          nuh_temporal_id_plus1 (3 bits)

        nal_unit_type = (first_byte >> 1) & 0x3F
        */

        // Network Abstraction Layer Unit Type Definitions per H.265/HEVC spec (ITU-T H.265 Table 7-1)
        enum NALUType
        {
            TRAIL_N    = 0,   // Trailing picture, non-reference
            TRAIL_R    = 1,   // Trailing picture, reference
            TSA_N      = 2,   // Temporal sub-layer access, non-reference
            TSA_R      = 3,   // Temporal sub-layer access, reference
            STSA_N     = 4,   // Step-wise temporal sub-layer access, non-reference
            STSA_R     = 5,   // Step-wise temporal sub-layer access, reference
            RADL_N     = 6,   // Random access decodable leading, non-reference
            RADL_R     = 7,   // Random access decodable leading, reference
            RASL_N     = 8,   // Random access skipped leading, non-reference
            RASL_R     = 9,   // Random access skipped leading, reference
            BLA_W_LP   = 16,  // Broken link access with leading pictures
            BLA_W_RADL = 17,  // Broken link access with RADL pictures
            BLA_N_LP   = 18,  // Broken link access without leading pictures
            IDR_W_RADL = 19,  // IDR with RADL pictures
            IDR_N_LP   = 20,  // IDR without leading pictures
            CRA_NUT    = 21,  // Clean random access
            VPS_NUT    = 32,  // Video parameter set
            SPS_NUT    = 33,  // Sequence parameter set
            PPS_NUT    = 34,  // Picture parameter set
            AUD_NUT    = 35,  // Access unit delimiter
            EOS_NUT    = 36,  // End of sequence
            EOB_NUT    = 37,  // End of bitstream
            FD_NUT     = 38,  // Filler data
            PREFIX_SEI = 39,  // Supplemental enhancement information (prefix)
            SUFFIX_SEI = 40,  // Supplemental enhancement information (suffix)
        }

        static NALUType GetNalUnitType(byte firstByte)
        {
            // nal_unit_type occupies bits [6:1] of the first header byte
            int type = (firstByte >> 1) & 0x3F;
            return (NALUType)type;
        }

        static string NalUnitTypeName(NALUType type)
        {
            if (Enum.IsDefined(typeof(NALUType), type))
                return type.ToString();
            return "RSV_" + (int)type;
        }

        static void PrintNaluHeader(byte data)
        {
            // HEVC NAL unit header is 2 bytes; nal_unit_type is in the first byte
            var type = GetNalUnitType(data);
            Console.WriteLine("  {0}", NalUnitTypeName(type));
        }

        static void PrintNalus(MediaBuffer buffer)
        {
            // This parsing code assumes that MediaBuffer contains
            // a single Access Unit of one or more complete NAL Units
            while (buffer.DataSize > 1)
            {
                int dataOffset = buffer.DataOffset;
                int dataSize = buffer.DataSize;

                // is this a NALU with a 3-byte start code prefix (0x000001)?
                if (dataSize >= 3 &&
                    0x00 == buffer.Start[dataOffset + 0] &&
                    0x00 == buffer.Start[dataOffset + 1] &&
                    0x01 == buffer.Start[dataOffset + 2])
                {
                    if (dataSize >= 4)
                        PrintNaluHeader(buffer.Start[dataOffset + 3]);

                    buffer.SetData(dataOffset + 3, dataSize - 3);
                }
                // OR is this a NALU with a 4-byte start code prefix (0x00000001)?
                else if (dataSize >= 4 &&
                         0x00 == buffer.Start[dataOffset + 0] &&
                         0x00 == buffer.Start[dataOffset + 1] &&
                         0x00 == buffer.Start[dataOffset + 2] &&
                         0x01 == buffer.Start[dataOffset + 3])
                {
                    if (dataSize >= 5)
                        PrintNaluHeader(buffer.Start[dataOffset + 4]);

                    buffer.SetData(dataOffset + 4, dataSize - 4);
                }
                else
                {
                    buffer.SetData(dataOffset + 1, dataSize - 1);
                }

                // NOTE: Some NALUs may have a trailing zero byte. The `while`
                // condition `buffer.DataSize > 1` will effectively
                // skip the trailing zero byte.
            }
        }

        static void WriteAuFile(string outputDir, int auIndex, MediaBuffer buffer)
        {
            string filePath = Path.Combine(outputDir,
                string.Format("au_{0:0000}.h265", auIndex));

            using (var file = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                file.BaseStream.Write(buffer.Start, buffer.DataOffset, buffer.DataSize);
            }
        }

        static bool ParseH265Stream(Options opt)
        {
            DeleteDirectory(opt.OutputDir);

            MediaSocket inSocket = new MediaSocket();
            inSocket.File = opt.InputFile;

            // Create an output socket with one video pin configured for H.265
            MediaPin outPin = new MediaPin();
            VideoStreamInfo outVsi = new VideoStreamInfo();
            outVsi.StreamType = StreamType.H265;
            outPin.StreamInfo = outVsi;

            MediaSocket outSocket = new MediaSocket();
            outSocket.Pins.Add(outPin);

            using (Transcoder transcoder = new Transcoder())
            {
                transcoder.Inputs.Add(inSocket);
                transcoder.Outputs.Add(outSocket);

                var res = transcoder.Open();
                PrintError("transcoder open", transcoder.Error);

                if (!res)
                    return false;

                int inputIndex = 0;
                MediaSample accessUnit = new MediaSample();

                if (!MakeDir(opt.OutputDir))
                {
                    Console.WriteLine("cannot create output directory: " + opt.OutputDir);
                    return false;
                }

                int auIndex = 0;
                while (transcoder.Pull(out inputIndex, accessUnit))
                {
                    // Each call to Transcoder.Pull returns one Access Unit.
                    // The Access Unit may contain one or more NAL units.
                    var auBuffer = accessUnit.Buffer;
                    Console.WriteLine("AU #{0}, {1} bytes", auIndex, auBuffer.DataSize);
                    WriteAuFile(opt.OutputDir, auIndex, auBuffer);
                    PrintNalus(auBuffer);
                    ++auIndex;
                }

                transcoder.Close();
            }

            return true;
        }

        static void DeleteDirectory(string dir)
        {
            try
            {
                if (Directory.Exists(dir))
                    Directory.Delete(dir, true);
            }
            catch { }
        }

        static bool MakeDir(string dir)
        {
            try
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                return true;
            }
            catch { }
            return false;
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
            ParseError = 2,
        }

        static int Main(string[] args)
        {
            var opt = new Options();

            if (!opt.Prepare(args))
                return opt.Error ? (int)ExitCodes.OptionsError : (int)ExitCodes.Success;

            Library.Initialize();

            // Set license information. To run AVBlocks in demo mode, comment the next line out
            // Library.SetLicense("<license-string>");

            bool parseResult = ParseH265Stream(opt);

            Library.Shutdown();

            if (!parseResult)
                return (int)ExitCodes.ParseError;

            Console.WriteLine("\nSuccessfully parsed input file: " + opt.InputFile);
            Console.WriteLine("Output directory: " + opt.OutputDir);

            return (int)ExitCodes.Success;
        }
    }
}
