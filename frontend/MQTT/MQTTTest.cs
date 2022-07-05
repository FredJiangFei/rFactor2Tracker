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

      public async Task Send(rF2Telemetry elemetry){
      if (!Directory.Exists(basePath))
        Directory.CreateDirectory(basePath);


        var mattFactory = new MqttFactory();
        var client = mattFactory.CreateMqttClient();
        var builder = new MqttClientOptionsBuilder()
                    .WithTcpServer("broker.emqx.io", 1883)
                    .WithCleanSession()
                    .Build();
        await client.ConnectAsync(builder);

        var index = 0;
        foreach (var vehicle in elemetry.mVehicles)
        {
          string path = $"{basePath}\\Telemetry_{elemetryNum}_{index}.log";

          string json = JsonConvert.SerializeObject(vehicle);
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
  }
}