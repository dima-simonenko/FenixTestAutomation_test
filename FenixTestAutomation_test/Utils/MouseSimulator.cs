using System;
using System.IO;
using System.Drawing;
using System.Threading;
using FlaUI.Core.Input;
using FlaUI.Core.Capturing;
using FlaUI.UIA3;
using FlaUI.Core.AutomationElements;

namespace FenixTestAutomation.Utils
{
    public static class MouseSimulator
    {
        public static void DrawLine(Window mainWindow, int deltaX, int deltaY, string projectFolder)
        {
            // Вычисляем центр окна вручную
            var rect = mainWindow.BoundingRectangle;
            var centerX = rect.Left + rect.Width / 2;
            var centerY = rect.Top + rect.Height / 2;
            var editorCenter = new System.Drawing.Point(centerX, centerY);

            Mouse.MoveTo(editorCenter);
            Thread.Sleep(200); // Даём UI время подготовиться

            // Скриншот перед началом рисования
            CaptureScreenshot("Before_Draw", projectFolder);

            // 1. Первый клик — начинаем рисовать стену
            Mouse.Click(MouseButton.Left);
            Thread.Sleep(200);

            // 2. Перемещаем курсор для задания длины стены
            var endPosition = new System.Drawing.Point(editorCenter.X + deltaX, editorCenter.Y + deltaY);
            Mouse.MoveTo(endPosition);
            Thread.Sleep(200);

            // 3. Второй клик — завершаем рисование стены
            Mouse.Click(MouseButton.Left);
            Thread.Sleep(200);

            // Скриншот после завершения рисования
            CaptureScreenshot("After_Draw", projectFolder);
        }




        private static void CaptureScreenshot(string fileName, string projectFolder)
        {
            try
            {
                string screenshotsDir = Path.Combine(projectFolder, "Screenshots");
                if (!Directory.Exists(screenshotsDir))
                {
                    Directory.CreateDirectory(screenshotsDir);
                }

                string safeFileName = $"{fileName.Replace(" ", "_")}.png";
                string filePath = Path.Combine(screenshotsDir, safeFileName);

                using (var automation = new UIA3Automation())
                {
                    var desktop = automation.GetDesktop();
                    var appWindow = desktop.FindFirstDescendant(cf => cf.ByClassName("Fenix"))?.AsWindow();

                    if (appWindow != null)
                    {
                        var image = Capture.Element(appWindow);
                        image.ToFile(filePath);
                    }
                    else
                    {
                        Console.WriteLine("Окно приложения Fenix не найдено для скриншота.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении скриншота: {ex.Message}");
            }
        }

    }
}
