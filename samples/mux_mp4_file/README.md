## mux_mp4_file

Shows how to multiplex AAC audio file and AVC / H.264 video file into a MP4 file.


### Command Line

```sh
mux_mp4_file --audio <AAC_file>.mp4 --video <H264_file>.mp4 --output <output>.mp4
```

###	Examples

List options:

```sh
./bin/net60/mux_mp4_file --help
mux_mp4_file 1.0.0.0
Copyright (C) 2023 Primo Software

  -?, --help      Display this help screen

  -a, --audio     input AAC files. Can be used multiple times

  -v, --video     input H264 files. Can be used multiple times

  -o, --output    output file

  --help          Display this help screen.

  --version       Display version information.
```


Multiplex the MP4 files `./assets/aud/big_buck_bunny_trailer.aud.mp4` and `./assets/vid/big_buck_bunny_trailer.vid.mp4` into a MP4 file `mux_mp4_file.mp4`: 

```sh
# Linux and macOS 
mkdir -p ./output/mux_mp4_file

./bin/net60/mux_mp4_file \
    --audio ./assets/aud/big_buck_bunny_trailer.aud.mp4 \
    --video ./assets/vid/big_buck_bunny_trailer.vid.mp4 \
    --output ./output/mux_mp4_file/big_buck_bunny_trailer.mp4
```

```powershell
# Windows
mkdir -Force -Path ./output/mux_mp4_file

./bin/net60/mux_mp4_file `
    --audio ./assets/aud/big_buck_bunny_trailer.aud.mp4 `
    --video ./assets/vid/big_buck_bunny_trailer.vid.mp4 `
    --output ./output/mux_mp4_file/big_buck_bunny_trailer.mp4
```
