using static rF2SMMonitor.rFactor2Constants;

namespace rF2SMMonitor
{
    internal partial class TransitionTracker
  {
        // Telemetry values (separate section)

        internal class PhaseAndState
    {
      internal rF2GamePhase mGamePhase = (rF2GamePhase)Enum.ToObject(typeof(rF2GamePhase), -255);
      internal int mSession = -255;
      internal rF2YellowFlagState mYellowFlagState = (rF2YellowFlagState)Enum.ToObject(typeof(rF2YellowFlagState), -255);
      internal int mSector = -255;
      internal int mCurrentSector = -255;
      internal byte mInRealTimeFC = 255;
      internal byte mInRealTime = 255;
      internal rF2YellowFlagState mSector1Flag = (rF2YellowFlagState)Enum.ToObject(typeof(rF2YellowFlagState), -255);
      internal rF2YellowFlagState mSector2Flag = (rF2YellowFlagState)Enum.ToObject(typeof(rF2YellowFlagState), -255);
      internal rF2YellowFlagState mSector3Flag = (rF2YellowFlagState)Enum.ToObject(typeof(rF2YellowFlagState), -255);
      internal rF2Control mControl;
      internal byte mInPits = 255;
      internal byte mIsPlayer = 255;
      internal int mPlace = -255;
      internal rF2PitState mPitState = (rF2PitState)Enum.ToObject(typeof(rF2PitState), -255);
      internal rF2GamePhase mIndividualPhase = (rF2GamePhase)Enum.ToObject(typeof(rF2GamePhase), -255);
      internal rF2PrimaryFlag mFlag = (rF2PrimaryFlag)Enum.ToObject(typeof(rF2PrimaryFlag), -255);
      internal byte mUnderYellow = 255;
      internal rF2CountLapFlag mCountLapFlag = (rF2CountLapFlag)Enum.ToObject(typeof(rF2CountLapFlag), -255);
      internal byte mInGarageStall = 255;
      internal rF2FinishStatus mFinishStatus = (rF2FinishStatus)Enum.ToObject(typeof(rF2FinishStatus), -255);
      internal int mLapNumber = -255;
      internal short mTotalLaps = -255;
      internal int mMaxLaps = -1;
      internal int mNumVehicles = -1;
      internal byte mScheduledStops = 255;
      internal byte mHeadlights = 255;
      internal byte mSpeedLimiter = 255;
      internal byte mFrontTireCompoundIndex = 255;
      internal byte mRearTireCompoundIndex = 255;
      internal string mFrontTireCompoundName = "Unknown";
      internal string mRearTireCompoundName = "Unknown";
      internal byte mFrontFlapActivated = 255;
      internal byte mRearFlapActivated = 255;
      internal rF2RearFlapLegalStatus mRearFlapLegalStatus = (rF2RearFlapLegalStatus)Enum.ToObject(typeof(rF2RearFlapLegalStatus), -255);
      internal rF2IgnitionStarterStatus mIgnitionStarter = (rF2IgnitionStarterStatus)Enum.ToObject(typeof(rF2IgnitionStarterStatus), -255);
      internal byte mSpeedLimiterAvailable = 255;
      internal byte mAntiStallActivated = 255;
      internal byte mStartLight = 255;
      internal byte mNumRedLights = 255;
      internal short mNumPitstops = -255;
      internal short mNumPenalties = -255;
      internal int mLapsBehindNext = -1;
      internal int mLapsBehindLeader = -1;
      internal byte mPlayerHeadlights = 255;
      internal byte mServerScored = 255;
      internal int mQualification = -1;
    }
  }
}
