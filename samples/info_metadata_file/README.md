## info_metadata_file

How to use the MediaInfo and Metadata APIs to extract metadata information from a media file.   

### Command Line

```sh
info_metadata_file --input <any_media_file>
```

###	Examples

List options:

```sh
./bin/net10.0/info_metadata_file --help
info_metadata_file 1.0.0.0
Copyright (C) 2023 Primo Software

  -i, --input    input file

  --help         Display this help screen.

  --version      Display version information.
```

Extract the metadata from the `./assets/aud/Hydrate-Kenny_Beltrey.ogg` song:

```sh
# Linux and macOS 
./bin/net10.0/info_metadata_file \
    --input ./assets/aud/Hydrate-Kenny_Beltrey.ogg
```

```powershell
# Windows
./bin/net10.0/info_metadata_file `
    --input ./assets/aud/Hydrate-Kenny_Beltrey.ogg
```
