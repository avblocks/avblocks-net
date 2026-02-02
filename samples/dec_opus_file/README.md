## dec_opus_file

Decode Opus OGG file and save output to WAV file.

### Command Line

```sh
dec_opus_file --input <opus file> --output <wav file>
```

### Examples

List options:

```sh
./bin/net10.0/dec_opus_file --help
dec_opus_file 1.0.0.0

  -i, --input     input Opus file

  -o, --output    output WAV file

  --help          Display this help screen.

  --version       Display version information.
```

Decode the input file `./assets/aud/Everybody-TBB.opus` into output file `./output/dec_opus_file/Everybody-TBB.wav`:

```sh
# Linux and macOS 
mkdir -p ./output/dec_opus_file

./bin/net10.0/dec_opus_file \
  --input ./assets/aud/Everybody-TBB.opus \
  --output ./output/dec_opus_file/Everybody-TBB.wav
```

```powershell
# Windows
mkdir -Force -Path ./output/dec_opus_file

./bin/net10.0/dec_opus_file `
  --input ./assets/aud/Everybody-TBB.opus `
  --output ./output/dec_opus_file/Everybody-TBB.wav
```
