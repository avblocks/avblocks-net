## enc_opus_file

How to encode WAV file to Opus OGG file.

### Command Line

```sh
enc_opus_file --input <wav file> --output <opus file>
```

### Examples

List options:

```sh
./bin/net10.0/enc_opus_file --help
enc_opus_file 1.0.0.0

  -i, --input     input WAV file

  -o, --output    output Opus file

  --help          Display this help screen.

  --version       Display version information.
```

Encode the input file `./assets/aud/equinox-48KHz.wav` into output file `./output/enc_opus_file/equinox-48KHz.opus`:

```sh
# Linux and macOS 
mkdir -p ./output/enc_opus_file

./bin/net10.0/enc_opus_file \
  --input ./assets/aud/equinox-48KHz.wav \
  --output ./output/enc_opus_file/equinox-48KHz.opus
```

```powershell
# Windows
mkdir -Force -Path ./output/enc_opus_file

./bin/net10.0/enc_opus_file `
  --input ./assets/aud/equinox-48KHz.wav `
  --output ./output/enc_opus_file/equinox-48KHz.opus
```
