// See https://aka.ms/new-console-template for more information
using MQTT;
using rF2SMMonitor;
using rF2SMMonitor.rFactor2Data;
using Test;

MappedBuffer<rF2Telemetry> telemetryBuffer = new MappedBuffer<rF2Telemetry>(rFactor2Constants.MM_TELEMETRY_FILE_NAME, true /*partial*/, true /*skipUnchanged*/);
MappedBuffer<rF2Scoring> scoringBuffer = new MappedBuffer<rF2Scoring>(rFactor2Constants.MM_SCORING_FILE_NAME, true /*partial*/, true /*skipUnchanged*/);

// Marshalled views:
rF2Telemetry telemetry = new rF2Telemetry();
rF2Scoring scoring = new rF2Scoring();
TransitionTracker tracker = new TransitionTracker();

bool connected = true;

// while (!connected)
// {
//     try
//     {
//         telemetryBuffer.Connect();
//         scoringBuffer.Connect();
//         connected = true;
//     }
//     catch (Exception)
//     {
//         Disconnect();
//     }
// }

// while (true)
// {
//     try
//     {
//         MainUpdate();

//         Thread.Sleep(3000);
//         tracker.TrackTelemetry(ref scoring, ref telemetry);
//     }
//     catch (Exception)
//     {
//         Disconnect();
//     }
// }


while(true)
{
    var test = new TestOverTaken();
    await test.OverTakenOneDriverWithAllPlaces();
}

void MainUpdate()
{
    if (!connected)
    {

        telemetryBuffer.Connect();
        scoringBuffer.Connect();
        connected = true;
        return;
    }

    try
    {
        telemetryBuffer.GetMappedData(ref telemetry);
        scoringBuffer.GetMappedData(ref scoring);
    }
    catch (Exception)
    {
        Disconnect();
    }
}


void Disconnect()
{
    telemetryBuffer.Disconnect();
    scoringBuffer.Disconnect();
    connected = false;
}


// await SendTopicFile("SIM-1", "log1.log");
// await SendTopicFile("SIM-2", "log1.log");
// await SendTopicFile("SIM-1", "log2.log");
// await SendTopicFile("SIM-1", "log10.log");
// await SendTopicFile("SIM-2", "log2.log");

async Task SendDirectoryFiles(string directory){

    string[] allfiles = Directory.GetFiles(directory);
    Array.Sort(allfiles, StringComparer.InvariantCulture);

    foreach (var file in allfiles)
    {
        await SendTopicFile(directory, file);
    }
}

async Task SendTopicFile(string topic, string file){
    
    var filePath = $"./{topic}/{file}";

    var tracker = new TransitionTracker();
    string json = File.ReadAllText(filePath);
    var sc = Newtonsoft.Json.JsonConvert.DeserializeObject<rF2MqttModel>(json);
    await tracker.SendFileData(topic, sc);
}