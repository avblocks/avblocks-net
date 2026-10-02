## enc_vorbis_file

How to encode WAV file to Vorbis OGG file.

### Command Line

```sh
enc_vorbis_file --input <wav file> --output <ogg file>
```

### Examples

List options:

```sh
./bin/x64/net10.0/enc_vorbis_file --help
enc_vorbis_file 1.0.0.0

  -i, --input     input WAV file

  -o, --output    output OGG file

  --help          Display this help screen.

  --version       Display version information.
```

Encode the input file `./assets/aud/equinox-48KHz.wav` into output file `./output/enc_vorbis_file/equinox-48KHz.ogg`:

```sh
# Linux and macOS 
mkdir -p ./output/enc_vorbis_file

./bin/x64/net10.0/enc_vorbis_file \
  --input ./assets/aud/equinox-48KHz.wav \
  --output ./output/enc_vorbis_file/equinox-48KHz.ogg
```

```powershell
# Windows
mkdir -Force -Path ./output/enc_vorbis_file

./bin/x64/net10.0/enc_vorbis_file `
  --input ./assets/aud/equinox-48KHz.wav `
  --output ./output/enc_vorbis_file/equinox-48KHz.ogg
```

On ARM platforms use the arm64 build:

```sh
# Linux and macOS (arm64) 
mkdir -p ./output/enc_vorbis_file

./bin/arm64/net10.0/enc_vorbis_file \
  --input ./assets/aud/equinox-48KHz.wav \
  --output ./output/enc_vorbis_file/equinox-48KHz.ogg
```

```powershell
# Windows (arm64)
mkdir -Force -Path ./output/enc_vorbis_file

./bin/arm64/net10.0/enc_vorbis_file `
  --input ./assets/aud/equinox-48KHz.wav `
  --output ./output/enc_vorbis_file/equinox-48KHz.ogg
```
