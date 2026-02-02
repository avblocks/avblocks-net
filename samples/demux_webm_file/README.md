## demux_webm_file

Demux a WebM file into separate audio and video WebM files.

### Command Line

```sh
demux_webm_file --input <webm file> --output <output file name without extension>
```

### Examples

List options:

```sh
./bin/net10.0/demux_webm_file --help
demux_webm_file 1.0.0.0

  -i, --input     Input webm file

  -o, --output    Output webm filename (without extension)

  --help          Display this help screen.

  --version       Display version information.
```

Demux the input file `./assets/mov/big-buck-bunny_trailer_vp8_vorbis.webm` into separate audio and video files:

```sh
# Linux and macOS 
mkdir -p ./output/demux_webm_file

./bin/net10.0/demux_webm_file \
  --input ./assets/mov/big-buck-bunny_trailer_vp8_vorbis.webm \
  --output ./output/demux_webm_file/big-buck-bunny_trailer_vp8_vorbis
```

```powershell
# Windows
mkdir -Force -Path ./output/demux_webm_file

./bin/net10.0/demux_webm_file `
  --input ./assets/mov/big-buck-bunny_trailer_vp8_vorbis.webm `
  --output ./output/demux_webm_file/big-buck-bunny_trailer_vp8_vorbis
```

This will produce:
- `big-buck-bunny_trailer_vp8_vorbis.aud.webm` (audio only)
- `big-buck-bunny_trailer_vp8_vorbis.vid.webm` (video only)
