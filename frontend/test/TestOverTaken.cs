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
        
        public async Task OverTakenOneDriverWithAllPlaces()
        {
            await sender.ConnectAsync(new string[] { "SIM-1", "SIM-2" });

            await Send(new TrackTelemetryDriver[]{
                new TrackTelemetryDriver{ DriverId = 1, Place = 2 },
                new TrackTelemetryDriver{ DriverId = 2, Place = 1 },
            });

            Thread.Sleep(1000);

            await Send(new TrackTelemetryDriver[]{
                new TrackTelemetryDriver{ DriverId = 1, Place = 1 },
                new TrackTelemetryDriver{ DriverId = 2, Place = 2 },
            });

            Thread.Sleep(6000);

            await Send(new TrackTelemetryDriver[]{
                new TrackTelemetryDriver{ DriverId = 1, Place = 1 },
                new TrackTelemetryDriver{ DriverId = 2, Place = 2 },
            }, rF2GamePhase.SessionOver);
        }

        private async Task Send(TrackTelemetryDriver[] xxx, rF2GamePhase phase = rF2GamePhase.GreenFlag){
            foreach (var x in xxx)
            {
                var d = GetData(x.DriverId, x.Place, phase);
                d.Drivers = xxx;
                await sender.Send($"SIM-{x.DriverId}", d);
            }
        }

        public async Task OverTakenOneDriverAndThenOverTakenTwoAgain()
        {
            await sender.ConnectAsync(new string[] { "SIM-1" });

            await sender.Send("SIM-1", GetData(1, 6, rF2GamePhase.GreenFlag));

            Thread.Sleep(1000);

            await sender.Send("SIM-1", GetData(1, 5, rF2GamePhase.GreenFlag));

            Thread.Sleep(6000);

            await sender.Send("SIM-1", GetData(1, 3, rF2GamePhase.GreenFlag));

            Thread.Sleep(6000);

            await sender.Send("SIM-1", GetData(1, 3, rF2GamePhase.SessionOver));
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

        public async Task DropsDownFromOriginalPlaceWhenIOvertaking()
        {
            sender = new MqttSender();
            await sender.ConnectAsync(new string[] { "SIM-1" });

            await sender.Send("SIM-1", GetData(1, 3, rF2GamePhase.GreenFlag));

            Thread.Sleep(1000);

            await sender.Send("SIM-1", GetData(1, 2, rF2GamePhase.GreenFlag));

            Thread.Sleep(3000);

            await sender.Send("SIM-1", GetData(1, 4, rF2GamePhase.SessionOver));
        }

        public async Task NothingHappendWhenDropsDownFromOriginalPlaceButNotOvertaking()
        {
            sender = new MqttSender();
            await sender.ConnectAsync(new string[] { "SIM-1" });

            await sender.Send("SIM-1", GetData(1, 3, rF2GamePhase.GreenFlag));

            Thread.Sleep(3000);

            await sender.Send("SIM-1", GetData(1, 4, rF2GamePhase.SessionOver));
        }

        public async Task DropsDownAndThenOvertaking()
        {
            sender = new MqttSender();
            await sender.ConnectAsync(new string[] { "SIM-1" });

            await sender.Send("SIM-1", GetData(1, 3, rF2GamePhase.GreenFlag));

            Thread.Sleep(3000);

            await sender.Send("SIM-1", GetData(1, 4, rF2GamePhase.GreenFlag));

            Thread.Sleep(3000);

            await sender.Send("SIM-1", GetData(1, 1, rF2GamePhase.GreenFlag));

             Thread.Sleep(6000);

            await sender.Send("SIM-1", GetData(1, 1, rF2GamePhase.SessionOver));
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