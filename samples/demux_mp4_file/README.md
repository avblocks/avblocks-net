## demux_mp4_file

Shows how to extract the audio and video streams from MP4 file and save to two separate MP4 files, one for the audio, and one for the video.     

### Command Line

```sh
demux_mp4_file --input <input_mp4_file> --output <output_mp4_filename_without_extension>
```

###	Examples

List options:

```sh
./bin/net10.0/demux_mp4_file --help
demux_mp4_file 1.0.0.0
Copyright (C) 2023 Primo Software

  -?, --help      Display this help screen

  -i, --input     Input mp4 file

  -o, --output    Output mp4 filename (without extension)

  --help          Display this help screen.

  --version       Display version information.
```

Extract the audio and video streams from `./assets/mov/big_buck_bunny_trailer.mp4` and save to `./output/demux_mp4_file/big_buck_bunny_trailer.aud.mp4` and `./output/demux_mp4_file/big_buck_bunny_trailer.vid.mp4`:

```sh
# Linux and macOS 
mkdir -p ./output/demux_mp4_file

./bin/net10.0/demux_mp4_file \
    --input ./assets/mov/big_buck_bunny_trailer.mp4 \
    --output ./output/demux_mp4_file/big_buck_bunny_trailer
```

```powershell
# Windows
mkdir -Force -Path ./output/demux_mp4_file

./bin/net10.0/demux_mp4_file `
    --input ./assets/mov/big_buck_bunny_trailer.mp4 `
    --output ./output/demux_mp4_file/big_buck_bunny_trailer
```
