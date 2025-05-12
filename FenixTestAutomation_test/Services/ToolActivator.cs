using System;
using System.IO;
using System.Threading;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using FenixTestAutomation.Constants;
using FenixTestAutomation.Utils;

namespace FenixTestAutomation.Services
{
    public class ToolActivator
    {
        private readonly Window _mainWindow;
        private readonly UIA3Automation _automation;

        public ToolActivator(Window mainWindow, UIA3Automation automation)
        {
            _mainWindow = mainWindow;
            _automation = automation;
        }

        public bool ActivateAndValidateTool(HotkeyTool tool)
        {
            try
            {
                _mainWindow.Focus();
                _mainWindow.SetForeground();
                Thread.Sleep(500);

                PressHotkey(tool.Modifier, tool.Key);
                Thread.Sleep(1200);

                var result = _automation.GetDesktop().FindFirstDescendant(cf => cf.ByName(tool.Name)) != null;
                Console.WriteLine(result
                    ? $"Инструмент '{tool.Name}' активировался и найден."
                    : $"Инструмент '{tool.Name}' не отобразился.");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при активации '{tool.Name}': {ex.Message}");
                return false;
            }
        }

        public bool ActivateAndDrawWall(double lengthInMeters, string projectFolder)
        {
            try
            {
                _mainWindow.Focus();
                _mainWindow.SetForeground();

                PressHotkey("Alt", "W");
                Thread.Sleep(500);

                int pixelsPerMeter = 100;
                int deltaX = (int)(lengthInMeters * pixelsPerMeter);

                // Рисуем стену и делаем скриншоты до и после
                MouseSimulator.DrawLine(_mainWindow, deltaX, 0, projectFolder);


                // Завершаем рисование
                Mouse.Click(MouseButton.Left);
                Thread.Sleep(500);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при рисовании стены: {ex.Message}");
                return false;
            }
        }

        private void PressHotkey(string modifier, string key)
        {
            var modKey = KeyParser.Parse(modifier);
            var mainKey = KeyParser.Parse(key) ?? throw new ArgumentException($"Неизвестная клавиша: {key}");

            _mainWindow.Focus();
            _mainWindow.SetForeground();
            Thread.Sleep(300);

            if (modKey.HasValue) Keyboard.Press(modKey.Value);
            Keyboard.Press(mainKey);
            Keyboard.Release(mainKey);
            if (modKey.HasValue) Keyboard.Release(modKey.Value);
        }
    }
}
