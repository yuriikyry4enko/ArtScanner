using System;
using System.ComponentModel;
using ArtScanner.Controls;
using ArtScanner.iOS.Renderers;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RoundedCornerRectangleContentView), typeof(RoundedCornerRectangleContentViewRenderer))]
namespace ArtScanner.iOS.Renderers
{
    public class RoundedCornerRectangleContentViewRenderer : VisualElementRenderer<RoundedCornerRectangleContentView>
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            SetNeedsDisplay();
        }

        public override void Draw(CGRect rect)
        {
            if (Element == null) return;

            var frame = Element as RoundedCornerRectangleContentView;

            using (var context = UIGraphics.GetCurrentContext())
            {
                var path = new CGPath();

                path.MoveToPoint(rect.Left, rect.Top);
                path.AddLineToPoint(rect.Right, rect.Top);
                path.AddLineToPoint(rect.Right, rect.Bottom);
                path.AddLineToPoint(rect.Left, rect.Bottom);
                path.AddLineToPoint(rect.Left, rect.Top);

                path.AddLineToPoint(rect.Left, rect.Height / 2f);

                // left bottom arc
                path.AddArcToPoint(rect.Left, rect.Bottom, rect.Width / 2f, rect.Bottom, (float)frame.CornerRadius);

                // right bottom arc
                path.AddArcToPoint(rect.Right, rect.Bottom, rect.Right, rect.Height / 2f, (float)frame.CornerRadius);

                // right top arc
                path.AddArcToPoint(rect.Right, rect.Top, rect.Width / 2f, rect.Top, (float)frame.CornerRadius);

                // left top arc
                path.AddArcToPoint(rect.Left, rect.Top, rect.Left, rect.Height / 2f, (float)frame.CornerRadius);

                path.CloseSubpath();

                context.SaveState();

                context.AddPath(path);
                context.SetFillColor(frame.Color.ToCGColor());
                context.DrawPath(CGPathDrawingMode.Fill);

                context.RestoreState();


                var cornerRadius = Math.Min((rect.Width < frame.CornerRadius * 2 ? rect.Width / 2 : frame.CornerRadius),
                    (rect.Height < frame.CornerRadius * 2 ? rect.Height / 2 : frame.CornerRadius));

                path = CGPath.FromRoundedRect(rect, (float)cornerRadius, (float)cornerRadius);
                context.AddPath(path);
                context.SetStrokeColor(frame.BorderColor.ToCGColor());
                context.SetFillColor(frame.InnerColor.ToCGColor());

                context.DrawPath(CGPathDrawingMode.FillStroke);
            }

        }
    }
}