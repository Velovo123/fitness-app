using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace fitness_app.Views.Views;

public partial class VideoProgressView : ContentView
{
    public VideoProgressView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty ProgressProperty =
        BindableProperty.Create(
            nameof(Progress),
            typeof(double),
            typeof(VideoProgressView),
            0.0);

    public double Progress
    {
        get => (double)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public static readonly BindableProperty CurrentVideoTimeProperty =
        BindableProperty.Create(
            nameof(CurrentVideoTime),
            typeof(string),
            typeof(VideoProgressView),
            "00:00");

    public string CurrentVideoTime
    {
        get => (string)GetValue(CurrentVideoTimeProperty);
        set => SetValue(CurrentVideoTimeProperty, value);
    }

    public static readonly BindableProperty RingWidthProperty =
        BindableProperty.Create(
            nameof(RingWidth),
            typeof(double),
            typeof(VideoProgressView),
            100.0);

    public double RingWidth
    {
        get => (double)GetValue(RingWidthProperty);
        set => SetValue(RingWidthProperty, value);
    }

    public static readonly BindableProperty RingHeightProperty =
        BindableProperty.Create(
            nameof(RingHeight),
            typeof(double),
            typeof(VideoProgressView),
            100.0);

    public double RingHeight
    {
        get => (double)GetValue(RingHeightProperty);
        set => SetValue(RingHeightProperty, value);
    }

    public static readonly BindableProperty RingThicknessProperty =
        BindableProperty.Create(
            nameof(RingThickness),
            typeof(double),
            typeof(VideoProgressView),
            10.0);

    public double RingThickness
    {
        get => (double)GetValue(RingThicknessProperty);
        set => SetValue(RingThicknessProperty, value);
    }

    public static readonly BindableProperty RingColorProperty =
        BindableProperty.Create(
            nameof(RingColor),
            typeof(Color),
            typeof(VideoProgressView),
            Colors.Black);

    public Color RingColor
    {
        get => (Color)GetValue(RingColorProperty);
        set => SetValue(RingColorProperty, value);
    }

    public static readonly BindableProperty TimeFontFamilyProperty =
        BindableProperty.Create(
            nameof(TimeFontFamily),
            typeof(string),
            typeof(VideoProgressView),
            "MontserratSemiBold");

    public string TimeFontFamily
    {
        get => (string)GetValue(TimeFontFamilyProperty);
        set => SetValue(TimeFontFamilyProperty, value);
    }

    public static readonly BindableProperty TimeFontSizeProperty =
        BindableProperty.Create(
            nameof(TimeFontSize),
            typeof(double),
            typeof(VideoProgressView),
            14.0);

    public double TimeFontSize
    {
        get => (double)GetValue(TimeFontSizeProperty);
        set => SetValue(TimeFontSizeProperty, value);
    }

    public static readonly BindableProperty TimeTextColorProperty =
        BindableProperty.Create(
            nameof(TimeTextColor),
            typeof(Color),
            typeof(VideoProgressView),
            Colors.Black);

    public Color TimeTextColor
    {
        get => (Color)GetValue(TimeTextColorProperty);
        set => SetValue(TimeTextColorProperty, value);
    }
}