using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace SurveyAnalyzer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputPath = args[0];
            string outputPath = args[1];

            CultureInfo culture = (CultureInfo)CultureInfo.CurrentUICulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            culture.NumberFormat.NumberGroupSeparator = ",";
            Thread.CurrentThread.CurrentCulture = culture;

            CsvConfiguration inputConfig = new CsvConfiguration(culture)
            {
                HasHeaderRecord = false,
            };

            CsvConfiguration outputConfig = new CsvConfiguration(culture)
            {
                Delimiter = "\t",
            };

            IEnumerable<InputRow> inputRows;
            IList<OutputRow> outputRows = new List<OutputRow>();

            ISet<string> ipAdresses = new HashSet<string>();

            using (StreamReader reader = new StreamReader(inputPath))
            using (CsvReader csv = new CsvReader(reader, inputConfig))
            {
                inputRows = csv.GetRecords<InputRow>();

                foreach (InputRow input in inputRows)
                {
                    if (!ipAdresses.Contains(input.IpAdress))
                    {
                        outputRows.Add(ProcessRow(input));
                        ipAdresses.Add(input.IpAdress);
                    }
                    else
                    {
                        Console.WriteLine($"[WARNING] Multiple entries found for IP adress {input.IpAdress}. Skipping entry with ID {input.Id}!");
                    }
                }
            }

            outputRows = outputRows.OrderBy(e => e.Group).ToList();

            using (var writer = new StreamWriter(outputPath))
            using (var csv = new CsvWriter(writer, outputConfig))
            {
                csv.WriteRecords(outputRows);
            }
        }

        private static OutputRow ProcessRow(InputRow input)
        {
            OutputRow output = new OutputRow()
            {
                Id = input.Id,
                Timestamp = input.Timestamp,
            };

            string[] entries = input.Log.Split("-");
            int[] stageStartTimes = new int[3];
            int[] stageResets = new int[3];
            int[] stageDeaths = new int[3];

            for (int i = 0; i < entries.Length; i++)
            {
                string[] current = entries[i].Split("/");
                EntryCategory category = (EntryCategory)int.Parse(current[0]);

                int stage;
                switch (category)
                {
                    case EntryCategory.GAME_START:
                        output.Group = current[2];
                        break;
                    case EntryCategory.STAGE_START:
                        stage = int.Parse(current[2]);
                        stageStartTimes[stage] = int.Parse(current[1]);
                        break;
                    case EntryCategory.RESET:
                        stage = int.Parse(current[2]);
                        stageResets[stage]++;
                        break;
                    case EntryCategory.DEATH:
                        stage = int.Parse(current[2]);
                        stageDeaths[stage]++;
                        break;
                    case EntryCategory.STAGE_CLEAR:
                        stage = int.Parse(current[2]);
                        output.SetTime((int.Parse(current[1]) - stageStartTimes[stage]) / 1000.0, stage);
                        break;
                    case EntryCategory.GAME_CLEAR:
                        break;
                }
            }

            for (int i = 0; i < stageResets.Count(); i++)
            {
                output.SetResets(stageResets[i], i);
                output.SetDeaths(stageDeaths[i], i);
            }

            return output;
        }

        public enum EntryCategory { GAME_START = 0, STAGE_START = 1, RESET = 2, DEATH = 3, STAGE_CLEAR = 4, GAME_CLEAR = 5 }
    }
}