﻿using System.Windows;


namespace View.Utils.Dialog;

public record WindowParameters
{
    public int Height { get; init; } = 200;
    public int Width { get; init; } = 300;
    public int MaxHeight { get; init; }
    public int MaxWidth { get; init; }
    public WindowStartupLocation StartupLocation { get; init; } = WindowStartupLocation.CenterScreen;
    public WindowState WindowState { get; set; } = WindowState.Normal;
    public string Title { get; init; } = "";
}

