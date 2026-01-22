## dec_aac_adts_pull

Decode AAC file in Audio Data Transport Stream (ADTS) format using `Transcoder.Pull` and save output to WAV file.

### Command Line

```sh
dec_aac_adts_pull --input <aac file> --output <wav file>
```

###	Examples

List options:

```sh
./bin/net10.0/dec_aac_adts_pull --help
dec_aac_adts_pull 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input     input AAC file

  -o, --output    output WAV file

  --help          Display this help screen.

  --version       Display version information.
```

Decode the input file `./assets/aud/Hydrate-Kenny_Beltrey.adts.aac` into output file `./output/dec_aac_adts_pull/Hydrate-Kenny_Beltrey.wav`:

```sh
# Linux and macOS 
mkdir -p ./output/dec_aac_adts_pull

./bin/net10.0/dec_aac_adts_pull \
    --input ./assets/aud/Hydrate-Kenny_Beltrey.adts.aac \
    --output ./output/dec_aac_adts_pull/Hydrate-Kenny_Beltrey.wav
```

```powershell
# Windows
mkdir -Force -Path ./output/dec_aac_adts_pull

./bin/net10.0/dec_aac_adts_pull `
    --input ./assets/aud/Hydrate-Kenny_Beltrey.adts.aac `
    --output ./output/dec_aac_adts_pull/Hydrate-Kenny_Beltrey.wav
```
