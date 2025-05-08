// Services/ToolActivator.cs
using System;
using System.IO;
using System.Linq;
using System.Threading;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
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

                var result = Retry.WhileNull(() =>
                    _automation.GetDesktop().FindFirstDescendant(cf => cf.ByName(tool.Name)),
                    TimeSpan.FromSeconds(4));

                if (result.Success)
                {
                    ConsoleHelper.WriteSuccess($"Инструмент '{tool.Name}' активировался и найден.");
                    return true;
                }
                else
                {
                    ConsoleHelper.WriteWarning($"Инструмент '{tool.Name}' не отобразился. Сохраняем скриншот...");
                    var timestamp = DateTime.Now.ToString("HHmmss");
                    var safeName = tool.Name.Replace(" ", "_");
                    var screenshotPath = Path.Combine("Screenshots", $"fail_{safeName}_{timestamp}.png");
                    var img = Capture.Element(_mainWindow);
                    img.ToFile(screenshotPath);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteError($"Ошибка при активации '{tool.Name}': {ex.Message}");
                return false;
            }
        }

        private void PressHotkey(string modifier, string key)
        {
            var mod = KeyParser.Parse(modifier);
            var main = KeyParser.Parse(key) ?? throw new ArgumentException($"Неизвестная клавиша: {key}");

            _mainWindow.Focus();
            _mainWindow.SetForeground();
            Thread.Sleep(300);

            ConsoleHelper.WriteInfo($"Нажимаем: {modifier}+{key}");

            if (mod.HasValue) Keyboard.Press(mod.Value);
            Keyboard.Press(main);
            Keyboard.Release(main);
            if (mod.HasValue) Keyboard.Release(mod.Value);
        }
    }
}