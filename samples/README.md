# Samples - .NET

## Media Info

### info_metadata_file

Extract metadata information from a media file.   

See [info_metadata_file](./info_metadata_file) for details.

### info_stream_file

Extract audio / video stream information from a media file.   

See [info_stream_file](./info_stream_file) for details.

---

## Demuxing

### MP4 

> MPEG-4 Part 14 / MPEG-4 Part 1

#### demux_mp4_file

Extract the first audio and video elementary stream from an MP4 container and save each stream into a separate MP4 file.

See [demux_mp4_file](./demux_mp4_file) for details.

### WebM

> WebM Container Format

#### demux_webm_file

Extract the audio and video elementary streams from a WebM container and save each stream into a separate file.

See [demux_webm_file](./demux_webm_file) for details.

### AVC / H.264

> Advanced Video Coding

#### dump_avc_au

Split an H.264/AVC elementary stream into Access Units and dump each Access Unit to a separate file, while also parsing and printing the NAL units within each Access Unit.

See [dump_avc_au](./dump_avc_au) for details.

### HEVC / H.265

> High Efficiency Video Coding

#### dump_hevc_au

Split an H.265/HEVC elementary stream into Access Units and dump each Access Unit to a separate file, while also parsing and printing the NAL units within each Access Unit.

See [dump_hevc_au](./dump_hevc_au) for details.

---

## Muxing

### MP4 

> MPEG-4 Part 1

#### mux_mp4_file

Multiplex two single-stream MP4 files containing AAC (audio) and H.264 (video) streams into an MP4 (container) file.

See [mux_mp4_file](./mux_mp4_file) for details.

### WebM

> WebM Container Format

#### mux_webm_file

Multiplex Vorbis/OGG (audio) and VP8/IVF (video) files into a WebM (container) file.

See [mux_webm_file](./mux_webm_file) for details.

---

## Decoding

### AAC 

> Advanced Audio Coding

#### dec_aac_adts_file

Decode AAC file in Audio Data Transport Stream (ADTS) format and save the output to WAV file.

See [dec_aac_adts_file](./dec_aac_adts_file) for details.

#### dec_aac_adts_pull

Decode AAC file in Audio Data Transport Stream (ADTS) format using `Transcoder.Pull` and save the output to WAV file.

See [dec_aac_adts_pull](./dec_aac_adts_pull) for details.

### G.711

> ITU-T G.711 Audio Codec

#### dec_g711_alaw_file

Decode a WAV file containing G.711 A-law audio to a WAV file with PCM audio.

See [dec_g711_alaw_file](./dec_g711_alaw_file) for details.

#### dec_g711_ulaw_file

Decode a WAV file containing G.711 μ-law audio to a WAV file with PCM audio.

See [dec_g711_ulaw_file](./dec_g711_ulaw_file) for details.

### MP3

> MPEG-1/2 Audio Layer III

#### dec_mp3_file

Decode an MP3 file to a WAV file.

See [dec_mp3_file](./dec_mp3_file) for details.

### Opus

> Opus Interactive Audio Codec

#### dec_opus_file

Decode an Opus/OGG file to a WAV file.

See [dec_opus_file](./dec_opus_file) for details.

### Vorbis

> Ogg Vorbis Audio Codec

#### dec_vorbis_file

Decode a Vorbis/OGG file to a WAV file.

See [dec_vorbis_file](./dec_vorbis_file) for details.

### AVC / H.264 

> Advanced Video Coding

#### dec_avc_au

Decode AVC / H.264 stream. The sample uses sequence of files to simulate a stream of H.264 Access Units (AUs) and a Transcoder object to decode the AUs to raw YUV video frames.    

See [dec_avc_au](./dec_avc_au) for details.

#### dec_avc_file

Decode a compressed AVC / H.264 file to raw uncompressed YUV video file.       

See [dec_avc_file](./dec_avc_file) for details.

#### dec_avc_pull

Decode AVC / H.264 file to YUV file using `Transcoder.Pull`.

See [dec_avc_pull](./dec_avc_pull) for details.

### HEVC / H.265 

> High Efficiency Video Coding

#### dec_hevc_au

Decode HEVC / H.265 stream. The sample uses sequence of files to simulate a stream of H.265 Access Units (AUs) and a Transcoder object to decode the AUs to raw YUV video frames.    

