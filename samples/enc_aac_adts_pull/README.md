## enc_aac_adts_pull

Encode WAV file to AAC file in Audio Data Transport Stream (ADTS) format using `Transcoder.Pull`.

### Command Line

```sh
enc_aac_adts_pull --input <wav file> --output <aac file>
```

###	Examples

List options:

```sh
./bin/x64/net10.0/enc_aac_adts_pull --help
enc_aac_adts_pull 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input     input WAV file

  -o, --output    output AAC file

  --help          Display this help screen.

  --version       Display version information.
```

Encode the input file `./assets/aud/equinox-48KHz.wav` into output file `./output/enc_aac_adts_pull/equinox-48KHz.adts.aac`:

```sh
# Linux and macOS 
mkdir -p ./output/enc_aac_adts_pull

./bin/x64/net10.0/enc_aac_adts_pull \
    --input ./assets/aud/equinox-48KHz.wav \
    --output ./output/enc_aac_adts_pull/equinox-48KHz.adts.aac
```

```powershell
# Windows
mkdir -Force -Path ./output/enc_aac_adts_pull

./bin/x64/net10.0/enc_aac_adts_pull `
    --input ./assets/aud/equinox-48KHz.wav `
    --output ./output/enc_aac_adts_pull/equinox-48KHz.adts.aac
```

On ARM platforms use the arm64 build:

```sh
# Linux and macOS (arm64) 
mkdir -p ./output/enc_aac_adts_pull

./bin/arm64/net10.0/enc_aac_adts_pull \
    --input ./assets/aud/equinox-48KHz.wav \
    --output ./output/enc_aac_adts_pull/equinox-48KHz.adts.aac
```

```powershell
# Windows (arm64)
mkdir -Force -Path ./output/enc_aac_adts_pull

./bin/arm64/net10.0/enc_aac_adts_pull `
    --input ./assets/aud/equinox-48KHz.wav `
    --output ./output/enc_aac_adts_pull/equinox-48KHz.adts.aac
```
