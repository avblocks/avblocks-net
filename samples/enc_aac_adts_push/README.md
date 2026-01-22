## enc_aac_adts_push

Encode WAV file to AAC file in Audio Data Transport Stream (ADTS) format using `Transcoder.Push`.

### Command Line

```sh
enc_aac_adts_push --input <wav file> --output <aac file>
```

###	Examples

List options:

```sh
./bin/net10.0/enc_aac_adts_push --help
enc_aac_adts_push 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input     input WAV file

  -o, --output    output AAC file

  --help          Display this help screen.

  --version       Display version information.
```

Encode the input file `./assets/aud/equinox-48KHz.wav` into output file `./output/enc_aac_adts_push/equinox-48KHz.adts.aac`:

```sh
# Linux and macOS 
mkdir -p ./output/enc_aac_adts_push

./bin/net10.0/enc_aac_adts_push \
    --input ./assets/aud/equinox-48KHz.wav \
    --output ./output/enc_aac_adts_push/equinox-48KHz.adts.aac
```

```powershell
# Windows
mkdir -Force -Path ./output/enc_aac_adts_push

./bin/net10.0/enc_aac_adts_push `
    --input ./assets/aud/equinox-48KHz.wav `
    --output ./output/enc_aac_adts_push/equinox-48KHz.adts.aac
```