See [dec_hevc_au](./dec_hevc_au) for details.

#### dec_hevc_file

Decode a compressed HEVC / H.265 file to raw uncompressed YUV video file.       

See [dec_hevc_file](./dec_hevc_file) for details.

### VP8

> VP8 Video Codec

#### dec_vp8_file

Decode a VP8/IVF file to a raw YUV video file.

See [dec_vp8_file](./dec_vp8_file) for details.

### VP9

> VP9 Video Codec

#### dec_vp9_file

Decode a VP9/IVF file to a raw YUV video file.

See [dec_vp9_file](./dec_vp9_file) for details.

---

## Encoding

### AAC 

> Advanced Audio Coding

#### enc_aac_adts_file

Encode WAV file to AAC file in Audio Data Transport Stream (ADTS) format.

See [enc_aac_adts_file](./enc_aac_adts_file) for details.

#### enc_aac_adts_pull

Encode WAV file to AAC file in Audio Data Transport Stream (ADTS) format using `Transcoder.Pull`.

See [enc_aac_adts_pull](./enc_aac_adts_pull) for details.

#### enc_aac_adts_push

Encode WAV file to AAC file in Audio Data Transport Stream (ADTS) format using `Transcoder.Push`.

See [enc_aac_adts_push](./enc_aac_adts_push) for details.

### G.711

> ITU-T G.711 Audio Codec

#### enc_g711_alaw_file

Encode a WAV file with PCM audio to a WAV file containing G.711 A-law audio.

See [enc_g711_alaw_file](./enc_g711_alaw_file) for details.

#### enc_g711_ulaw_file

Encode a WAV file with PCM audio to a WAV file containing G.711 μ-law audio.

See [enc_g711_ulaw_file](./enc_g711_ulaw_file) for details.

### AVC / H.264 

> Advanced Video Coding

#### enc_avc_file

Convert a raw YUV video file to a compressed AVC / H.264 video file.  

See [enc_avc_file](./enc_avc_file) for details.

#### enc_avc_pull

Encode a raw YUV video file to a compressed AVC / H.264 video file using `Transcoder.Pull`.  

See [enc_avc_pull](./enc_avc_pull) for details.

### HEVC / H.265 

> High Efficiency Video Coding

#### enc_hevc_file

Convert a raw YUV video file to a compressed HEVC / H.265 video file.  

See [enc_hevc_file](./enc_hevc_file) for details.

#### enc_hevc_pull

Encode a raw YUV video file to a compressed HEVC / H.265 video file using `Transcoder.Pull`.  

See [enc_hevc_pull](./enc_hevc_pull) for details.

### MP3

> MPEG-1/2 Audio Layer III

#### enc_mp3_file

Encode WAV file to MP3 file.

See [enc_mp3_file](./enc_mp3_file) for details.

#### enc_mp3_pull

Encode WAV file to MP3 file using `Transcoder.Pull`.

See [enc_mp3_pull](./enc_mp3_pull) for details.

#### enc_mp3_push

Encode WAV file to MP3 file using `Transcoder.Push`.

See [enc_mp3_push](./enc_mp3_push) for details.

### Opus

> Opus Interactive Audio Codec

#### enc_opus_file

Encode a WAV file to an Opus/OGG file.

See [enc_opus_file](./enc_opus_file) for details.

### Vorbis

> Ogg Vorbis Audio Codec

#### enc_vorbis_file

Encode a WAV file to a Vorbis/OGG file.

See [enc_vorbis_file](./enc_vorbis_file) for details.

### VP8

> VP8 Video Codec

#### enc_vp8_file

Convert a raw YUV video file to a compressed VP8/IVF video file.

See [enc_vp8_file](./enc_vp8_file) for details.

### VP9

> VP9 Video Codec

#### enc_vp9_file

Convert a raw YUV video file to a compressed VP9/IVF video file.

See [enc_vp9_file](./enc_vp9_file) for details.

### Other 

#### enc_preset_file

Convert a raw YUV video file to a compressed video file. The format of the output is configured with an AVBlocks preset.

See [enc_preset_file](./enc_preset_file) for details.

---

## Misc

### re-encode

Take an MP4 input and re-encodes the audio and video streams back into an MP4 output. 

See [re-encode](./re-encode) for details.

### slideshow

Create a video from a sequence of images.

See [slideshow](./slideshow) for details.
