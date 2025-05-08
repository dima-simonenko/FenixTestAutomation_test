// Services/ScreenshotService.cs
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;

using DrawingFont = System.Drawing.Font;
using DrawingBrushes = System.Drawing.Brushes;
using DrawingPoint = System.Drawing.Point;
using DrawingFontStyle = System.Drawing.FontStyle;
using DrawingImageFormat = System.Drawing.Imaging.ImageFormat;
using DrawingBitmap = System.Drawing.Bitmap;

namespace FenixTestAutomation.Services
{
    public class ScreenshotService
    {
        private readonly Window _window;

        public ScreenshotService(Window window)
        {
            _window = window;
        }

        public void Capture(string toolName, string projectFolder)
        {
            string screenshotFolder = Path.Combine(projectFolder, "Screenshots");
            if (!Directory.Exists(screenshotFolder))
                Directory.CreateDirectory(screenshotFolder);

            string safeName = toolName.Replace(" ", "_") + ".png";
            string filePath = Path.Combine(screenshotFolder, safeName);

            var img = FlaUI.Core.Capturing.Capture.Element(_window);
            using (var bmp = new DrawingBitmap(img.Bitmap))
            using (var g = Graphics.FromImage(bmp))
            {
                var font = new DrawingFont("Arial", 16, DrawingFontStyle.Bold);
                var brush = DrawingBrushes.Yellow;
                var outline = DrawingBrushes.Black;
                var position = new DrawingPoint(10, 10);

                g.DrawString(toolName, font, outline, position.X + 1, position.Y + 1);
                g.DrawString(toolName, font, brush, position);

                bmp.Save(filePath, DrawingImageFormat.Png);
            }
        }
    }
}