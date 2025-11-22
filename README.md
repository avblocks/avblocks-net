# avblocks-net

AVBlocks .NET SDK (CLI Samples)

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

## Available Samples

See [samples/README.md](./samples/README.md) for a complete list of working examples including:

- **Media Info**: Extract stream information and metadata
- **Encoding**: AAC, MP3, H.264/AVC, with presets
- **Decoding**: H.264 to YUV
- **Container Operations**: Demux/Mux MP4 files
- **Misc**: Re-encoding, slideshow generation

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

