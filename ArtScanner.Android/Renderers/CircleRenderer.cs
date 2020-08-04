using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using ArtScanner.Controls;
using ArtScanner.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ArtScanner.Controls.Circle), typeof(CircleRenderer))]
namespace ArtScanner.Droid.Renderers
{
    public class CircleRenderer : VisualElementRenderer<Circle>
    {
        public CircleRenderer(Context context) : base(context)
        {
            SetWillNotDraw(false);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            var circle = (Circle)Element;

            if (Width == 0 || Height == 0) return;

            canvas.DrawCircle(Width / 2f, Height / 2f, Math.Min(Width, Height) / 2f, new Paint() { Color = circle.Color.ToAndroid() });
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Circle.ColorProperty.PropertyName)
            {
                Invalidate();
            }
        }
    }
}