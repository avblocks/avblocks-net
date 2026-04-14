## video_crop

Crop a video by removing pixels from the edges.

### Command Line

```sh
video_crop -i <input.mp4> -o <output.mp4> [--crop-left <left>] [--crop-right <right>] [--crop-top <top>] [--crop-bottom <bottom>]
```

### Examples

List options:

```sh
./bin/net10.0/video_crop --help
video_crop 1.0.0.0
Copyright (C) 2026 Primo Software

  -i, --input      MP4 input file.

  -o, --output     MP4 output file.

  --crop-left      Pixels to crop from left.

  --crop-right     Pixels to crop from right.

  --crop-top       Pixels to crop from top.

  --crop-bottom    Pixels to crop from bottom.

  --help           Display this help screen.

  --version        Display version information.
```

Crop `./assets/vid/big_buck_bunny_trailer.vid.mp4` from 16:9 (480x270) to 4:3 (360x270) by removing 60 pixels from each side and save to `./output/video_crop/cropped.mp4`:

```sh
mkdir -p ./output/video_crop

./bin/net10.0/video_crop \
    --input ./assets/vid/big_buck_bunny_trailer.vid.mp4 \
    --output ./output/video_crop/cropped.mp4 \
    --crop-left 60 \
    --crop-right 60
```

- Crop values should ideally be even numbers for proper alignment with video codec block sizes
