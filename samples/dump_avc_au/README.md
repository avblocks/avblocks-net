## dump_avc_au

How to split an H.264/AVC elementary stream into Access Units and dump each Access Unit to a separate file, while also parsing and printing the NAL units within each Access Unit.

### Command Line

```sh
dump_avc_au --input <h264-file> --output <folder>
```

###	Examples

List options:

```sh
./bin/x64/net10.0/dump_avc_au --help
```

Parse the H.264 file `./assets/vid/foreman_qcif.h264` and dump Access Units to `./output/dump_avc_au/`:

```sh
# Linux and macOS 
./bin/x64/net10.0/dump_avc_au \
    --input ./assets/vid/foreman_qcif.h264 \
    --output ./output/dump_avc_au
```

```powershell
# Windows
./bin/x64/net10.0/dump_avc_au `
    --input ./assets/vid/foreman_qcif.h264 `
    --output ./output/dump_avc_au
```

On ARM platforms use the arm64 build:

```sh
# Linux and macOS (arm64) 
./bin/arm64/net10.0/dump_avc_au \
    --input ./assets/vid/foreman_qcif.h264 \
    --output ./output/dump_avc_au
```

```powershell
# Windows (arm64)
./bin/arm64/net10.0/dump_avc_au `
    --input ./assets/vid/foreman_qcif.h264 `
    --output ./output/dump_avc_au
```
