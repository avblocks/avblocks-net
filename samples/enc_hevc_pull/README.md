## enc_hevc_pull

Encode raw YUV video file to HEVC / H.265 Annex B video file using `Transcoder.Pull`.

### Command Line

```sh
enc_hevc_pull --frame <width>x<height> --rate <fps> --color <COLOR> --input <file.yuv> --output <file.h265> [--colors] [--help]
```

###	Examples

List options:

```sh
./bin/net10.0/enc_hevc_pull --help
enc_hevc_pull 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input     input YUV file

  -o, --output    output HEVC / H.265 file

  -r, --rate      input frame rate

  -f, --frame     input frame size

  -c, --color     input color space. Use --colors to list all supported input
                  color spaces.

  --colors        list supported input color spaces

  --help          Display this help screen.

  --version       Display version information.
```

List supported color spaces:

```sh
./bin/net10.0/enc_hevc_pull --colors

COLORS
---------
yv12                 Planar Y, V, U (4:2:0) (note V,U order!)
nv12                 Planar Y, merged U->V (4:2:0)
yuy2                 Composite Y->U->Y->V (4:2:2)
uyvy                 Composite U->Y->V->Y (4:2:2)
yuv411               Planar Y, U, V (4:1:1)
yuv420               Planar Y, U, V (4:2:0)
yuv422               Planar Y, U, V (4:2:2)
yuv444               Planar Y, U, V (4:4:4)
y411                 Composite Y, U, V (4:1:1)
y41p                 Composite Y, U, V (4:1:1)
bgr32                Composite B->G->R
bgra32               Composite B->G->R->A
bgr24                Composite B->G->R
bgr565               Composite B->G->R, 5 bit per B & R, 6 bit per G
bgr555               Composite B->G->R->A, 5 bit per component, 1 bit per A
bgr444               Composite B->G->R->A, 4 bit per component
gray                 Luminance component only
yuv420a              Planar Y, U, V, Alpha (4:2:0)
yuv422a              Planar Y, U, V, Alpha (4:2:2)
yuv444a              Planar Y, U, V, Alpha (4:4:4)
yvu9                 Planar Y, V, U, 9 bits per sample
```

Encode a raw YUV video from `./assets/vid/foreman_qcif.yuv` to a H.265 video in `./output/enc_hevc_pull/foreman_qcif.h265`:

```sh
# Linux and macOS 
mkdir -p ./output/enc_hevc_pull

./bin/net10.0/enc_hevc_pull \
    --input ./assets/vid/foreman_qcif.yuv \
    --output ./output/enc_hevc_pull/foreman_qcif.h265 \
    --frame 176x144 \
    --rate 30 \
    --color yuv420
```

```powershell
# Windows
mkdir -Force -Path ./output/enc_hevc_pull

./bin/net10.0/enc_hevc_pull `
    --input ./assets/vid/foreman_qcif.yuv `
    --output ./output/enc_hevc_pull/foreman_qcif.h265 `
    --frame 176x144 `
    --rate 30 `
    --color yuv420
```
