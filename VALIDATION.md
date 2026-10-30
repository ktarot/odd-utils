# ODD Utils first-build validation

## Automated verification
- GitHub Actions: publish ODD_Utils.csproj as self-contained Windows x64 single-file EXE.
- Verify artifact contains ODD_Utils.exe.

## Manual Windows smoke test (not yet performed)
- Application opens in 360×640 default size; no clipped controls at 100%, 125%, 150% DPI.
- Opening/closing the music drawer keeps all core copy buttons accessible.
- ODD tray Open/Close works on supported drives.
- Saving to a new or existing folder retains original copy behavior.
- MP3 and WAV play/pause/stop and display LED progress.
- LRC sidecar lyrics display when available.
- Audio CD track playback is tested on the target optical drive; note legacy MCI compatibility.
- Copy and playback are mutually exclusive.
- Light/Dark mode switches correctly.

Hardware/codec-dependent outcomes must be checked on the user Windows PC.
