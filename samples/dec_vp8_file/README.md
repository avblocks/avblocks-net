## dec_vp8_file

Decode VP8/IVF file and save output to raw YUV file.

### Command Line

```sh
dec_vp8_file --input <ivf file> --output <yuv file>
```

### Examples

List options:

```sh
./bin/net10.0/dec_vp8_file --help
dec_vp8_file 1.0.0.0

  -i, --input     input VP8/IVF file

  -o, --output    output YUV file

  --help          Display this help screen.

  --version       Display version information.
```

Decode the input file `./assets/vid/foreman_qcif_vp8.ivf` into output file `./output/dec_vp8_file/foreman_qcif.yuv`:

```sh
# Linux and macOS 
mkdir -p ./output/dec_vp8_file

./bin/net10.0/dec_vp8_file \
  --input ./assets/vid/foreman_qcif_vp8.ivf \
  --output ./output/dec_vp8_file/foreman_qcif.yuv
```

```powershell
# Windows
mkdir -Force -Path ./output/dec_vp8_file

./bin/net10.0/dec_vp8_file `
  --input ./assets/vid/foreman_qcif_vp8.ivf `
  --output ./output/dec_vp8_file/foreman_qcif.yuv
```
