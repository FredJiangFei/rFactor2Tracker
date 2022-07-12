namespace F1ArcadeOverlay.Models
{
    public class TrackTelemetryWheel
    {
        // 0=dry, 1=wet, 2=grass, 3=dirt, 4=gravel, 5=rumblestrip, 6=special
        public int SurfaceType { get; set; }

        public double Wear { get; set; }
    }
}
