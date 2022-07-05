using MQTTnet;
using MQTTnet.Client;
using rF2SMMonitor.rFactor2Data;
using Newtonsoft.Json;

namespace MQTT
{
  public class MQTTTest
  {
      public async Task Send(rF2Telemetry elemetry){
        var mattFactory = new MqttFactory();
        var client = mattFactory.CreateMqttClient();
        var builder = new MqttClientOptionsBuilder()
                    .WithTcpServer("broker.emqx.io", 1883)
                    .WithCleanSession()
                    .Build();

        await client.ConnectAsync(builder);
        string json = JsonConvert.SerializeObject(elemetry);
        var message = new MqttApplicationMessageBuilder()
                    .WithTopic("/nodejs/mqtt/Telemetry")
                    .WithPayload(json)
                    .Build();
        await client.PublishAsync(message);
        await client.DisconnectAsync();
    }
  }
}