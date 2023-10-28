## re-encode

Takes an MP4 input and re-encodes the audio and video streams back into MP4 output. It shows how to force encoding of individual streams even when it is not needed. 

### Command Line

```sh
re-encode [--audio] [--video] --input <mp4_file.mp4> --output <mp4_file.mp4> 
```

###	Examples

List options:

```sh
./bin/net60/re-encode --help
re-encode 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input     input file

  -o, --output    output file

  -a, --audio     Force audio re-encoding.

  -v, --video     Force video re-encoding.

  --help          Display this help screen.

  --version       Display version information.
```

Re-encode the `./assets/mov/big_buck_bunny_trailer.mp4` file. Only the video stream will be re-encoded, the audio stream will be copied as is:

```sh
# Linux and macOS 
mkdir -p ./output/re-encode

./bin/net60/re-encode \
    --video \
    --input ./assets/mov/big_buck_bunny_trailer.mp4 \
    --output ./output/re-encode/big_buck_bunny_trailer.mp4
```

```powershell
# Windows
mkdir -Force -Path ./output/re-encode

./bin/net60/re-encode `
    --video `
    --input ./assets/mov/big_buck_bunny_trailer.mp4 `
    --output ./output/re-encode/big_buck_bunny_trailer.mp4
```
