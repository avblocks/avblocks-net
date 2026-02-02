## mux_webm_file

Multiplex two single-stream WebM files containing Vorbis (audio) and VP8 (video) streams into a WebM (container) file.

### Command Line

```sh
mux_webm_file --audio <Vorbis_file>.webm --video <VP8_file>.webm --output <output>.webm
```

### Examples

List options:

```sh
./bin/net10.0/mux_webm_file --help
mux_webm_file 1.0.0.0

  -a, --audio     input Vorbis/WebM files. Could be a list of files.

  -v, --video     input VP8/WebM files. Could be a list of files.

  -o, --output    output WebM file

  --help          Display this help screen.

  --version       Display version information.
```

Mux the WebM files `big-buck-bunny_trailer_vp8_vorbis.aud.webm` and `big-buck-bunny_trailer_vp8_vorbis.vid.webm` into the WebM file `big-buck-bunny_trailer.webm`:

```sh
# Linux and macOS 
mkdir -p ./output/mux_webm_file

./bin/net10.0/mux_webm_file \
    --audio ./assets/aud/big-buck-bunny_trailer_vp8_vorbis.aud.webm \
    --video ./assets/vid/big-buck-bunny_trailer_vp8_vorbis.vid.webm \
    --output ./output/mux_webm_file/big-buck-bunny_trailer.webm
```

```powershell
# Windows
mkdir -Force -Path ./output/mux_webm_file

./bin/net10.0/mux_webm_file `
    --audio ./assets/aud/big-buck-bunny_trailer_vp8_vorbis.aud.webm `
    --video ./assets/vid/big-buck-bunny_trailer_vp8_vorbis.vid.webm `
    --output ./output/mux_webm_file/big-buck-bunny_trailer.webm
```
