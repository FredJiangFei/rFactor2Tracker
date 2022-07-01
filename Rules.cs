using rF2SMMonitor.rFactor2Data;
using static rF2SMMonitor.rFactor2Constants;

namespace rF2SMMonitor
{
    internal partial class TransitionTracker
  {
        internal class Rules
    {
      public rF2TrackRulesStage mStage = (rF2TrackRulesStage)Enum.ToObject(typeof(rF2TrackRulesStage), -255);
      public rF2TrackRulesColumn mPoleColumn = (rF2TrackRulesColumn)Enum.ToObject(typeof(rF2TrackRulesColumn), -255);      // column assignment where pole position seems to be located
      public int mNumActions = -1;                     // number of recent actions
      public int mNumParticipants = -1;                // number of participants (vehicles)

      public byte mYellowFlagDetected = 255;             // whether yellow flag was requested or sum of participant mYellowSeverity's exceeds mSafetyCarThreshold
      public byte mYellowFlagLapsWasOverridden = 255;    // whether mYellowFlagLaps (below) is an admin request

      public byte mSafetyCarExists = 255;                // whether safety car even exists
      public byte mSafetyCarActive = 255;                // whether safety car is active
      public int mSafetyCarLaps = 255;                  // number of laps
      public float mSafetyCarThreshold = -1.0f;            // the threshold at which a safety car is called out (compared to the sum of TrackRulesParticipantV01::mYellowSeverity for each vehicle)
      public double mSafetyCarLapDist;             // safety car lap distance
      public float mSafetyCarLapDistAtStart;       // where the safety car starts from

      public float mPitLaneStartDist = -1.0f;              // where the waypoint branch to the pits breaks off (this may not be perfectly accurate)
      public float mTeleportLapDist = -1.0f;               // the front of the teleport locations (a useful first guess as to where to throw the green flag)

      // input/output
      public sbyte mYellowFlagState = 127;         // see ScoringInfoV01 for values
      public short mYellowFlagLaps = 127;                // suggested number of laps to run under yellow (may be passed in with admin command)

      public rF2SafetyCarInstruction mSafetyCarInstruction = (rF2SafetyCarInstruction)Enum.ToObject(typeof(rF2SafetyCarInstruction), -255);
      public float mSafetyCarSpeed = -1.0f;                // maximum speed at which to drive
      public float mSafetyCarMinimumSpacing = -2.0f;       // minimum spacing behind safety car (-1 to indicate no limit)
      public float mSafetyCarMaximumSpacing = -2.0f;       // maximum spacing behind safety car (-1 to indicate no limit)

      public float mMinimumColumnSpacing = -2.0f;          // minimum desired spacing between vehicles in a column (-1 to indicate indeterminate/unenforced)
      public float mMaximumColumnSpacing = -2.0f;          // maximum desired spacing between vehicles in a column (-1 to indicate indeterminate/unenforced)

      public float mMinimumSpeed = -2.0f;                  // minimum speed that anybody should be driving (-1 to indicate no limit)
      public float mMaximumSpeed = -2.0f;                  // maximum speed that anybody should be driving (-1 to indicate no limit)

      public string mMessage = "unknown";                  // a message for everybody to explain what is going on (which will get run through translator on client machines)

      public short mFrozenOrder = 127;                           // 0-based place when caution came out (not valid for formation laps)
      public short mPlace = 127;                                 // 1-based place (typically used for the initialization of the formation lap track order)
      public float mYellowSeverity = -1.0f;                        // a rating of how much this vehicle is contributing to a yellow flag (the sum of all vehicles is compared to TrackRulesV01::mSafetyCarThreshold)
      public double mCurrentRelativeDistance = -1.0;              // equal to ( ( ScoringInfoV01::mLapDist * this->mRelativeLaps ) + VehicleScoringInfoV01::mLapDist )

      // input/output
      public int mRelativeLaps = -1;                            // current formation/caution laps relative to safety car (should generally be zero except when safety car crosses s/f line); this can be decremented to implement 'wave around' or 'beneficiary rule' (a.k.a. 'lucky dog' or 'free pass')
      public rF2TrackRulesColumn mColumnAssignment = (rF2TrackRulesColumn)Enum.ToObject(typeof(rF2TrackRulesColumn), -255);        // which column (line/lane) that participant is supposed to be in
      public int mPositionAssignment = -1;                      // 0-based position within column (line/lane) that participant is supposed to be located at (-1 is invalid)
      public byte mPitsOpen = 255;                           // whether the rules allow this particular vehicle to enter pits right now

      public double mGoalRelativeDistance = -1.0;                 // calculated based on where the leader is, and adjusted by the desired column spacing and the column/position assignments

      public string mMessage_Participant = "unknown";                  // a message for this participant to explain what is going on (untranslated; it will get run through translator on client machines)
    }
  }
}
