using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using System.Text;
using MQTTnet.Packets;

namespace MQTT
{
    public class MqttSender
    {
        private readonly string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\logs";
        static int elemetryNum = 0;
        IMqttClient client = new MqttFactory().CreateMqttClient(); 

        public bool IsSessionEnd = false;

        public async Task ConnectAsync(string[] topics)
        {
            var builder = new MqttClientOptionsBuilder()
                        .WithTcpServer("broker.hivemq.com", 1883)
                        .WithCleanSession()
                        .Build();
            await client.ConnectAsync(builder);

            client.ApplicationMessageReceivedAsync += e =>
            {
                OnApplicationMessageReceived(e);
                return Task.CompletedTask;
            };

            var objSubOptions = new MqttClientSubscribeOptions();
            var topicFilters = topics.Select(x => new MqttTopicFilter { Topic = $"{x}/callback" }).ToList();
            objSubOptions.TopicFilters = topicFilters;
            await client.SubscribeAsync(objSubOptions);
        }

        public async Task ConnectAsync()
        {
            var builder = new MqttClientOptionsBuilder()
                        .WithTcpServer("broker.hivemq.com", 1883)
                        .WithCleanSession()
                        .Build();
            await client.ConnectAsync(builder);
        }

        public async Task Send<T>(string topic, T data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(json)
                    .Build();
            await client.PublishAsync(message);
        }

        void OnApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            IsSessionEnd = true;
            Console.WriteLine(e.ApplicationMessage.Topic);
            Console.WriteLine(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
        }

        private void WriteFiles<T>(T data)
        {
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            string path = $"{basePath}\\log_{elemetryNum}.log";
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var fileBytes = Encoding.Default.GetBytes(json);
            using (var stream = File.Create(path))
            {
                stream.Write(fileBytes, 0, fileBytes.Length);
            }
        }
    }
}