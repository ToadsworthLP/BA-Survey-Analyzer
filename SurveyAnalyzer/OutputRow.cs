namespace SurveyAnalyzer
{
    public class OutputRow
    {
        public string Id { get; set; }
        public string Timestamp { get; set; }
        public string Group { get; set; }
        public double Stage0Time { get; set; }
        public double Stage1Time { get; set; }
        public double Stage2Time { get; set; }
        public double TotalTime { get => Math.Round(Stage0Time + Stage1Time + Stage2Time, 3); }
        public int Stage0Resets { get; set; }
        public int Stage1Resets { get; set; }
        public int Stage2Resets { get; set; }
        public int TotalResets { get => Stage0Resets + Stage1Resets + Stage2Resets; }
        public int Stage0Deaths { get; set; }
        public int Stage1Deaths { get; set; }
        public int Stage2Deaths { get; set; }
        public int TotalDeaths { get => Stage0Deaths + Stage1Deaths + Stage2Deaths; }

        public void SetTime(double time, int stage)
        {
            switch (stage)
            {
                case 0:
                    Stage0Time = time;
                    break;

                case 1:
                    Stage1Time = time;
                    break;

                case 2:
                    Stage2Time = time;
                    break;
            }
        }

        public void SetResets(int count, int stage)
        {
            switch (stage)
            {
                case 0:
                    Stage0Resets = count;
                    break;

                case 1:
                    Stage1Resets = count;
                    break;

                case 2:
                    Stage2Resets = count;
                    break;
            }
        }

        public void SetDeaths(int count, int stage)
        {
            switch (stage)
            {
                case 0:
                    Stage0Deaths = count;
                    break;

                case 1:
                    Stage1Deaths = count;
                    break;

                case 2:
                    Stage2Deaths = count;
                    break;
            }
        }
    }
}
