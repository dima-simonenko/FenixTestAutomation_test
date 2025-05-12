using NUnit.Framework;
using FlaUI.Core;
using FlaUI.UIA3;
using FenixTestAutomation.Services;
using FenixTestAutomation.Constants;
using FenixTestAutomation.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace FenixTestAutomation.Tests
{
    [TestFixture]
    public class FenixHotkeyTests
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
        public void Инструменты_Активируются_Через_ГорячиеКлавиши()
        {
            var projectTypes = new List<(string Name, string RadioId)>
            {
                ("Гражданский объект", "rbCivil"),
                ("Производственный объект", "rbIndustrial"),
                ("Противопожарные расстояния", "rbFireGap")
            };

            foreach (var (projectType, radioId) in projectTypes)
            {
                _projectCreator = new ProjectCreator(_app, _automation);
                var mainWindow = _projectCreator.CreateProject(projectType, radioId, out string projectFolder);

                _toolActivator = new ToolActivator(mainWindow, _automation);
                _screenshotService = new ScreenshotService(mainWindow);

                var steps = new List<object>();

                foreach (var tool in HotkeyList.Tools)
                {
                    bool result = _toolActivator.ActivateAndValidateTool(tool);
                    Assert.That(result, Is.True, $"Инструмент '{tool.Name}' не активировался.");

                    _screenshotService.Capture(tool.Name, projectFolder);

                    string screenshotPath = Path.Combine(projectFolder, "Screenshots", $"{tool.Name.Replace(" ", "_")}.png");

                    steps.Add(new
                    {
                        name = $"Активация инструмента '{tool.Name}'",
                        status = result ? "passed" : "failed",
                        attachments = new[]
                        {
                            new
                            {
                                name = "Screenshot",
                                source = screenshotPath,
                                type = "image/png"
                            }
                        }
                    });
                }

                SaveAllureJsonReport(projectType, steps);
                _projectCreator.SaveAndClose(projectFolder);
            }
        }

        private void SaveAllureJsonReport(string projectType, List<object> steps)
        {
            var result = new
            {
                uuid = Guid.NewGuid().ToString(),
                name = $"Тест проекта {projectType}",
                status = "passed",
                steps = steps
            };

            string reportsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AllureResults");
            if (!Directory.Exists(reportsFolder))
                Directory.CreateDirectory(reportsFolder);

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);
            File.WriteAllText(Path.Combine(reportsFolder, $"{result.uuid}-result.json"), json);
        }
    }
}
