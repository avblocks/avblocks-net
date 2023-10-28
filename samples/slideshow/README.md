## slideshow

Shows how to make a video clip from a sequence of images. The input is a series of JPEG images. The output is configured with an AVBlocks preset.

### Command Line

```sh
slideshow --input <directory> --output <filename_without_extension> --preset <preset_id>
```

###	Examples

List options:

```sh
./bin/net60/slideshow --help
slideshow 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input        Input directory containing images for the slideshow.

  -o, --output       Output filename (without extension). The extension is added based on the preset.

  -p, --preset       output preset id.

  --help             Display this help screen.

  --version          Display version information.
```

Create an MP4 / H.264 clip from a sequence of images in the `./assets/img` folder using the `mp4.h264.aac` preset:

```sh
# Linux and macOS 
mkdir -p ./output/slideshow

./bin/net60/slideshow \
    --input ./assets/img \
    --output ./output/slideshow/cube \
    --preset mp4.h264.aac
```

```powershell
# Windows
mkdir -Force -Path ./output/slideshow

./bin/net60/slideshow `
    --input ./assets/img `
    --output ./output/slideshow/cube `
    --preset mp4.h264.aac
```

List supported presets for input:

```sh
./bin/net60/slideshow --presets

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
