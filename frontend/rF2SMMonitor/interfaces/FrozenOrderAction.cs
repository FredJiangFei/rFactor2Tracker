namespace rF2SMMonitor
{
    internal partial class TransitionTracker
  {
        public enum FrozenOrderAction
    {
      None,
      Follow,
      CatchUp,
      AllowToPass,
      StayInPole,  // Case of being assigned pole/pole row with no SC present (Rolling start in rF2 Karts, for example).
      MoveToPole  // Case of falling behind assigned pole/pole row with no SC present (Rolling start in rF2 Karts, for example).
    }
  }
}
