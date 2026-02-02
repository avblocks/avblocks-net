## dec_vorbis_file

Decode Vorbis OGG file and save output to WAV file.

### Command Line

```sh
dec_vorbis_file --input <ogg file> --output <wav file>
```

### Examples

List options:

```sh
./bin/net10.0/dec_vorbis_file --help
dec_vorbis_file 1.0.0.0

  -i, --input     input OGG file

  -o, --output    output WAV file

  --help          Display this help screen.

  --version       Display version information.
```

Decode the input file `./assets/aud/Hydrate-Kenny_Beltrey.ogg` into output file `./output/dec_vorbis_file/Hydrate-Kenny_Beltrey.wav`:

```sh
# Linux and macOS 
mkdir -p ./output/dec_vorbis_file

./bin/net10.0/dec_vorbis_file \
  --input ./assets/aud/Hydrate-Kenny_Beltrey.ogg \
  --output ./output/dec_vorbis_file/Hydrate-Kenny_Beltrey.wav
```

```powershell
# Windows
mkdir -Force -Path ./output/dec_vorbis_file

./bin/net10.0/dec_vorbis_file `
  --input ./assets/aud/Hydrate-Kenny_Beltrey.ogg `
  --output ./output/dec_vorbis_file/Hydrate-Kenny_Beltrey.wav
```
