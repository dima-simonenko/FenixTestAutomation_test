// Services/ProjectCreator.cs
using System;
using System.IO;
using System.Threading;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using Window = FlaUI.Core.AutomationElements.Window;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.UIA3;


namespace FenixTestAutomation.Services
{
    public class ProjectCreator
    {
        private readonly Application _app;
        private readonly UIA3Automation _automation;

        public ProjectCreator(Application app, UIA3Automation automation)
        {
            _app = app;
            _automation = automation;
        }

        public Window CreateProject(string projectTypeName, string radioId, out string projectFolder)
        {
            int counter = ReadCounter();
            string projectName = $"{projectTypeName.Replace(" ", "_")}_{counter}";
            SaveCounter(counter + 1);

            projectFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), projectName);

            var mainWindow = _app.GetMainWindow(_automation);
            mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("NewProjectBigBtn"))?.AsButton()?.Invoke();
            Console.WriteLine("Открыто окно создания проекта.");

            var newProjectWindowResult = Retry.WhileNull(() =>
                _automation.GetDesktop().FindFirstDescendant(cf => cf.ByName("Новый проект")),
                TimeSpan.FromSeconds(5));

            var window = newProjectWindowResult.Result?.AsWindow();
            window?.FindFirstDescendant(cf => cf.ByControlType(ControlType.Edit))?.AsTextBox()?.Enter(projectName);
            Console.WriteLine($"Введено имя проекта: {projectName}");

            var radio = window?.FindFirstDescendant(cf => cf.ByAutomationId(radioId))?.AsRadioButton();
            if (radio != null)
            {
                radio.IsChecked = true;
                Console.WriteLine($"Выбран тип проекта: {projectTypeName}");
            }

            window?.FindFirstDescendant(cf => cf.ByControlType(ControlType.Button).And(cf.ByName("Создать")))?.AsButton()?.Invoke();
            Console.WriteLine("Проект создан.");

            Thread.Sleep(2000);
            return mainWindow;
        }

        public void SaveAndClose(string projectFolder)
        {
            PressHotkey("Ctrl", "S");
            Thread.Sleep(3000);

            Console.WriteLine("Закрываем проект...");
            var mainWindow = _app.GetMainWindow(_automation);
            mainWindow?.FindFirstDescendant(cf => cf.ByAutomationId("CloseProjectBtn"))?.AsButton()?.Invoke();
            Thread.Sleep(1000);

            string fnxFile = Path.Combine(projectFolder, Path.GetFileName(projectFolder) + ".fnx");
            if (File.Exists(fnxFile))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✔ Файл найден: {fnxFile}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"✘ Файл НЕ найден: {fnxFile}");
            }
            Console.ResetColor();
        }

        private void PressHotkey(string modifier, string key)
        {
            var mod = Utils.KeyParser.Parse(modifier);
            var main = Utils.KeyParser.Parse(key) ?? throw new ArgumentException($"Неизвестная клавиша: {key}");

            if (mod.HasValue) Keyboard.Press(mod.Value);
            Keyboard.Press(main);
            Keyboard.Release(main);
            if (mod.HasValue) Keyboard.Release(mod.Value);
        }

        private int ReadCounter()
        {
            const string path = "project_counter.txt";
            if (!File.Exists(path)) return 1;
            var text = File.ReadAllText(path);
            return int.TryParse(text, out int result) ? result : 1;
        }

        private void SaveCounter(int newValue)
        {
            const string path = "project_counter.txt";
            File.WriteAllText(path, newValue.ToString());
        }
    }
}