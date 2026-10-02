## enc_mp3_pull

Encode WAV file to MP3 file using `Transcoder.Pull`.

### Command Line

```sh
enc_mp3_pull --input <wav file> --output <mp3 file>
```

###	Examples

List options:

```sh
./bin/x64/net10.0/enc_mp3_pull --help
enc_mp3_pull 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input     input WAV file

  -o, --output    output MP3 file

  --help          Display this help screen.

  --version       Display version information.
```

Encode the input file `./assets/aud/equinox-48KHz.wav` into output file `./output/enc_mp3_pull/equinox-48KHz.mp3`:

```sh
# Linux and macOS 
mkdir -p ./output/enc_mp3_pull

./bin/x64/net10.0/enc_mp3_pull \
    --input ./assets/aud/equinox-48KHz.wav \
    --output ./output/enc_mp3_pull/equinox-48KHz.mp3
```

```powershell
# Windows
mkdir -Force -Path ./output/enc_mp3_pull

./bin/x64/net10.0/enc_mp3_pull `
    --input ./assets/aud/equinox-48KHz.wav `
    --output ./output/enc_mp3_pull/equinox-48KHz.mp3
```

On ARM platforms use the arm64 build:

```sh
# Linux and macOS (arm64) 
mkdir -p ./output/enc_mp3_pull

./bin/arm64/net10.0/enc_mp3_pull \
    --input ./assets/aud/equinox-48KHz.wav \
    --output ./output/enc_mp3_pull/equinox-48KHz.mp3
```

```powershell
# Windows (arm64)
mkdir -Force -Path ./output/enc_mp3_pull

./bin/arm64/net10.0/enc_mp3_pull `
    --input ./assets/aud/equinox-48KHz.wav `
    --output ./output/enc_mp3_pull/equinox-48KHz.mp3
```
