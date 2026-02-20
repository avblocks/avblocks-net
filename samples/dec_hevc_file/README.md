## dec_hevc_file

How to use the `Transcoder.Run` method to decode an H.265 / HEVC file to YUV uncompressed file.   
   

### Command Line

```sh
dec_hevc_file --input <file.h265> --output <file.yuv>
```

###	Examples

List options:

```sh
./bin/net10.0/dec_hevc_file --help
```

Decode the H.265 file `./assets/vid/foreman_qcif.h265` to YUV output:

```sh
# Linux and macOS 
mkdir -p ./output/dec_hevc_file

./bin/net10.0/dec_hevc_file \
    --input ./assets/vid/foreman_qcif.h265 \
    --output ./output/dec_hevc_file/foreman_qcif.yuv
```

```powershell
# Windows
mkdir -Force -Path ./output/dec_hevc_file

./bin/net10.0/dec_hevc_file `
    --input ./assets/vid/foreman_qcif.h265 `
    --output ./output/dec_hevc_file/foreman_qcif.yuv
```
