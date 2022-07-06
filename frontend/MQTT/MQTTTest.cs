using MQTTnet;
using MQTTnet.Client;
using rF2SMMonitor.rFactor2Data;
using Newtonsoft.Json;
using System.Text;

namespace MQTT
{
  public class MQTTTest
  {
      private readonly string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\logs";
      static int elemetryNum = 0;

    public async Task<IMqttClient> Connect(){
      if (!Directory.Exists(basePath))
        Directory.CreateDirectory(basePath);

        var mattFactory = new MqttFactory();
        var client = mattFactory.CreateMqttClient();
        var builder = new MqttClientOptionsBuilder()
                    .WithTcpServer("broker.emqx.io", 1883)
                    .WithCleanSession()
                    .Build();
        await client.ConnectAsync(builder);

        return client;
    }
    

      public async Task SendTelemetry(rF2Telemetry elemetry){
        var client = await Connect();
        var index = 0;

        foreach (var vehicle in elemetry.mVehicles)
        {
          string path = $"{basePath}\\Telemetry_{elemetryNum}_{index}.log";
          string json = JsonConvert.SerializeObject(vehicle, Formatting.Indented);
          var xx = Encoding.Default.GetBytes(json);
          using (var stream = File.Create(path))
          {
            stream.Write(xx, 0, xx.Length);
          }

          var message = new MqttApplicationMessageBuilder()
                      .WithTopic("/nodejs/mqtt/Telemetry")
                      .WithPayload(json)
                      .Build();
          await client.PublishAsync(message);

          index++;
        }

        var overMessage = new MqttApplicationMessageBuilder()
                .WithTopic("/nodejs/mqtt/Telemetry")
                .WithPayload("/nodejs/mqtt/Telemetry over")
                .Build();
        await client.PublishAsync(overMessage);
        
        elemetryNum++;
        await client.DisconnectAsync();
    }

    public async Task SendScoring(rF2Scoring scoring){
        if(scoring.mVehicles == null) return;
        var pv = scoring.mVehicles.FirstOrDefault(x=>x.mIsPlayer == 1);

        var client = await Connect();
        var xx = new { mID = pv.mID, mPlace = pv.mPlace, mGamePhase = scoring.mScoringInfo.mGamePhase };

        WriteFile("Scoring", pv);
        await Send(client, "Scoring", xx);
      
        elemetryNum++;
        await client.DisconnectAsync();
    }

    private void WriteFile<T>(string topic, T data){
      string path = $"{basePath}\\{topic}_{elemetryNum}.log";
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
                .WithTopic($"/nodejs/mqtt/{topic}")
                .WithPayload(json)
                .Build();
        await client.PublishAsync(message);   
    }
  }
}