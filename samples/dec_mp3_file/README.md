## dec_mp3_file

Decode MP3 file and save output to WAV file.

### Command Line

```sh
dec_mp3_file --input <mp3 file> --output <wav file>
```

###	Examples

List options:

```sh
./bin/net10.0/dec_mp3_file --help
dec_mp3_file 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input     input MP3 file

  -o, --output    output WAV file

  --help          Display this help screen.

  --version       Display version information.
```

Decode the input file `./assets/aud/Hydrate-Kenny_Beltrey.mp3` into output file `./output/dec_mp3_file/Hydrate-Kenny_Beltrey.wav`:

```sh
# Linux and macOS 
mkdir -p ./output/dec_mp3_file

./bin/net10.0/dec_mp3_file \
    --input ./assets/aud/Hydrate-Kenny_Beltrey.mp3 \
    --output ./output/dec_mp3_file/Hydrate-Kenny_Beltrey.wav
```

```powershell
# Windows
mkdir -Force -Path ./output/dec_mp3_file

./bin/net10.0/dec_mp3_file `
    --input ./assets/aud/Hydrate-Kenny_Beltrey.mp3 `
    --output ./output/dec_mp3_file/Hydrate-Kenny_Beltrey.wav
```
