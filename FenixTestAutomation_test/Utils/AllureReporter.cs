using System;
using System.IO;
using Newtonsoft.Json;

public class AllureTestResult
{
    public string uuid { get; set; }
    public string name { get; set; }
    public string fullName { get; set; }
    public string status { get; set; }
    public string stage { get; set; } = "finished";
    public Step[] steps { get; set; }
    public Attachment[] attachments { get; set; } = Array.Empty<Attachment>();
    public Parameter[] parameters { get; set; }
    public long start { get; set; }
    public long stop { get; set; }
}

public class Step
{
    public string name { get; set; }
    public string status { get; set; }
    public Attachment[] attachments { get; set; } = Array.Empty<Attachment>();
}

public class Attachment
{
    public string name { get; set; }
    public string source { get; set; }
    public string type { get; set; }
}

public class Parameter
{
    public string name { get; set; }
    public string value { get; set; }
}

// 📌 Генерация отчёта
public static class AllureReportGenerator
{
    public static void CreateTestResult(string testName, string wallLength)
    {
        var uuid = Guid.NewGuid().ToString();
        var start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var stop = start + 5000;

        var result = new AllureTestResult
        {
            uuid = uuid,
            name = testName,
            fullName = $"FenixWallDrawingTest.{testName}",
            status = "passed",
            start = start,
            stop = stop,
            steps = new[]
            {
                new Step
                {
                    name = "Активация инструмента 'Стена'",
                    status = "passed",
                    attachments = new[]
                    {
                        new Attachment
                        {
                            name = "Screenshot",
                            source = "screenshot_wall.png",
                            type = "image/png"
                        }
                    }
                },
                new Step
                {
                    name = $"Рисование стены длиной {wallLength} м",
                    status = "passed"
                }
            },
            parameters = new[]
            {
                new Parameter { name = "Длина стены", value = wallLength }
            }
        };

        var json = JsonConvert.SerializeObject(result, Formatting.Indented);
        var reportPath = @"C:\Users\dimas\Documents\allure-results";
        Directory.CreateDirectory(reportPath);
        File.WriteAllText(Path.Combine(reportPath, $"{uuid}.json"), json);
    }
}
