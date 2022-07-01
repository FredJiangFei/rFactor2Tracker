using rF2SMMonitor.rFactor2Data;

namespace rF2SMMonitor
{
    internal partial class TransitionTracker
  {
        internal class DamageInfo
    {
      internal byte[] mDentSeverity = new byte[8];         // dent severity at 8 locations around the car (0=none, 1=some, 2=more)
      internal double mLastImpactMagnitude = -1.0;   // magnitude of last impact
      internal double mAccumulatedImpactMagnitude = -1.0;   // magnitude of last impact
      internal double mMaxImpactMagnitude = -1.0;   // magnitude of last impact
      internal rF2Vec3 mLastImpactPos;        // location of last impact
      internal double mLastImpactET = -1.0;          // time of last impact
      internal byte mOverheating = 255;            // whether overheating icon is shown
      internal byte mDetached = 255;               // whether any parts (besides wheels) have been detached
      //internal byte mHeadlights = 255;             // whether headlights are on

      internal byte mFrontLeftFlat = 255;                    // whether tire is flat
      internal byte mFrontLeftDetached = 255;                // whether wheel is detached
      internal byte mFrontRightFlat = 255;                    // whether tire is flat
      internal byte mFrontRightDetached = 255;                // whether wheel is detached

      internal byte mRearLeftFlat = 255;                    // whether tire is flat
      internal byte mRearLeftDetached = 255;                // whether wheel is detached
      internal byte mRearRightFlat = 255;                    // whether tire is flat
      internal byte mRearRightDetached = 255;                // whether wheel is detached
    }
  }
}
