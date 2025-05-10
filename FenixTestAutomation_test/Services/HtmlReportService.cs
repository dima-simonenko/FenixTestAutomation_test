// Services/HtmlReportService.cs
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace FenixTestAutomation.Services
{
    public class HtmlReportService
    {
        private readonly string _reportFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Reports");

        private readonly StringBuilder _htmlContent = new StringBuilder();
        private int _projectCount = 0;
        private int _toolsPassed = 0;
        private int _toolsFailed = 0;

        public HtmlReportService()
        {
            if (!Directory.Exists(_reportFolder))
                Directory.CreateDirectory(_reportFolder);

            InitializeReport();
        }

        private void InitializeReport()
        {
            _htmlContent.AppendLine("<html><head><title>Fenix Automation Report</title>");
            _htmlContent.AppendLine("<style>");
            _htmlContent.AppendLine("body{font-family:Arial;} table{border-collapse:collapse;width:100%;} th,td{border:1px solid #ddd;padding:8px;} th{background-color:#f2f2f2;} img{margin-top:5px;cursor:pointer;} .modal{display:none;position:fixed;z-index:1;padding-top:100px;left:0;top:0;width:100%;height:100%;overflow:auto;background-color:rgba(0,0,0,0.8);} .modal-content {margin:auto;display:block;width:95%;max-width:1200px;}\r\n .close{position:absolute;top:50px;right:50px;color:white;font-size:40px;font-weight:bold;cursor:pointer;}");
            _htmlContent.AppendLine("</style>");

            _htmlContent.AppendLine("<script>");
            _htmlContent.AppendLine("function showModal(src) { var modal = document.getElementById('imgModal'); var modalImg = document.getElementById('modalImage'); modal.style.display = 'block'; modalImg.src = src; } function closeModal() { document.getElementById('imgModal').style.display = 'none'; }");
            _htmlContent.AppendLine("</script>");

            _htmlContent.AppendLine("</head><body>");
            _htmlContent.AppendLine("<h1>Отчёт об автоматизации тестирования Fenix</h1>");
            _htmlContent.AppendLine($"<p>Дата: {DateTime.Now}</p>");

            // Добавляем модальное окно для увеличения изображения
            _htmlContent.AppendLine("<div id='imgModal' class='modal'>");
            _htmlContent.AppendLine("<span class='close' onclick='closeModal()'>&times;</span>");
            _htmlContent.AppendLine("<img class='modal-content' id='modalImage'>");
            _htmlContent.AppendLine("</div>");
        }

        public void AddProjectSection(string projectName, string projectFilePath, List<(string ToolName, bool IsSuccess, string ScreenshotPath)> toolResults)
        {
            _projectCount++;
            _htmlContent.AppendLine($"<h2>Проект: {projectName}</h2>");
            _htmlContent.AppendLine($"<p>Файл проекта: {projectFilePath}</p>");
            _htmlContent.AppendLine("<table>");
            _htmlContent.AppendLine("<tr><th>Инструмент</th><th>Статус</th><th>Скриншот</th></tr>");

            foreach (var result in toolResults)
            {
                var status = result.IsSuccess ? "✔" : "✘";
                if (result.IsSuccess) _toolsPassed++; else _toolsFailed++;

                _htmlContent.AppendLine("<tr>");
                _htmlContent.AppendLine($"<td>{result.ToolName}</td>");
                _htmlContent.AppendLine($"<td style='text-align:center;'>{status}</td>");

                if (File.Exists(result.ScreenshotPath))
                {
                    var base64Image = Convert.ToBase64String(File.ReadAllBytes(result.ScreenshotPath));
                    _htmlContent.AppendLine($"<td><img src='data:image/png;base64,{base64Image}' width='300' onclick='showModal(this.src)'/></td>");
                }
                else
                {
                    _htmlContent.AppendLine("<td>Скриншот не найден</td>");
                }

                _htmlContent.AppendLine("</tr>");
            }

            _htmlContent.AppendLine("</table><hr/>");
        }

        public void FinalizeReport()
        {
            _htmlContent.AppendLine("<h2>Итоги сессии</h2>");
            _htmlContent.AppendLine($"<p>Проектов обработано: {_projectCount}</p>");
            _htmlContent.AppendLine($"<p>Инструментов успешно активировано: {_toolsPassed}</p>");
            _htmlContent.AppendLine($"<p>Инструментов с ошибками: {_toolsFailed}</p>");
            _htmlContent.AppendLine("</body></html>");

            var reportFile = Path.Combine(_reportFolder, $"Report_{DateTime.Now:yyyyMMdd_HHmmss}.html");
            File.WriteAllText(reportFile, _htmlContent.ToString(), Encoding.UTF8);

            Console.WriteLine($"✔ HTML отчёт успешно сформирован: {reportFile}");
        }
    }
}
