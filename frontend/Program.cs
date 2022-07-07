// See https://aka.ms/new-console-template for more information
using MQTT;
using rF2SMMonitor;
using rF2SMMonitor.rFactor2Data;

MappedBuffer<rF2Scoring> scoringBuffer = new MappedBuffer<rF2Scoring>(rFactor2Constants.MM_SCORING_FILE_NAME, true /*partial*/, true /*skipUnchanged*/);
rF2Scoring scoring = new rF2Scoring();

bool connected = false;

await Test("SIM-1");
await Test("SIM-2");

async Task Test(string directory){
    var mqttTest = new MQTTTest();

    string[] allfiles = Directory.GetFiles(directory);
    Array.Sort(allfiles, StringComparer.InvariantCulture);
    foreach (var file in allfiles)
    {
        string json = File.ReadAllText(file);
        var sc = Newtonsoft.Json.JsonConvert.DeserializeObject<rF2Scoring>(json);
        await mqttTest.SendScoring(directory, sc);
    }
}

async Task Connect()
{
    while (!connected)
    {
        try
        {
            scoringBuffer.Connect();
            connected = true;
        }
        catch (Exception)
        {
            Disconnect();
        }
    }

    while (connected)
    {
        scoringBuffer.GetMappedData(ref scoring);

        
        var mqttTest = new MQTTTest();
        await mqttTest.SendScoring("telemetry/SIM-1", scoring);
    }
}

void Disconnect()
{
    scoringBuffer.Disconnect();
    connected = false;
}