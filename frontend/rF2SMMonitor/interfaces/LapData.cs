namespace rF2SMMonitor
{
    internal partial class TransitionTracker
  {
        // string -> lap data

        internal class LapData
    {
      internal class LapStats
      {
        internal int lapNumber = -1;
        internal double lapTime = -1.0;
        internal double S1Time = -1.0;
        internal double S2Time = -1.0;
        internal double S3Time = -1.0;
      }

      internal int lastLapCompleted = -1;
      internal List<LapStats> lapStats = new List<LapStats>();
    }
  }
}
