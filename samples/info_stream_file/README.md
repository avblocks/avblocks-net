## info_stream_file

How to use the MediaInfo API to extract audio / video stream information from a media file.   

### Command Line

```sh
info_stream_file --input <any_media_file>
```

###	Examples

List options:

```sh
./bin/net60/info_stream_file --help
info_stream_file 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input    input file

  --help         Display this help screen.

  --version      Display version information.
```

List the audio and video streams of the `./assets/mov/big_buck_bunny_trailer.mp4` movie trailer:

```sh
# Linux and macOS 
./bin/net60/info_stream_file \
    --input ./assets/mov/big_buck_bunny_trailer.mp4
```

```powershell
# Windows
./bin/net60/info_stream_file `
    --input ./assets/mov/big_buck_bunny_trailer.mp4
```
