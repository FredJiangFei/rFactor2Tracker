namespace rF2SMMonitor
{
    internal partial class TransitionTracker
  {
        internal class PlayerTimingInfo
    {
      internal string name = null;
      internal double lastS1Time = -1.0;
      internal double lastS2Time = -1.0;
      internal double lastS3Time = -1.0;

      internal double currS1Time = -1.0;
      internal double currS2Time = -1.0;
      internal double currS3Time = -1.0;

      internal double bestS1Time = -1.0;
      internal double bestS2Time = -1.0;
      internal double bestS3Time = -1.0;

      internal double currLapET = -1.0;
      internal double lastLapTime = -1.0;
      internal double currLapTime = -1.0;
      internal double bestLapTime = -1.0;

      internal int currLap = -1;
    }
  }
}
