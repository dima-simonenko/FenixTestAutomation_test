using System;
using System.IO;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;

namespace FenixTestAutomation.Services
{
    public class ScreenshotService
    {
        private readonly Window _mainWindow;

        public ScreenshotService(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void Capture(string fileName, string projectFolder)
        {
            try
            {
                var screenshotsDir = Path.Combine(projectFolder, "Screenshots");
                if (!Directory.Exists(screenshotsDir))
                    Directory.CreateDirectory(screenshotsDir);

                var safeFileName = $"{fileName.Replace(" ", "_")}.png";
                var filePath = Path.Combine(screenshotsDir, safeFileName);

                var image = FlaUI.Core.Capturing.Capture.Element(_mainWindow);
                image.ToFile(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении скриншота: {ex.Message}");
            }
        }
    }
}
