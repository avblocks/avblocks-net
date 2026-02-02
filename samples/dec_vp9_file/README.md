## dec_vp9_file

Decode VP9/IVF file and save output to raw YUV file.

### Command Line

```sh
dec_vp9_file --input <ivf file> --output <yuv file>
```

### Examples

List options:

```sh
./bin/net10.0/dec_vp9_file --help
dec_vp9_file 1.0.0.0

  -i, --input     input VP9/IVF file

  -o, --output    output YUV file

  --help          Display this help screen.

  --version       Display version information.
```

Decode the input file `./assets/vid/foreman_qcif_vp9.ivf` into output file `./output/dec_vp9_file/foreman_qcif.yuv`:

```sh
# Linux and macOS 
mkdir -p ./output/dec_vp9_file

./bin/net10.0/dec_vp9_file \
  --input ./assets/vid/foreman_qcif_vp9.ivf \
  --output ./output/dec_vp9_file/foreman_qcif.yuv
```

```powershell
# Windows
mkdir -Force -Path ./output/dec_vp9_file

./bin/net10.0/dec_vp9_file `
  --input ./assets/vid/foreman_qcif_vp9.ivf `
  --output ./output/dec_vp9_file/foreman_qcif.yuv
```
