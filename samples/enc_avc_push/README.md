## enc_avc_push

How to encode raw YUV video file to AVC / H.264 Annex B video file using `Transcoder::push`.

### Command Line

```sh
enc_avc_push --frame <width>x<height> --rate <fps> --color <color> --input <file.yuv> --output <file.h264> [--colors]
```

###	Examples

List options:

```sh
./bin/x64/net10.0/enc_avc_push --help
enc_avc_push 1.0.0.0
Copyright (C) 2026 Primo Software

  -i, --input     input YUV file

  -o, --output    output H264 file

  -r, --rate      input frame rate

  -f, --frame     input frame sizes <width>x<height>

  -c, --color     input color format. Use --colors to list all supported color formats

  --colors        list COLOR constants

  --help          Display this help screen.

  --version       Display version information.
```

List supported color formats for input:

```sh
./bin/x64/net10.0/enc_avc_push --colors

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

Encode a raw YUV video from `./assets/vid/foreman_qcif.yuv` to a H.264 video in `./output/enc_avc_push/foreman_qcif.h264`:

```sh
# Linux and macOS 
mkdir -p ./output/enc_avc_push

./bin/x64/net10.0/enc_avc_push \
    --input ./assets/vid/foreman_qcif.yuv \
    --output ./output/enc_avc_push/foreman_qcif.h264 \
    --frame 176x144 \
    --rate 30 \
    --color yuv420
```

```powershell
# Windows
mkdir -Force -Path ./output/enc_avc_push

./bin/x64/net10.0/enc_avc_push.exe `
    --input assets\vid\foreman_qcif.yuv `
    --output output\enc_avc_push\foreman_qcif.h264 `
    --frame 176x144 `
    --rate 30 `
    --color yuv420
```

On ARM platforms use the arm64 build:

```sh
# Linux and macOS (arm64) 
mkdir -p ./output/enc_avc_push

./bin/arm64/net10.0/enc_avc_push \
    --input ./assets/vid/foreman_qcif.yuv \
    --output ./output/enc_avc_push/foreman_qcif.h264 \
    --frame 176x144 \
    --rate 30 \
    --color yuv420
```

```powershell
# Windows (arm64)
mkdir -Force -Path ./output/enc_avc_push

./bin/arm64/net10.0/enc_avc_push.exe `
    --input assets\vid\foreman_qcif.yuv `
    --output output\enc_avc_push\foreman_qcif.h264 `
    --frame 176x144 `
    --rate 30 `
    --color yuv420
```
