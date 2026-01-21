## enc_aac_adts_file

How to encode WAV file to AAC file in Audio Data Transport Stream (ADTS) format.

### Command Line

```sh
enc_aac_adts_file --input <wav file> --output <aac file>
```

###	Examples

List options:

```sh
./bin/net10.0/enc_aac_adts_file --help
enc_aac_adts_file 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input     input WAV file

  -o, --output    output AAC file

  --help          Display this help screen.

  --version       Display version information.
```

Encode the input file `./assets/aud/equinox-48KHz.wav` into output file `./output/enc_aac_adts_file/equinox-48KHz.adts.aac`:

```sh
# Linux and macOS 
mkdir -p ./output/enc_aac_adts_file

./bin/net10.0/enc_aac_adts_file \
    --input ./assets/aud/equinox-48KHz.wav \
    --output ./output/enc_aac_adts_file/equinox-48KHz.adts.aac
```

```powershell
# Windows
mkdir -Force -Path ./output/enc_aac_adts_file

./bin/net10.0/enc_aac_adts_file `
    --input ./assets/aud/equinox-48KHz.wav `
    --output ./output/enc_aac_adts_file/equinox-48KHz.adts.aac
```