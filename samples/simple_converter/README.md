# simple_converter

Transcodes a media file from `H.264/AVC` to `H.265/HEVC` using AVBlocks Transcoder with hardcoded input and output file paths.

This sample demonstrates the basic AVBlocks transcoding workflow:

1. Initialize the AVBlocks library
2. Create a `MediaInfo` object to probe the input file
3. Create an input `MediaSocket` from the probed media info
4. Clone the input socket to create an output socket and change the video stream type to `H.265 (HEVC)`
5. Configure a `Transcoder` with the input and output sockets
6. Run the transcode operation

No command line parsing is used — the input and output file paths are hardcoded in the source code.

## Building

From the repository root:

```sh
dotnet build samples.sln
```

The built executable will be placed in `bin/net10.0/simple_converter` (or `bin/net48/simple_converter.exe` on Windows).

## Running

1. Download the sample video:

    ```sh
    curl -L -o Wildlife_h264_aac.mp4 https://archive.org/download/WildlifeSampleVideo/Wildlife.mp4
    ```

2. Run the sample from the sample directory (the input/output file paths are relative to the working directory):

    ```sh
    cd samples/simple_converter
    ../../bin/net10.0/simple_converter
    ```

3. The transcoded output file `Wildlife_h265_aac.mp4` will be created in the `samples/simple_converter` directory.

## Notes

- The input file `Wildlife_h264_aac.mp4` and output file `Wildlife_h265_aac.mp4` paths are hardcoded in `Program.cs`. They are relative to the working directory from which the executable is run.
- If `transcoder.Open()` fails, it may be because `Wildlife_h265_aac.mp4` already exists in the directory. Delete the output file and try again.
- The output uses the same container and audio as the input (MP4 / AAC) with the video stream type changed to `StreamType.H265`, producing an MP4 container with H.265 video and AAC audio streams.
