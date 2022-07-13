using F1ArcadeOverlay.Models;
using MQTT;
using static rF2SMMonitor.rFactor2Constants;

namespace Test
{
    public class TestOverTaken
    {
        public MqttSender sender;

        public TestOverTaken(){
            sender = new MqttSender();
        }

        public async Task OverTakenOneDriver()
        {
            await sender.ConnectAsync(new string[] { "SIM-1" });

            await sender.Send("SIM-1", GetData(1, 2, rF2GamePhase.GreenFlag));

            Thread.Sleep(1000);

            await sender.Send("SIM-1", GetData(1, 1, rF2GamePhase.GreenFlag));

            Thread.Sleep(6000);

            await sender.Send("SIM-1", GetData(1, 1, rF2GamePhase.SessionOver));
        }

        public async Task OverTakenTwoDriver()
        {
            await sender.ConnectAsync(new string[] { "SIM-1" });

            await sender.Send("SIM-1", GetData(1, 3, rF2GamePhase.GreenFlag));

            Thread.Sleep(1000);

            await sender.Send("SIM-1", GetData(1, 1, rF2GamePhase.GreenFlag));

            Thread.Sleep(6000);

            await sender.Send("SIM-1", GetData(1, 1, rF2GamePhase.SessionOver));
        }

        public async Task OverTakenTwoDriversAndOverOneAgain()
        {
            await sender.ConnectAsync(new string[] { "SIM-1" });

            await sender.Send("SIM-1", GetData(1, 4, rF2GamePhase.GreenFlag));

            Thread.Sleep(1000);

            await sender.Send("SIM-1", GetData(1, 2, rF2GamePhase.GreenFlag));

            Thread.Sleep(3000);

            await sender.Send("SIM-1", GetData(1, 1, rF2GamePhase.GreenFlag));

            Thread.Sleep(6000);

            await sender.Send("SIM-1", GetData(1, 1, rF2GamePhase.SessionOver));
        }

        public async Task DropsDownFromOriginalPlace()
        {
            sender = new MqttSender();
            await sender.ConnectAsync(new string[] { "SIM-1" });

            await sender.Send("SIM-1", GetData(1, 3, rF2GamePhase.GreenFlag));

            Thread.Sleep(1000);

            await sender.Send("SIM-1", GetData(1, 2, rF2GamePhase.GreenFlag));

            Thread.Sleep(3000);

            await sender.Send("SIM-1", GetData(1, 4, rF2GamePhase.GreenFlag));
        }

        private TrackTelemetryModel GetData(int driverId, byte place, rF2GamePhase gamePhase)
        {
            var trackTelemetry = new TrackTelemetryModel
            {
                SessionId = 1,
                DriverId = driverId,
                Place = place,
                GamePhase = gamePhase
            };

            return trackTelemetry;
        }
    }
}