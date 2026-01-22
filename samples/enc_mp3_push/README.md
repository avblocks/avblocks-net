## enc_mp3_push

Encode WAV file to MP3 file using `Transcoder.Push`.

### Command Line

```sh
enc_mp3_push --input <wav file> --output <mp3 file>
```

###	Examples

List options:

```sh
./bin/net10.0/enc_mp3_push --help
enc_mp3_push 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input     input WAV file

  -o, --output    output MP3 file

  --help          Display this help screen.

  --version       Display version information.
```

Encode the input file `./assets/aud/equinox-48KHz.wav` into output file `./output/enc_mp3_push/equinox-48KHz.mp3`:

```sh
# Linux and macOS 
mkdir -p ./output/enc_mp3_push

./bin/net10.0/enc_mp3_push \
    --input ./assets/aud/equinox-48KHz.wav \
    --output ./output/enc_mp3_push/equinox-48KHz.mp3
```

```powershell
# Windows
mkdir -Force -Path ./output/enc_mp3_push

./bin/net10.0/enc_mp3_push `
    --input ./assets/aud/equinox-48KHz.wav `
    --output ./output/enc_mp3_push/equinox-48KHz.mp3
```
