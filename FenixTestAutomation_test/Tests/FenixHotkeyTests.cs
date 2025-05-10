// Tests/FenixHotkeyTests.cs
using NUnit.Framework;
using FlaUI.Core;
using FlaUI.UIA3;
using FenixTestAutomation.Services;
using FenixTestAutomation.Constants;
using FenixTestAutomation.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        private static HtmlReportService _htmlReportService = new HtmlReportService();

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

        [OneTimeTearDown]
        public void FinalizeReport()
        {
            _htmlReportService.FinalizeReport();
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

            var toolResults = new List<(string ToolName, bool IsSuccess, string ScreenshotPath)>();

            foreach (var tool in HotkeyList.Tools)
            {
                bool result = _toolActivator.ActivateAndValidateTool(tool);
                Assert.That(result, Is.True, $"Инструмент '{tool.Name}' не активировался.");

                _screenshotService.Capture(tool.Name, projectFolder);

                var screenshotPath = Path.Combine(projectFolder, "Screenshots", $"{tool.Name.Replace(" ", "_")}.png");
                toolResults.Add((tool.Name, result, screenshotPath));
            }

            string fnxFilePath = Path.Combine(projectFolder, $"{projectType.Replace(" ", "_")}.fnx");

            _htmlReportService.AddProjectSection(projectType, fnxFilePath, toolResults);

            _projectCreator.SaveAndClose(projectFolder);
        }
    }
}
