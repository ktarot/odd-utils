# ODD Utils v2.2 — three-layer player validation

## Layer 0: Copier
- Default Windows size 360 × 640 (title bar included).
- Drive letter / optical-drive family / disc-ready status
- Existing destination selection, new folder creation, file-copy start/stop, overwrite collision detection
- Tray Open/Close, LED Dot Matrix, Light/Dark, status log
- Opening the full player MUST NOT resize or move any copier controls.

## Layer 1: Mini player
- Music button is visible in a fixed-height row.
- With audio active, track name and mini Play/Pause button show.
- Collapse the full player: music continues playing.
- Mini Play/Pause and LED seek MUST NOT reopen the full layer.

## Layer 2: Full player
- Floating overlay appears over copier controls, above Exit/Theme row.
- File open / CD play, previous / play-pause / next / stop.
- Switch content between timed LRC/TXT lyrics and track list; only content area scrolls.
- Full player collapses to mini; copier layout remains unchanged.
- Single shared LED meter shows audio progress during audio playback.

## Mixed-mode CD
- MCI enumerates each physical CD track; non-audio tracks are skipped.
- Remaining audio tracks use original CD numbers (e.g., Track 02 when Track 01 is data).
- If there are no audio tracks, show a clear 'no playable audio tracks' message.
- MCI CD playback is legacy and is not supported by all drives/Windows configurations.

## Hardware / UI smoke test
1. Check 360×640 initial window at 100%, 125%, and 150% display scaling.
2. Confirm no clipped text or overlapping buttons in base, mini, and full layers.
3. Test MP3/WAV playback, LED progress, pause/resume, navigation, then collapse to mini.
4. Test an LRC file and lyrics/track-list toggle.
5. Test normal audio CD, mixed-mode CD (if available), and data-only CD.
6. Verify CD play stopped before the tray opens; verify copying is blocked while audio is active.
7. Verify copying a data DVD/CD to a new and existing folder still works.
8. Check both Light and Dark themes.

GitHub Actions verifies that source compiles and the self-contained EXE is produced. It does NOT confirm real optical drive, display scaling, or codec compatibility.
