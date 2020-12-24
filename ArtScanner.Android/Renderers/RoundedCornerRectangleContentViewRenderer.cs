using Android.Content;
using Android.Graphics;
using Android.Util;
using ArtScanner.Controls;
using ArtScanner.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Application = Android.App.Application;

[assembly: ExportRenderer(typeof(RoundedCornerRectangleContentView), typeof(RoundedCornerRectangleContentViewRenderer))]
namespace ArtScanner.Droid.Renderers
{
    public class RoundedCornerRectangleContentViewRenderer : VisualElementRenderer<RoundedCornerRectangleContentView>
    {
        public RoundedCornerRectangleContentViewRenderer(Context context) : base(context)
        {
            SetWillNotDraw(false);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            var frame = (RoundedCornerRectangleContentView)Element;

            var cornerRadius = UnitsConverter.DpToPx(frame.CornerRadius);

            var path = new Path();
            path.MoveTo(0, 0);
            path.LineTo(Width, 0);
            path.LineTo(Width, Height);
            path.LineTo(0, Height);
            path.LineTo(0, 0);

            path.LineTo(0, Height - cornerRadius * 2);

            path.ArcTo(new RectF(0, (float)(Height - cornerRadius * 2), (float)cornerRadius * 2, (float)Height), 180, -90);

            path.LineTo(Width - cornerRadius * 2, Height);

            path.ArcTo(new RectF(Width - cornerRadius * 2, (Height - cornerRadius * 2), Width, Height), 90, -90);

            path.LineTo(Width, cornerRadius * 2);

            path.ArcTo(new RectF(Width - cornerRadius * 2, 0, Width, cornerRadius * 2), 0, -90);

            path.LineTo(cornerRadius * 2, 0);

            path.ArcTo(new RectF(0, 0, cornerRadius * 2, cornerRadius * 2), -90, -90);

            path.Close();

            var framePaint = new Paint() { AntiAlias = true, Color = frame.Color.ToAndroid() };

            canvas.DrawPath(path, framePaint);

            var innerPaint = new Paint() { AntiAlias = true, Color = frame.InnerColor.ToAndroid() };
            innerPaint.SetStyle(Paint.Style.Fill);

            canvas.DrawRoundRect(0, 0, Width, Height, cornerRadius, cornerRadius, innerPaint);

            var borderPaint = new Paint() { AntiAlias = true, Color = frame.BorderColor.ToAndroid(), StrokeWidth = 1 };
            borderPaint.SetStyle(Paint.Style.Stroke);

            canvas.DrawRoundRect(0, 0, Width, Height, cornerRadius, cornerRadius, borderPaint);
        }
    }

    class UnitsConverter
    {
        public static int DpToPx(float dp)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, Application.Context.Resources.DisplayMetrics);
        }
    }
}