## enc_g711_ulaw_file

Encode WAV file to G.711 μ-law WAV file.

### Command Line

```sh
enc_g711_ulaw_file --input <wav file> --output <g711 ulaw wav file>
```

###	Examples

List options:

```sh
./bin/net10.0/enc_g711_ulaw_file --help
enc_g711_ulaw_file 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input     input WAV file

  -o, --output    output G.711 μ-law WAV file

  --help          Display this help screen.

  --version       Display version information.
```

Encode the input file `./assets/aud/express-dictate_8000_s16_1ch_pcm.wav` into output file `./output/enc_g711_ulaw_file/express-dictate_g711_ulaw.wav`:

```sh
# Linux and macOS 
mkdir -p ./output/enc_g711_ulaw_file

./bin/net10.0/enc_g711_ulaw_file \
    --input ./assets/aud/express-dictate_8000_s16_1ch_pcm.wav \
    --output ./output/enc_g711_ulaw_file/express-dictate_g711_ulaw.wav
```

```powershell
# Windows
mkdir -Force -Path ./output/enc_g711_ulaw_file

./bin/net10.0/enc_g711_ulaw_file `
    --input ./assets/aud/express-dictate_8000_s16_1ch_pcm.wav `
    --output ./output/enc_g711_ulaw_file/express-dictate_g711_ulaw.wav
```
