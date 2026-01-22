## dec_g711_alaw_file

Decode G.711 A-law WAV file to PCM WAV file.

### Command Line

```sh
dec_g711_alaw_file --input <g711 alaw wav file> --output <wav file>
```

###	Examples

List options:

```sh
./bin/net10.0/dec_g711_alaw_file --help
dec_g711_alaw_file 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input     input G.711 A-law WAV file

  -o, --output    output PCM WAV file

  --help          Display this help screen.

  --version       Display version information.
```

Decode the input file `./assets/aud/express-dictate_8000_s8_1ch_alaw.wav` into output file `./output/dec_g711_alaw_file/express-dictate_8000_s16_1ch_pcm.wav`:

```sh
# Linux and macOS 
mkdir -p ./output/dec_g711_alaw_file

./bin/net10.0/dec_g711_alaw_file \
    --input ./assets/aud/express-dictate_8000_s8_1ch_alaw.wav \
    --output ./output/dec_g711_alaw_file/express-dictate_8000_s16_1ch_pcm.wav
```

```powershell
# Windows
mkdir -Force -Path ./output/dec_g711_alaw_file

./bin/net10.0/dec_g711_alaw_file `
    --input ./assets/aud/express-dictate_8000_s8_1ch_alaw.wav `
    --output ./output/dec_g711_alaw_file/express-dictate_8000_s16_1ch_pcm.wav
```
