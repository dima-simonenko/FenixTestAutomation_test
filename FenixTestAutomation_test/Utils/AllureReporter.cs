using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace FenixTestAutomation.Utils
{
    public class AllureResult
    {
        public string uuid { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string stage { get; set; } = "finished";
        public List<Step> steps { get; set; } = new List<Step>();
    }

    public class Step
    {
        public string name { get; set; }
        public string status { get; set; }
        public List<Attachment> attachments { get; set; } = new List<Attachment>();
    }

    public class Attachment
    {
        public string name { get; set; }
        public string type { get; set; } = "image/png";
        public string source { get; set; }
    }

    public static class AllureReporter
    {
        public static void CreateTestResult(string uuid, string testName, string status, List<Step> steps)
        {
            var result = new AllureResult
            {
                uuid = uuid,
                name = testName,
                status = status,
                steps = steps
            };

            var json = JsonConvert.SerializeObject(result, Formatting.Indented);
            var reportDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AllureResults");

            if (!Directory.Exists(reportDir))
                Directory.CreateDirectory(reportDir);

            File.WriteAllText(Path.Combine(reportDir, $"{uuid}.json"), json);
        }
    }
}
