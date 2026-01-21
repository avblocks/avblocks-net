## dec_avc_pull

How to use the `Transcoder.Pull` method to decode AVC / H.264 compressed file to YUV uncompressed file.   
   

### Command Line

```sh
dec_avc_pull --input <file.h264> --output <file.yuv>
```

###	Examples

List options:

```sh
./bin/net10.0/dec_avc_pull --help
dec_avc_pull 1.0.0.0
Copyright (C) 2025 Primo Software

  -i, --input     input AVC / H.264 file

  -o, --output    output YUV file

  --help          Display this help screen.
```

Decode the input file `./assets/vid/foreman_qcif.h264` into output file `./output/dec_avc_pull/foreman_qcif.yuv`:

```sh
# Linux and macOS 
mkdir -p ./output/dec_avc_pull

./bin/net10.0/dec_avc_pull \
    --input ./assets/vid/foreman_qcif.h264 \
    --output ./output/dec_avc_pull/foreman_qcif.yuv
```

```powershell
# Windows
mkdir -Force -Path ./output/dec_avc_pull

./bin/net10.0/dec_avc_pull `
    --input ./assets/vid/foreman_qcif.h264 `
    --output ./output/dec_avc_pull/foreman_qcif.yuv
```
