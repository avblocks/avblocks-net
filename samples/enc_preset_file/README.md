## enc_yuv_preset_file

Shows how to convert a raw YUV video file and to a compressed video file. The format of the output is configured with an AVBlocks preset.

### Command Line

```sh
enc_preset_file --frame <width>x<height> --rate <fps> --color <COLOR> --input <file> --output <filename_without_extension> [--preset <PRESET>] [--colors] [--presets]
```

###	Examples

List options:

```sh
./bin/net10.0/enc_preset_file --help
enc_preset_file 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input     input YUV file

  -o, --output    output AVC / H.264 file

  -r, --rate      input frame rate

  -f, --frame     input frame size

  -c, --color     input color space. Use --colors to list all supported input color spaces.

  --colors        list supported input color spaces

  -p, --preset    output preset id.

  --presets       list supported input presets

  --help          Display this help screen.

  --version       Display version information.
```

Encode a raw YUV video from `./assets/vid/foreman_qcif.yuv` to a H.264 video in an MP4 container in `./output/enc_preset_file/foreman_qcif.mp4`: 

```sh
# Linux and macOS 
mkdir -p ./output/enc_preset_file

./bin/net10.0/enc_preset_file \
    --input ./assets/vid/foreman_qcif.yuv \
    --frame 176x144 --rate 30 --color yuv420 \
    --output ./output/enc_avc_file/foreman_qcif.h264 \
    --preset ipad.mp4.h264.720p
```

```powershell
# Windows
mkdir -Force -Path ./output/enc_preset_file

./bin/net10.0/enc_preset_file `
    --input ./assets/vid/foreman_qcif.yuv `
    --frame 176x144 --rate 30 --color yuv420 `
    --output ./output/enc_avc_file/foreman_qcif.h264 `
    --preset ipad.mp4.h264.720p
```

List supported color spaces for input:

```sh
./bin/net10.0/enc_preset_file --colors

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

List supported presets for input:

```sh
./bin/net10.0/enc_preset_file --presets

PRESETS
-----------
dvd.ntsc.16x9.mp2              .mpg
dvd.ntsc.16x9.pcm              .mpg
dvd.ntsc.4x3.mp2               .mpg
dvd.ntsc.4x3.pcm               .mpg
dvd.pal.16x9.mp2               .mpg
dvd.pal.4x3.mp2                .mpg
ipad.mp4.h264.576p             .mp4
ipad.mp4.h264.720p             .mp4
ipad.mp4.mpeg4.480p            .mp4
iphone.mp4.h264.480p           .mp4
iphone.mp4.mpeg4.480p          .mp4
ipod.mp4.h264.240p             .mp4
ipod.mp4.mpeg4.240p            .mp4
mp4.h264.aac                   .mp4
wifi.h264.640x480.30p.1200k.aac.96k .ts
wifi.wide.h264.1280x720.30p.4500k.aac.128k .ts
android-phone.mp4.h264.360p    .mp4
android-phone.mp4.h264.720p    .mp4
android-tablet.mp4.h264.720p   .mpg
android-tablet.webm.vp8.720p   .webm
vcd.ntsc                       .mpg
vcd.pal                        .mpg
webm.vp8.vorbis                .webm
```
