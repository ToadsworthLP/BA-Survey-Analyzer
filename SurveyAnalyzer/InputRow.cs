using CsvHelper.Configuration.Attributes;

namespace SurveyAnalyzer
{
    public class InputRow
    {
        [Index(0)] public string Id { get; set; }
        [Index(1)] public string Log { get; set; }
        [Index(2)] public string IpAdress { get; set; }
        [Index(3)] public string Timestamp { get; set; }
    }
}
