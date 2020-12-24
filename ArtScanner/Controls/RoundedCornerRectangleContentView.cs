using System;
using Xamarin.Forms;

namespace ArtScanner.Controls
{
    public class RoundedCornerRectangleContentView : ContentView
    {
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(int),
            typeof(RoundedCornerRectangleContentView), 0);

        public int CornerRadius
        {
            get => (int)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Xamarin.Forms.Color),
            typeof(RoundedCornerRectangleContentView), Xamarin.Forms.Color.White);

        public Xamarin.Forms.Color Color
        {
            get => (Xamarin.Forms.Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly BindableProperty InnerColorProperty = BindableProperty.Create(nameof(Color), typeof(Xamarin.Forms.Color),
            typeof(RoundedCornerRectangleContentView), Xamarin.Forms.Color.White);

        public Xamarin.Forms.Color InnerColor
        {
            get => (Xamarin.Forms.Color)GetValue(InnerColorProperty);
            set => SetValue(InnerColorProperty, value);
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Xamarin.Forms.Color),
            typeof(RoundedCornerRectangleContentView), Xamarin.Forms.Color.FromHex("#DDE2E8"));

        public Xamarin.Forms.Color BorderColor
        {
            get => (Xamarin.Forms.Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }
    }
}
