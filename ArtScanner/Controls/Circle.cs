using System;
using Xamarin.Forms;

namespace ArtScanner.Controls
{
    public class Circle : ContentView
    {
        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color),
            typeof(Circle), Xamarin.Forms.Color.Black);

        public Xamarin.Forms.Color Color
        {
            get => (Xamarin.Forms.Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
    }
}
