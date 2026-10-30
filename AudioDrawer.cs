using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DvdCopier;

/// <summary>
/// An actual overlay, independent of the copier's TableLayoutPanel.
/// The primary controls never resize when this panel opens.
/// </summary>
internal sealed class AudioDrawer : Panel
{
    public readonly Button CdButton;
    public readonly Button FileButton;
    public readonly Button PreviousButton;
    public readonly Button PlayPauseButton;
    public readonly Button NextButton;
    public readonly Button StopButton;
    public readonly Button MinimizeButton;
    public readonly Label TrackTitle;
    public readonly Label TimeDisplay;
    public readonly TextBox Lyrics;
    public readonly ListBox Tracks;

    private readonly Label heading;
    private readonly Button lyricsTab;
    private readonly Button tracksTab;
    private readonly Panel contentHost;
    private readonly TableLayoutPanel root;
    private bool showingLyrics = true;

    public event Action<int>? TrackActivated;

    public AudioDrawer()
    {
        Visible = false;
        BorderStyle = BorderStyle.FixedSingle;
        Padding = new Padding(10);
        Margin = Padding.Empty;

        root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 6,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        foreach (int h in new[] { 34, 40, 36, 46, 34 })
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, h));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Controls.Add(root);

        var titleRow = Columns(82, 18);
        heading = TextLabel("♫  MUSIC PLAYER", 11, true);
        MinimizeButton = FlatButton("⌄", 13);
        titleRow.Controls.Add(heading, 0, 0);
        titleRow.Controls.Add(MinimizeButton, 1, 0);
        root.Controls.Add(titleRow, 0, 0);

        var sourceRow = Columns(50, 50);
        CdButton = FlatButton("CD 재생", 10);
        FileButton = FlatButton("파일 열기", 10);
        sourceRow.Controls.Add(CdButton, 0, 0);
        sourceRow.Controls.Add(FileButton, 1, 0);
        root.Controls.Add(sourceRow, 0, 1);

        var songRow = Columns(62, 38);
        TrackTitle = TextLabel("선택한 음악 없음", 9, true);
        TrackTitle.AutoEllipsis = true;
        TimeDisplay = TextLabel("00:00 / 00:00", 8);
        TimeDisplay.TextAlign = ContentAlignment.MiddleRight;
        songRow.Controls.Add(TrackTitle, 0, 0);
        songRow.Controls.Add(TimeDisplay, 1, 0);
        root.Controls.Add(songRow, 0, 2);

        var transport = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 1
        };
        for (int i = 0; i < 4; i++)
            transport.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        PreviousButton = FlatButton("⏮", 12);
        PlayPauseButton = FlatButton("▶", 15);
        NextButton = FlatButton("⏭", 12);
        StopButton = FlatButton("■", 11);
        transport.Controls.Add(PreviousButton, 0, 0);
        transport.Controls.Add(PlayPauseButton, 1, 0);
        transport.Controls.Add(NextButton, 2, 0);
        transport.Controls.Add(StopButton, 3, 0);
        root.Controls.Add(transport, 0, 3);

        var viewRow = Columns(50, 50);
        lyricsTab = FlatButton("가사", 9);
        tracksTab = FlatButton("트랙 목록", 9);
        viewRow.Controls.Add(lyricsTab, 0, 0);
        viewRow.Controls.Add(tracksTab, 1, 0);
        root.Controls.Add(viewRow, 0, 4);

        contentHost = new Panel { Dock = DockStyle.Fill, Margin = new Padding(3, 4, 3, 2) };
        Lyrics = new TextBox
        {
            Dock = DockStyle.Fill, ReadOnly = true,
            Multiline = true, WordWrap = true,
            BorderStyle = BorderStyle.None,
            ScrollBars = ScrollBars.Vertical,
            Text = "가사 정보 없음"
        };
        Tracks = new ListBox
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
            IntegralHeight = false,
            Visible = false
        };
        contentHost.Controls.Add(Tracks);
        contentHost.Controls.Add(Lyrics);
        root.Controls.Add(contentHost, 0, 5);

        lyricsTab.Click += (_, _) => ShowLyrics(true);
        tracksTab.Click += (_, _) => ShowLyrics(false);
        Tracks.DoubleClick += (_, _) =>
        {
            if (Tracks.SelectedIndex >= 0)
                TrackActivated?.Invoke(Tracks.SelectedIndex);
        };
        ShowLyrics(true);
        UpdatePlaybackControls(false, false, false, false);
    }

    private static TableLayoutPanel Columns(int first, int second)
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1,
            Margin = Padding.Empty, Padding = Padding.Empty
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, first));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, second));
        return panel;
    }

    private static Label TextLabel(string value, float size, bool bold = false)
    {
        return new Label
        {
            Dock = DockStyle.Fill,
            Text = value,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI", size,
                bold ? FontStyle.Bold : FontStyle.Regular)
        };
    }

    private static Button FlatButton(string text, float size)
    {
        var b = new Button
        {
            Dock = DockStyle.Fill,
            FlatStyle = FlatStyle.Flat,
            Text = text,
            Margin = new Padding(2),
            Font = new Font("Segoe UI", size, FontStyle.Regular),
            Cursor = Cursors.Hand,
            UseVisualStyleBackColor = false
        };
        b.FlatAppearance.BorderSize = 0;
        return b;
    }

    public void ShowLyrics(bool show)
    {
        showingLyrics = show;
        Lyrics.Visible = show;
        Tracks.Visible = !show;
        if (show) Lyrics.BringToFront();
        else Tracks.BringToFront();
    }

    public void PopulateTracks(IReadOnlyList<string> names, int current)
    {
        Tracks.BeginUpdate();
        try
        {
            Tracks.Items.Clear();
            Tracks.Items.AddRange(names.Cast<object>().ToArray());
            if (current >= 0 && current < Tracks.Items.Count)
                Tracks.SelectedIndex = current;
        }
        finally { Tracks.EndUpdate(); }
    }

    public void UpdatePlaybackControls(
        bool active, bool playing, bool canPrevious, bool canNext)
    {
        PlayPauseButton.Enabled = active;
        StopButton.Enabled = active;
        PreviousButton.Enabled = active && canPrevious;
        NextButton.Enabled = active && canNext;
        PlayPauseButton.Text = playing ? "Ⅱ" : "▶";
    }

    public void ApplyTheme(bool dark)
    {
        var bg = dark ? Color.FromArgb(26, 39, 46) : Color.White;
        var surface = dark ? Color.FromArgb(46, 60, 69) : Color.FromArgb(234, 240, 243);
        var fg = dark ? Color.FromArgb(241, 246, 247) : Color.FromArgb(30, 44, 53);
        var accent = dark ? Color.FromArgb(64, 213, 173) : Color.FromArgb(20, 145, 119);

        BackColor = bg;
        root.BackColor = bg;
        contentHost.BackColor = bg;

        foreach (var label in new[] { heading, TrackTitle, TimeDisplay })
        {
            label.ForeColor = fg;
            label.BackColor = bg;
        }
        foreach (var button in new[] {
            MinimizeButton, CdButton, FileButton, PreviousButton,
            PlayPauseButton, NextButton, StopButton, lyricsTab, tracksTab
        })
        {
            button.BackColor = surface;
            button.ForeColor = fg;
        }
        PlayPauseButton.ForeColor = accent;

        Lyrics.BackColor = bg;
        Lyrics.ForeColor = fg;
        Tracks.BackColor = bg;
        Tracks.ForeColor = fg;

        lyricsTab.ForeColor = showingLyrics ? accent : fg;
        tracksTab.ForeColor = showingLyrics ? fg : accent;
    }
}
