// Tests/FenixHotkeyTests.cs
using NUnit.Framework;
using FlaUI.Core;
using FlaUI.UIA3;
using FenixTestAutomation.Services;
using FenixTestAutomation.Constants;
using FenixTestAutomation.Utils;

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

        [TestCase("Гражданский объект", "rbCivil")]
        [TestCase("Производственный объект", "rbIndustrial")]
        [TestCase("Противопожарные расстояния", "rbFireGap")]
        public void Инструменты_Активируются_Через_ГорячиеКлавиши(string projectType, string radioId)
        {
            _projectCreator = new ProjectCreator(_app, _automation);
            var mainWindow = _projectCreator.CreateProject(projectType, radioId, out string projectFolder);

            _toolActivator = new ToolActivator(mainWindow, _automation);
            _screenshotService = new ScreenshotService(mainWindow);

            foreach (var tool in HotkeyList.Tools)
            {
                bool result = _toolActivator.ActivateAndValidateTool(tool);
                Assert.That(result, Is.True, $"Инструмент '{tool.Name}' не активировался.");

                _screenshotService.Capture(tool.Name, projectFolder);
            }

            _projectCreator.SaveAndClose(projectFolder);
        }
    }
}