# avblocks-net

AVBlocks .NET SDK (CLI Samples)

# Installation

Download and extract the AVBlocks .NET demo version and the demo assets for your platform. The SDK files will be placed under the `sdk/` directory and the demo assets under the `assets/` directory of this repository.

For the available versions check the [AVBlocks .NET Core](https://github.com/avblocks/avblocks-net-core/releases) releases.

## Linux

```bash
# select version and platform
tag="v3.4.0-demo.1"
platform="linux"

# download
mkdir -p ./sdk/net10.0
cd ./sdk/net10.0

# sdk
curl \
  --location \
  --output ./avblocks-net10.0-$tag-$platform.tar.gz \
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net10.0-$tag-$platform.tar.gz

# sha256 checksum
curl \
  --location \
  --output ./avblocks-net10.0-$tag-$platform.tar.gz.sha256 \
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net10.0-$tag-$platform.tar.gz.sha256

# verify sha256 checksum
shasum --check ./avblocks-net10.0-$tag-$platform.tar.gz.sha256

# extract
tar -xvf avblocks-net10.0-$tag-$platform.tar.gz

cd ../..
```

## macOS

```bash
# select version and platform
tag="v3.4.0-demo.1"
platform="darwin"

# download
mkdir -p ./sdk/net10.0
cd ./sdk/net10.0

# sdk
curl \
  --location \
  --output ./avblocks-net10.0-$tag-$platform.zip \
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net10.0-$tag-$platform.zip

# sha256 checksum
curl \
  --location \
  --output ./avblocks-net10.0-$tag-$platform.zip.sha256 \
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net10.0-$tag-$platform.zip.sha256

# verify sha256 checksum
shasum --check ./avblocks-net10.0-$tag-$platform.zip.sha256

# unzip
unzip avblocks-net10.0-$tag-$platform.zip

cd ../..
```

## Windows

> Scripts are PowerShell

```powershell
# select version and platform
$tag='v3.4.0-demo.1'
$platform='windows'

# download
new-item -Force -ItemType Directory ./sdk/net10.0
cd ./sdk/net10.0

# sdk
curl.exe `
  --location `
  --output ./avblocks-net10.0-$tag-$platform.zip `
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net10.0-$tag-$platform.zip

# sha256 checksum
curl.exe `
  --location `
  --output ./avblocks-net10.0-$tag-$platform.zip.sha256 `
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net10.0-$tag-$platform.zip.sha256

# verify checksum
$downloadedHash = (Get-FileHash -Algorithm SHA256 ./avblocks-net10.0-$tag-$platform.zip).Hash.ToLower()
$expectedHash = (Get-Content ./avblocks-net10.0-$tag-$platform.zip.sha256).Split(' ')[0].ToLower()
if ($downloadedHash -eq $expectedHash) { Write-Host "Checksum OK!"; } else { Write-Host "Checksum failed!"; }

# unzip
expand-archive -Force -Path avblocks-net10.0-$tag-$platform.zip -DestinationPath .

cd ../..
```

On Windows you can also download the .NET Framework 4.8 build (`avblocks-net48-$tag-$platform.zip`) into `sdk/net48`.

## Assets

These demo audio and video assets are used as input for the AVBlocks samples.

```bash
mkdir -p ./assets
cd ./assets

curl \
  --location \
  --output ./avblocks_assets_v5.zip \
  https://github.com/avblocks/avblocks-assets/releases/download/v5/avblocks_assets_v5.zip

# unzip
unzip avblocks_assets_v5.zip

cd ..
```

For more details, see the platform-specific setup guides in [docs/](docs/).

# Usage

## Getting Started

### Library Initialization

Before using any AVBlocks functionality, initialize the library:

```csharp
using PrimoSoftware.AVBlocks;

Library.Initialize();

// Set license information. To run AVBlocks in demo mode, comment the next line out
// Library.SetLicense("<license-string>");

// Your code here

Library.Shutdown();
```

## Core Concepts

### MediaSocket

A `MediaSocket` represents an input or output media endpoint. It describes:
- The media file location (`File` property)
- The stream type (`StreamType` property)
- One or more media pins (`Pins` collection)

```csharp
MediaSocket socket = new MediaSocket();
socket.File = "input.mp4";
socket.StreamType = StreamType.UncompressedVideo;
```

### MediaPin

A `MediaPin` represents a single media stream (audio or video) within a `MediaSocket`. Each pin contains:
- Stream information (`StreamInfo` property) - describes the format and characteristics of the stream
- Stream configuration for encoding/decoding

**Key Properties:**
- `StreamInfo` - Contains detailed format information (VideoStreamInfo or AudioStreamInfo)

Example for video input:
```csharp
MediaPin pin = new MediaPin();
VideoStreamInfo vsi = new VideoStreamInfo();
pin.StreamInfo = vsi;

vsi.StreamType = StreamType.UncompressedVideo;
vsi.FrameWidth = 176;
vsi.FrameHeight = 144;
vsi.ColorFormat = ColorFormat.YUV420;
vsi.FrameRate = 30.0;
vsi.ScanType = ScanType.Progressive;

socket.Pins.Add(pin);
```

### MediaInfo

`MediaInfo` is used to extract metadata and stream information from media files without processing the content.

**Key Features:**
- Extract audio/video stream details
- Read file metadata (title, artist, album, etc.)
- No transcoding required

```csharp
using (MediaInfo info = new MediaInfo())
{
    info.Inputs[0].File = "input.mp4";
    
    if (info.Open())
    {
        // Access stream information
        foreach (StreamInfo streamInfo in info.Outputs[0].Pins)
        {
            // Process stream info
        }
        
        // Access metadata
        foreach (var attr in info.Outputs[0].Metadata)
        {
            Console.WriteLine($"{attr.Name}: {attr.Value}");
        }
        
        info.Close();
    }
    else
    {
        // Handle error
        ErrorInfo error = info.Error;
    }
}
```

## Common Operations

### Media Information Extraction

#### Stream Information
```csharp
using (MediaInfo info = new MediaInfo())
{
    info.Inputs[0].File = inputFile;
    
    if (info.Open())
    {
        // Iterate through streams
        for (int streamIndex = 0; streamIndex < info.Outputs[0].Pins.Count; streamIndex++)
        {
            StreamInfo si = info.Outputs[0].Pins[streamIndex].StreamInfo;
            
            if (si.MediaType == MediaType.Video)
            {
                VideoStreamInfo vsi = (VideoStreamInfo)si;
                Console.WriteLine($"Video: {vsi.FrameWidth}x{vsi.FrameHeight}");
            }
            else if (si.MediaType == MediaType.Audio)
            {
                AudioStreamInfo asi = (AudioStreamInfo)si;
                Console.WriteLine($"Audio: {asi.SampleRate}Hz, {asi.Channels} channels");
            }
        }
        
        info.Close();
    }
}
```

#### Metadata Extraction
```csharp
using (MediaInfo info = new MediaInfo())
{
    info.Inputs[0].File = inputFile;
    
    if (info.Open())
    {
        foreach (var attr in info.Outputs[0].Metadata)
        {
            Console.WriteLine($"{attr.Name}: {attr.Value}");
        }
        info.Close();
    }
}
```

### Video Encoding

#### Encode Raw YUV to H.264/AVC
```csharp
// Create input socket for raw YUV
MediaSocket inSocket = new MediaSocket();
inSocket.StreamType = StreamType.UncompressedVideo;
inSocket.File = "input.yuv";

MediaPin inPin = new MediaPin();
VideoStreamInfo inVsi = new VideoStreamInfo();
inPin.StreamInfo = inVsi;

inVsi.StreamType = StreamType.UncompressedVideo;
inVsi.ScanType = ScanType.Progressive;
inVsi.FrameWidth = 176;
inVsi.FrameHeight = 144;
inVsi.ColorFormat = ColorFormat.YUV420;
inVsi.FrameRate = 30.0;

inSocket.Pins.Add(inPin);

// Create output socket for H.264
MediaSocket outSocket = new MediaSocket();
outSocket.File = "output.h264";
outSocket.StreamType = StreamType.H264;

MediaPin outPin = new MediaPin();
VideoStreamInfo outVsi = new VideoStreamInfo();
outPin.StreamInfo = outVsi;

outVsi.StreamType = StreamType.H264;
outVsi.FrameWidth = 176;
outVsi.FrameHeight = 144;
outVsi.FrameRate = 30.0;

outSocket.Pins.Add(outPin);

// Encode
using (Transcoder transcoder = new Transcoder())
{
    transcoder.Inputs.Add(inSocket);
    transcoder.Outputs.Add(outSocket);
    
    if (transcoder.Open())
    {
        if (transcoder.Run())
        {
            Console.WriteLine("Encoding successful");
        }
        transcoder.Close();
    }
}
```

#### Encode with Preset
```csharp
MediaSocket inSocket = new MediaSocket();
inSocket.File = "input.yuv";
// ... configure input

MediaSocket outSocket = new MediaSocket();
outSocket.File = "output.mp4";
outSocket.Preset = Preset.Video.Generic.MP4.Base_H264_AAC;

using (Transcoder transcoder = new Transcoder())
{
    transcoder.Inputs.Add(inSocket);
    transcoder.Outputs.Add(outSocket);
    
    if (transcoder.Open())
    {
        transcoder.Run();
        transcoder.Close();
    }
}
```

### Video Decoding

#### Decode H.264 to Raw YUV
```csharp
// Create input socket for H.264
MediaSocket inSocket = new MediaSocket();
inSocket.File = "input.h264";
inSocket.StreamType = StreamType.H264;

// Create output socket for raw YUV
MediaSocket outSocket = new MediaSocket();
outSocket.File = "output.yuv";
outSocket.StreamType = StreamType.UncompressedVideo;

MediaPin outPin = new MediaPin();
VideoStreamInfo outVsi = new VideoStreamInfo();
outPin.StreamInfo = outVsi;

outVsi.StreamType = StreamType.UncompressedVideo;
outVsi.ColorFormat = ColorFormat.YUV420;

outSocket.Pins.Add(outPin);

// Decode
using (Transcoder transcoder = new Transcoder())
{
    transcoder.Inputs.Add(inSocket);
    transcoder.Outputs.Add(outSocket);
    
    if (transcoder.Open())
    {
        transcoder.Run();
        transcoder.Close();
    }
}
```

### Audio Encoding

#### Encode WAV to AAC (ADTS)
```csharp
MediaSocket inSocket = new MediaSocket();
inSocket.File = "input.wav";

MediaSocket outSocket = new MediaSocket();
outSocket.File = "output.aac";
outSocket.StreamType = StreamType.Aac;
outSocket.StreamSubType = StreamSubType.AacAdts;

using (Transcoder transcoder = new Transcoder())
{
    transcoder.Inputs.Add(inSocket);
    transcoder.Outputs.Add(outSocket);
    
    if (transcoder.Open())
    {
        transcoder.Run();
        transcoder.Close();
    }
}
```

#### Encode WAV to MP3
```csharp
MediaSocket inSocket = new MediaSocket();
inSocket.File = "input.wav";

MediaSocket outSocket = new MediaSocket();
outSocket.File = "output.mp3";
outSocket.StreamType = StreamType.Mp3;

using (Transcoder transcoder = new Transcoder())
{
    transcoder.Inputs.Add(inSocket);
    transcoder.Outputs.Add(outSocket);
    
    if (transcoder.Open())
    {
        transcoder.Run();
        transcoder.Close();
    }
}
```

### Container Operations

#### Demux MP4 File
```csharp
// Extract video and audio streams from MP4
using (Transcoder transcoder = new Transcoder())
{
    // Input
    MediaSocket inSocket = new MediaSocket();
    inSocket.File = "input.mp4";
    transcoder.Inputs.Add(inSocket);
    
    // Output for video stream
    MediaSocket videoOut = new MediaSocket();
    videoOut.File = "video.mp4";
    videoOut.StreamType = StreamType.H264;
    transcoder.Outputs.Add(videoOut);
    
    // Output for audio stream
    MediaSocket audioOut = new MediaSocket();
    audioOut.File = "audio.mp4";
    audioOut.StreamType = StreamType.Aac;
    transcoder.Outputs.Add(audioOut);
    
    if (transcoder.Open())
    {
        transcoder.Run();
        transcoder.Close();
    }
}
```

#### Mux MP4 File
```csharp
// Combine separate audio and video files into MP4 container
using (Transcoder transcoder = new Transcoder())
{
    // Video input
    MediaSocket videoIn = new MediaSocket();
    videoIn.File = "video.h264.mp4";
    transcoder.Inputs.Add(videoIn);
    
    // Audio input
    MediaSocket audioIn = new MediaSocket();
    audioIn.File = "audio.aac.mp4";
    transcoder.Inputs.Add(audioIn);
    
    // Combined output
    MediaSocket outSocket = new MediaSocket();
    outSocket.File = "output.mp4";
    transcoder.Outputs.Add(outSocket);
    
    if (transcoder.Open())
    {
        transcoder.Run();
        transcoder.Close();
    }
}
```

### Error Handling

```csharp
using (Transcoder transcoder = new Transcoder())
{
    transcoder.Inputs.Add(inSocket);
    transcoder.Outputs.Add(outSocket);
    
    if (!transcoder.Open())
    {
        PrintError("Open Transcoder", transcoder.Error);
        return false;
    }
    
    if (!transcoder.Run())
    {
        PrintError("Run Transcoder", transcoder.Error);
        transcoder.Close();
        return false;
    }
    
    transcoder.Close();
}

static void PrintError(string action, ErrorInfo error)
{
    if (error != null)
    {
        Console.WriteLine($"Error: {action}");
        Console.WriteLine($"  Code: {error.Code}");
        Console.WriteLine($"  Message: {error.Message}");
        Console.WriteLine($"  Hint: {error.Hint}");
    }
}
```

# Examples

Complete working examples are available in the [samples](./samples) directory.

**Getting Started**
- [`simple_converter`](./samples/simple_converter) — Transcode a media file from H.264/AVC to H.265/HEVC using a `Transcoder` with hardcoded input and output file paths

**Media Info**
- [`info_metadata_file`](./samples/info_metadata_file) — Extract metadata information from a media file
- [`info_stream_file`](./samples/info_stream_file) — Extract the audio/video stream information from a media file

**Decoding Audio**
- [`dec_aac_adts_file`](./samples/dec_aac_adts_file) — Decode AAC file in ADTS format and save output to WAV file
- [`dec_aac_adts_pull`](./samples/dec_aac_adts_pull) — Decode AAC file in ADTS format using `Transcoder.Pull` and save output to WAV file
- [`dec_g711_alaw_file`](./samples/dec_g711_alaw_file) — Decode G.711 A-law WAV file to PCM WAV file
- [`dec_g711_ulaw_file`](./samples/dec_g711_ulaw_file) — Decode G.711 μ-law WAV file to PCM WAV file
- [`dec_mp3_file`](./samples/dec_mp3_file) — Decode MP3 file and save output to WAV file
- [`dec_opus_file`](./samples/dec_opus_file) — Decode Opus OGG file and save output to WAV file
- [`dec_vorbis_file`](./samples/dec_vorbis_file) — Decode Vorbis OGG file and save output to WAV file

**Decoding Video**
- [`dec_avc_au`](./samples/dec_avc_au) — Decode an H.264 stream using a sequence of files to simulate a stream of H.264 Access Units
- [`dec_avc_file`](./samples/dec_avc_file) — Decode AVC/H.264 compressed file to raw uncompressed YUV file
- [`dec_avc_pull`](./samples/dec_avc_pull) — Decode AVC/H.264 file to YUV file using `Transcoder.Pull`
- [`dec_hevc_au`](./samples/dec_hevc_au) — Decode an H.265 stream using a sequence of files to simulate a stream of H.265 Access Units
- [`dec_hevc_file`](./samples/dec_hevc_file) — Decode HEVC/H.265 compressed file to raw uncompressed YUV file
- [`dec_vp8_file`](./samples/dec_vp8_file) — Decode VP8 video in IVF container to raw uncompressed YUV file
- [`dec_vp9_file`](./samples/dec_vp9_file) — Decode VP9 video in IVF container to raw uncompressed YUV file

**Encoding Audio**
- [`enc_aac_adts_file`](./samples/enc_aac_adts_file) — Encode WAV file to AAC file in ADTS format
- [`enc_aac_adts_pull`](./samples/enc_aac_adts_pull) — Encode WAV file to AAC file in ADTS format using `Transcoder.Pull`
- [`enc_aac_adts_push`](./samples/enc_aac_adts_push) — Encode WAV file to AAC file in ADTS format using `Transcoder.Push`
- [`enc_g711_alaw_file`](./samples/enc_g711_alaw_file) — Encode WAV file to G.711 A-law WAV file
- [`enc_g711_ulaw_file`](./samples/enc_g711_ulaw_file) — Encode WAV file to G.711 μ-law WAV file
- [`enc_mp3_file`](./samples/enc_mp3_file) — Encode WAV file to MP3 file
- [`enc_mp3_pull`](./samples/enc_mp3_pull) — Encode WAV file to MP3 file using `Transcoder.Pull`
- [`enc_mp3_push`](./samples/enc_mp3_push) — Encode WAV file to MP3 file using `Transcoder.Push`
- [`enc_opus_file`](./samples/enc_opus_file) — Encode WAV file to Opus OGG file
- [`enc_vorbis_file`](./samples/enc_vorbis_file) — Encode WAV file to Vorbis OGG file

**Encoding Video**
- [`enc_avc_file`](./samples/enc_avc_file) — Encode raw YUV video file to AVC/H.264 video file using `Transcoder.Run`
- [`enc_avc_pull`](./samples/enc_avc_pull) — Encode raw YUV video file to AVC/H.264 video file using `Transcoder.Pull`
- [`enc_hevc_file`](./samples/enc_hevc_file) — Encode raw YUV video file to HEVC/H.265 video file using `Transcoder.Run`
- [`enc_hevc_pull`](./samples/enc_hevc_pull) — Encode raw YUV video file to HEVC/H.265 video file using `Transcoder.Pull`
- [`enc_preset_file`](./samples/enc_preset_file) — Convert a raw YUV video file to a compressed video file using an AVBlocks preset
- [`enc_vp8_file`](./samples/enc_vp8_file) — Encode raw YUV video file to VP8 video in IVF container
- [`enc_vp9_file`](./samples/enc_vp9_file) — Encode raw YUV video file to VP9 video in IVF container

**Muxing / Demuxing**
- [`demux_mp4_file`](./samples/demux_mp4_file) — Extract the first audio and video elementary stream from an MP4 container into separate MP4 files
- [`demux_webm_file`](./samples/demux_webm_file) — Extract the audio and video elementary streams from a WebM container into separate files
- [`dump_avc_au`](./samples/dump_avc_au) — Split an H.264 (AVC) elementary stream into Access Units, writing each to a separate file
- [`dump_hevc_au`](./samples/dump_hevc_au) — Split an H.265 (HEVC) elementary stream into Access Units, writing each to a separate file
- [`mux_mp4_file`](./samples/mux_mp4_file) — Multiplex two single-stream MP4 files (AAC audio + H.264 video) into an MP4 container
- [`mux_webm_file`](./samples/mux_webm_file) — Multiplex Vorbis/OGG audio and VP8/IVF video files into a WebM container

**Advanced**
- [`re-encode`](./samples/re-encode) — Re-encode the audio and video streams of an MP4 file back into an MP4 output
- [`slideshow`](./samples/slideshow) — Create a video clip from a sequence of images

**Audio Processing**
- [`audio_upsample`](./samples/audio_upsample) — Upsample audio from 44.1 KHz to 48 KHz

**Video Processing**
- [`video_crop`](./samples/video_crop) — Crop a video by removing pixels from the edges
- [`video_framerate`](./samples/video_framerate) — Change the frame rate of a video from 24 fps to 30 fps
- [`video_pad`](./samples/video_pad) — Add black border padding around a video
- [`video_upscale`](./samples/video_upscale) — Upscale a video to Full HD (1920×1080) using bicubic interpolation

# Development

## macOS

### Download AVBlocks .NET Core and Assets

See [Download .NET Core and Assets on macOS](./docs/download-avblocks-net-core-and-assets-mac.md) 

### Setup

See [Setup for macOS](./docs/setup-mac.md)

### Build

See [Build on macOS](./docs/build-mac.md)

### Run

See [README](./samples/README.md) in the `samples` subdirectory. 

## Linux

### Download AVBlocks .NET Core and Assets

See [Download .NET Core and Assets on Linux](./docs/download-avblocks-net-core-and-assets-linux.md) 

### Setup

See [Setup for Linux](./docs/setup-linux.md)

### Build

See [Build on Linux](./docs/build-linux.md)

### Run

See [README](./samples/README.md) in the `samples` subdirectory. 

## Windows

### Download AVBlocks .NET Core and Assets

See [Download .NET Core and Assets on Windows](./docs/download-avblocks-net-core-and-assets-windows.md) 

### Setup

See [Setup for Windows](./docs/setup-windows.md)

### Build

See [Build on Windows](./docs/build-windows.md)

### Run

See [README](./samples/README.md) in the `samples` subdirectory. 

# How to obtain Commercial License

See [License Options](https://avblocks.com/license/) for details.

We offer discounts for:

- Competitive product
- Startup
- Educational institution
- Open source project

