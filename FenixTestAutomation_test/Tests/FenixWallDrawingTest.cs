using NUnit.Framework;
using FlaUI.Core;
using FlaUI.UIA3;
using FenixTestAutomation.Services;
using FenixTestAutomation.Utils;
using System;
using System.IO;
using Newtonsoft.Json;

namespace FenixTestAutomation.Tests
{
    [TestFixture]
    public class FenixWallDrawingTest
    {
        private Application _app;
        private UIA3Automation _automation;
        private ProjectCreator _projectCreator;
        private ToolActivator _toolActivator;
        private ScreenshotService _screenshotService;

        [SetUp]
        public void SetUp()
        {
            _app = Application.Attach("Fenix");
            _automation = new UIA3Automation();
        }

        [TearDown]
        public void TearDown()
        {
            _automation?.Dispose();
        }

        [Test]
        public void Рисование_Стены_Через_ГорячиеКлавиши()
        {
            _projectCreator = new ProjectCreator(_app, _automation);
            var mainWindow = _projectCreator.CreateProject("Тестовый проект", "rbCivil", out string projectFolder);

            _toolActivator = new ToolActivator(mainWindow, _automation);
            _screenshotService = new ScreenshotService(mainWindow);

            // Рисуем стену длиной 3 метра
            bool result = _toolActivator.ActivateAndDrawWall(3, projectFolder);

            Assert.That(result, Is.True, "Не удалось нарисовать стену.");

            string screenshotPath = Path.Combine(projectFolder, "Screenshots", "EndDrawing.png");

            if (File.Exists(screenshotPath))
            {
                SaveAllureJsonReport("Рисование стены", screenshotPath);
            }

            _projectCreator.SaveAndClose(projectFolder);
        }

        private void SaveAllureJsonReport(string testName, string screenshotPath)
        {
            var result = new
            {
                uuid = Guid.NewGuid().ToString(),
                name = testName,
                status = "passed",
                steps = new[]
                {
                    new
                    {
                        name = "Рисование стены",
                        status = "passed",
                        attachments = new[]
                        {
                            new
                            {
                                name = "End Screenshot",
                                source = screenshotPath,
                                type = "image/png"
                            }
                        }
                    }
                }
            };

            string reportsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AllureResults");
            if (!Directory.Exists(reportsFolder))
                Directory.CreateDirectory(reportsFolder);

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);
            File.WriteAllText(Path.Combine(reportsFolder, $"{result.uuid}-result.json"), json);
        }
    }
}
