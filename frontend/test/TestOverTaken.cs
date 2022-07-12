using F1ArcadeOverlay.Models;
using MQTT;
using static rF2SMMonitor.rFactor2Constants;

namespace Test
{
  public class TestOverTaken
  {
    MqttSender sender;

    public async Task OverTakenOneDriver(){
        sender = new MqttSender();
        await sender.ConnectAsync();

        await sender.SendTelemetry("SIM-1", GetData(1, 2,  rF2GamePhase.GreenFlag));
        await sender.SendTelemetry("SIM-2", GetData(2, 1,  rF2GamePhase.GreenFlag));

        Thread.Sleep(1000);

        await sender.SendTelemetry("SIM-1", GetData(1, 1,  rF2GamePhase.GreenFlag));
        await sender.SendTelemetry("SIM-2", GetData(2, 2,  rF2GamePhase.GreenFlag));

        Thread.Sleep(6000);

        await sender.SendTelemetry("SIM-1", GetData(1, 1,  rF2GamePhase.SessionOver));
        await sender.SendTelemetry("SIM-2", GetData(2, 2,  rF2GamePhase.SessionOver));
    }

    public async Task OverTakenTwoDriver(){
        sender = new MqttSender();
        await sender.ConnectAsync();

        await sender.SendTelemetry("SIM-1", GetData(1, 3,  rF2GamePhase.GreenFlag));
        await sender.SendTelemetry("SIM-2", GetData(2, 1,  rF2GamePhase.GreenFlag));
        await sender.SendTelemetry("SIM-3", GetData(3, 2,  rF2GamePhase.GreenFlag));

        Thread.Sleep(1000);

        await sender.SendTelemetry("SIM-1", GetData(1, 1,  rF2GamePhase.GreenFlag));
        await sender.SendTelemetry("SIM-2", GetData(2, 2,  rF2GamePhase.GreenFlag));
        await sender.SendTelemetry("SIM-3", GetData(3, 3,  rF2GamePhase.GreenFlag));

        Thread.Sleep(6000);

        await sender.SendTelemetry("SIM-1", GetData(1, 1,  rF2GamePhase.SessionOver));
        await sender.SendTelemetry("SIM-2", GetData(2, 2,  rF2GamePhase.SessionOver));
        await sender.SendTelemetry("SIM-3", GetData(3, 3,  rF2GamePhase.SessionOver));
    }

     public async Task OverTakenTwoDriversAndOverOneAgain(){
        sender = new MqttSender();
        await sender.ConnectAsync();

        await sender.SendTelemetry("SIM-1", GetData(1, 4,  rF2GamePhase.GreenFlag));
        await sender.SendTelemetry("SIM-2", GetData(2, 3,  rF2GamePhase.GreenFlag));
        await sender.SendTelemetry("SIM-3", GetData(3, 2,  rF2GamePhase.GreenFlag));
        await sender.SendTelemetry("SIM-4", GetData(4, 1,  rF2GamePhase.GreenFlag));

        Thread.Sleep(1000);

        await sender.SendTelemetry("SIM-1", GetData(1, 2,  rF2GamePhase.GreenFlag));
        await sender.SendTelemetry("SIM-2", GetData(2, 3,  rF2GamePhase.GreenFlag));
        await sender.SendTelemetry("SIM-3", GetData(3, 4,  rF2GamePhase.GreenFlag));
        await sender.SendTelemetry("SIM-4", GetData(4, 1,  rF2GamePhase.GreenFlag));

        Thread.Sleep(3000);

        await sender.SendTelemetry("SIM-1", GetData(1, 1,  rF2GamePhase.GreenFlag));
        await sender.SendTelemetry("SIM-2", GetData(2, 2,  rF2GamePhase.GreenFlag));
        await sender.SendTelemetry("SIM-3", GetData(3, 3,  rF2GamePhase.GreenFlag));
        await sender.SendTelemetry("SIM-4", GetData(4, 4,  rF2GamePhase.GreenFlag));

        Thread.Sleep(6000);

        await sender.SendTelemetry("SIM-1", GetData(1, 1,  rF2GamePhase.SessionOver));
        await sender.SendTelemetry("SIM-2", GetData(2, 2,  rF2GamePhase.SessionOver));
        await sender.SendTelemetry("SIM-3", GetData(3, 3,  rF2GamePhase.SessionOver));
        await sender.SendTelemetry("SIM-4", GetData(4, 4,  rF2GamePhase.SessionOver));
    }

    private TrackTelemetryModel GetData(int driverId, byte place, rF2GamePhase gamePhase){
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