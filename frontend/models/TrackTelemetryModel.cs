
using static rF2SMMonitor.rFactor2Constants;

namespace F1ArcadeOverlay.Models
{
    public class TrackTelemetryModel
    {
        public int SessionId { get; set; }
        public int DriverId { get; set; }
        public byte Place { get; set; }

        // Game phases:
        // 0 Before session has begun
        // 1 Reconnaissance laps (race only)
        // 2 Grid walk-through (race only)
        // 3 Formation lap (race only)
        // 4 Starting-light countdown has begun (race only)
        // 5 Green flag
        // 6 Full course yellow / safety car
        // 7 Session stopped
        // 8 Session over
        // 9 Paused (tag.2015.09.14 - this is new, and indicates that this is a heartbeat call to the plugin)
        public double Speed { get; set; }
        public rF2GamePhase GamePhase { get; set; }
        public double LastImpactET { get; set; }
        public double Fuel { get; set; }
        public double EngineOilTemp { get; set; }
        public TrackTelemetryWheel[] Wheels { get; set; }
        public TrackTelemetryDriver[] Drivers { get; set; }
    }
}
