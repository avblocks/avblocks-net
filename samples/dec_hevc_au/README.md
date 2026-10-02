## dec_hevc_au

How to use the `Transcoder.Push` method to decode a sequence of H.265 Access Units to YUV uncompressed file.   
   

### Command Line

```sh
dec_hevc_au --input <directory> --output <file.yuv> [--color <COLOR>]
```

###	Examples

List options:

```sh
./bin/x64/net10.0/dec_hevc_au --help
```

List supported color formats:

```sh
./bin/x64/net10.0/dec_hevc_au --colors
```

Decode the H.265 Access Units from `./assets/vid/foreman_qcif.h265.au/` into output file `./output/dec_hevc_au/foreman_qcif.yuv`:

```sh
# Linux and macOS 
mkdir -p ./output/dec_hevc_au

./bin/x64/net10.0/dec_hevc_au \
    --input ./assets/vid/foreman_qcif.h265.au \
    --output ./output/dec_hevc_au/foreman_qcif.yuv \
    --color yuv420
```

```powershell
# Windows
mkdir -Force -Path ./output/dec_hevc_au

./bin/x64/net10.0/dec_hevc_au `
    --input ./assets/vid/foreman_qcif.h265.au `
    --output ./output/dec_hevc_au/foreman_qcif.yuv `
    --color yuv420
```

On ARM platforms use the arm64 build:

```sh
# Linux and macOS (arm64) 
mkdir -p ./output/dec_hevc_au

./bin/arm64/net10.0/dec_hevc_au \
    --input ./assets/vid/foreman_qcif.h265.au \
    --output ./output/dec_hevc_au/foreman_qcif.yuv \
    --color yuv420
```

```powershell
# Windows (arm64)
mkdir -Force -Path ./output/dec_hevc_au

./bin/arm64/net10.0/dec_hevc_au `
    --input ./assets/vid/foreman_qcif.h265.au `
    --output ./output/dec_hevc_au/foreman_qcif.yuv `
    --color yuv420
```
