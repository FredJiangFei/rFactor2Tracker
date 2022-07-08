using rF2SMMonitor.rFactor2Data;

public struct rF2MqttModel
{
    public uint mVersionUpdateBegin;          // Incremented right before buffer is written to.
    public uint mVersionUpdateEnd;            // Incremented after buffer write is done.
    public int mBytesUpdatedHint;             // How many bytes of the structure were written during the last update.
                                            // 0 means unknown (whole buffer should be considered as updated).
    public int mLastImpactET;
    public rF2ScoringInfo mScoringInfo;
    public rF2VehicleScoring[] mVehicles;
    public rF2Wheel[] mWheels;
}