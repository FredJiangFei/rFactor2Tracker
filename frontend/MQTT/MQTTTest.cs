using MQTTnet;
using MQTTnet.Client;
using rF2SMMonitor.rFactor2Data;
using Newtonsoft.Json;
using System.Text;
using MQTTnet.Packets;
using rF2SMMonitor;
using F1ArcadeOverlay.Models;

namespace MQTT
{
  public class MqttSender
  {
      private readonly string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\logs";
      static int elemetryNum = 0;

    public async Task<IMqttClient> Connect(){
        var mattFactory = new MqttFactory();
        var client = mattFactory.CreateMqttClient();
        var builder = new MqttClientOptionsBuilder()
                    .WithTcpServer("broker.hivemq.com", 1883)
                    .WithCleanSession()
                    .Build();
        await client.ConnectAsync(builder);
        return client;
    }
    
    public async Task TrackScoring(string topic, rF2MqttModel scoring){
      if (scoring.mScoringInfo.mNumVehicles == 0)
        return;

      var playerVeh = GetPlayerScoring(ref scoring);
      if (playerVeh.mIsPlayer != 1)
        return;

      var pv = scoring.mVehicles.FirstOrDefault(x=>x.mIsPlayer == 1);

      var client = await Connect();
      client.ApplicationMessageReceivedAsync += e =>
      {
          OnApplicationMessageReceived(e);
          return Task.CompletedTask;
      };

      var objSubOptions = new MqttClientSubscribeOptions();
      var objTopics = new List<MqttTopicFilter>{
        new MqttTopicFilter {
          Topic = $"{topic}/callback"
        }
      };
      objSubOptions.TopicFilters = objTopics;
      await client.SubscribeAsync(objSubOptions); 

      var speed = Math.Sqrt((playerVeh.mLocalVel.x * playerVeh.mLocalVel.x)
      + (playerVeh.mLocalVel.y * playerVeh.mLocalVel.y)
      + (playerVeh.mLocalVel.z * playerVeh.mLocalVel.z));

      var info = new { 
        Session = scoring.mScoringInfo.mSession,
        DriverId = pv.mID, 
        Place = pv.mPlace, 
        GamePhase = scoring.mScoringInfo.mGamePhase,
        Wheels = scoring.mWheels?.Select(x=> new { 
          SurfaceType = x.mSurfaceType,
          Wear = x.mWear
        }),
        LastImpactET = scoring.mLastImpactET,
        Speed = speed
      };
     
      await Send(client, topic, info);
      
      elemetryNum++;
        // await client.DisconnectAsync();
    }

    public rF2VehicleScoring GetPlayerScoring(ref rF2MqttModel scoring)
    {
      var playerVehScoring = new rF2VehicleScoring();
      for (int i = 0; i < scoring.mScoringInfo.mNumVehicles; ++i)
      {
        var vehicle = scoring.mVehicles[i];
        switch ((rFactor2Constants.rF2Control)vehicle.mControl)
        {
          case rFactor2Constants.rF2Control.AI:
          case rFactor2Constants.rF2Control.Player:
          case rFactor2Constants.rF2Control.Remote:
            if (vehicle.mIsPlayer == 1)
              playerVehScoring = vehicle;

            break;

          default:
            continue;
        }

        if (playerVehScoring.mIsPlayer == 1)
          break;
      }

      return playerVehScoring;
    }

    void OnApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
    {
      Console.WriteLine(e.ApplicationMessage.Topic);
      Console.WriteLine(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
    }

    private void WriteFile<T>(T data){
      if (!Directory.Exists(basePath))
        Directory.CreateDirectory(basePath);

      string path = $"{basePath}\\log_{elemetryNum}.log";
      string json = JsonConvert.SerializeObject(data, Formatting.Indented);
      var xx = Encoding.Default.GetBytes(json);
      using (var stream = File.Create(path))
      {
        stream.Write(xx, 0, xx.Length);
      }
    }

    public async Task Send<T>(IMqttClient client, string topic, T data){
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(json)
                .Build();
        await client.PublishAsync(message);   
    }

    IMqttClient client = new MqttFactory().CreateMqttClient();

    public async Task ConnectAsync()
    {
        var builder = new MqttClientOptionsBuilder()
                    .WithTcpServer("broker.hivemq.com", 1883)
                    .WithCleanSession()
                    .Build();
        await client.ConnectAsync(builder);
    }

    public async Task SendTelemetry(string topic, TrackTelemetryModel data)
    {
        string json = JsonConvert.SerializeObject(data);
        var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(json)
                    .Build();
        if (client.IsConnected)
        {
            await client.PublishAsync(message);
        }
    }
  }
}