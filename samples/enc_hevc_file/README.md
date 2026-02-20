## enc_hevc_file

How to convert a raw YUV video file to a compressed H.265 video file.   

### Command Line

```sh
enc_hevc_file --frame <width>x<height> --rate <fps> --color <color> --input <file.yuv> --output <file.h265> [--colors]
```

###	Examples

List options:

```sh
./bin/net10.0/enc_hevc_file --help
enc_hevc_file 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input     input YUV file

  -o, --output    output HEVC / H.265 file

  -r, --rate      input frame rate

  -f, --frame     input frame size

  -c, --color     input color space. Use --colors to list all supported input color spaces.

  --colors        list supported input color spaces

  --help          Display this help screen.

  --version       Display version information.
```

Encode a raw YUV video from `./assets/vid/foreman_qcif.yuv` to a H.265 video in `./output/enc_hevc_file/foreman_qcif.h265`:

```sh
# Linux and macOS 
mkdir -p ./output/enc_hevc_file

./bin/net10.0/enc_hevc_file \
    --input ./assets/vid/foreman_qcif.yuv \
    --output ./output/enc_hevc_file/foreman_qcif.h265 \
    --frame 176x144 \
    --rate 30 \
    --color yuv420
```

```powershell
# Windows
mkdir -Force -Path ./output/enc_hevc_file

./bin/net10.0/enc_hevc_file `
    --input ./assets/vid/foreman_qcif.yuv `
    --output ./output/enc_hevc_file/foreman_qcif.h265 `
    --frame 176x144 `
    --rate 30 `
    --color yuv420
```
