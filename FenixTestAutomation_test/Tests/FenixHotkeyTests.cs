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

        private const string AllureResultsPath = @"C:\Users\dimas\Documents\AllureResults";

        [SetUp]
        public void SetUp()
        {
            _app = Application.Attach("Fenix");
            _automation = new UIA3Automation();

            if (!Directory.Exists(AllureResultsPath))
                Directory.CreateDirectory(AllureResultsPath);
        }

        [TearDown]
        public void TearDown()
        {
            _automation?.Dispose();
        }

        [TestCase("Гражданский объект", "rbCivil")]
        [TestCase("Производственный объект", "rbIndustrial")]
        [TestCase("Противопожарные расстояния", "rbFireGap")]
        public void Инструменты_Активируются_Через_ГорячиеКлавиши(string projectType, string radioId)
        {
            _projectCreator = new ProjectCreator(_app, _automation);
            var mainWindow = _projectCreator.CreateProject(projectType, radioId, out string projectFolder);

            _toolActivator = new ToolActivator(mainWindow, _automation);
            _screenshotService = new ScreenshotService(mainWindow);

            var steps = new List<AllureStep>();

            foreach (var tool in HotkeyList.Tools)
            {
                bool result = _toolActivator.ActivateAndValidateTool(tool);
                Assert.That(result, Is.True, $"Инструмент '{tool.Name}' не активировался.");

                _screenshotService.Capture(tool.Name, projectFolder);

                var screenshotPath = Path.Combine(projectFolder, "Screenshots", $"{tool.Name.Replace(" ", "_")}.png");

                steps.Add(new AllureStep
                {
                    name = $"Активация инструмента '{tool.Name}'",
                    status = result ? "passed" : "failed",
                    attachments = new List<AllureAttachment>
                    {
                        new AllureAttachment
                        {
                            name = "Screenshot",
                            type = "image/png",
                            source = screenshotPath
                        }
                    }
                });
            }

            var testResult = new AllureTestResult
            {
                uuid = Guid.NewGuid().ToString(),
                name = $"Тест проекта {projectType}",
                status = "passed",
                steps = steps
            };

            var resultFile = Path.Combine(AllureResultsPath, $"{testResult.uuid}-result.json");
            File.WriteAllText(resultFile, JsonConvert.SerializeObject(testResult, Formatting.Indented));

            _projectCreator.SaveAndClose(projectFolder);
        }
    }

    public class AllureTestResult
    {
        public string uuid { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public List<AllureStep> steps { get; set; }
    }

    public class AllureStep
    {
        public string name { get; set; }
        public string status { get; set; }
        public List<AllureAttachment> attachments { get; set; }
    }

    public class AllureAttachment
    {
        public string name { get; set; }
        public string type { get; set; }
        public string source { get; set; }
    }
}
