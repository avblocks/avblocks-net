## dec_avc_file

How to use the `Transcoder.Run` method to decode AVC / H.264 compressed file to YUV uncompressed file.   
   

### Command Line

```sh
dec_avc_file --input <file.h264> --output <file.yuv>
```

###	Examples

List options:

```sh
./bin/net10.0/dec_avc_file --help
dec_avc_file 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input     input AVC / H.264 file

  -o, --output    output YUV file

  --help          Display this help screen.
```

Decode the input file `./assets/vid/foreman_qcif.h264` into output file `./output/dec_avc_file/foreman_qcif.yuv`:

```sh
# Linux and macOS 
mkdir -p ./output/dec_avc_file

./bin/net10.0/dec_avc_file \
    --input ./assets/vid/foreman_qcif.h264 \
    --output ./output/dec_avc_file/foreman_qcif.yuv
```

```powershell
# Windows
mkdir -Force -Path ./output/dec_avc_file

./bin/net10.0/dec_avc_file `
    --input ./assets/vid/foreman_qcif.h264 `
    --output ./output/dec_avc_file/foreman_qcif.yuv `
    --frame 176x144 --rate 30 --color yuv420 
```
