namespace rF2SMMonitor
{
    internal partial class TransitionTracker
  {
        public class FrozenOrderData
    {
      public FrozenOrderPhase Phase = FrozenOrderPhase.None;
      public FrozenOrderAction Action = FrozenOrderAction.None;

      // If column is assigned, p1 and p2 follows SC.  Otherwise,
      // only p1 follows SC.
      public int AssignedPosition = -1;

      public FrozenOrderColumn AssignedColumn = FrozenOrderColumn.None;
      // Only matters if AssignedColumn != None
      public int AssignedGridPosition = -1;

      public string DriverToFollow = "";

      // Meters/s.  If -1, SC either left or not present.
      public float SafetyCarSpeed = -1.0f;
    }
  }
}
