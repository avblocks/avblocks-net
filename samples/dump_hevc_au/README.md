## dump_hevc_au

How to split an H.265/HEVC elementary stream into Access Units and dump each Access Unit to a separate file, while also parsing and printing the NAL units within each Access Unit.

### Command Line

```sh
dump_hevc_au --input <h265-file> --output <folder>
```

###	Examples

List options:

```sh
./bin/x64/net10.0/dump_hevc_au --help
```

Parse the H.265 file `./assets/vid/foreman_qcif.h265` and dump Access Units to `./output/dump_hevc_au/`:

```sh
# Linux and macOS 
./bin/x64/net10.0/dump_hevc_au \
    --input ./assets/vid/foreman_qcif.h265 \
    --output ./output/dump_hevc_au
```

```powershell
# Windows
./bin/x64/net10.0/dump_hevc_au `
    --input ./assets/vid/foreman_qcif.h265 `
    --output ./output/dump_hevc_au
```

On ARM platforms use the arm64 build:

```sh
# Linux and macOS (arm64) 
./bin/arm64/net10.0/dump_hevc_au \
    --input ./assets/vid/foreman_qcif.h265 \
    --output ./output/dump_hevc_au
```

```powershell
# Windows (arm64)
./bin/arm64/net10.0/dump_hevc_au `
    --input ./assets/vid/foreman_qcif.h265 `
    --output ./output/dump_hevc_au
```
