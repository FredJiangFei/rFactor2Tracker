// See https://aka.ms/new-console-template for more information
using MQTT;
using rF2SMMonitor;
using rF2SMMonitor.rFactor2Data;

MappedBuffer<rF2Telemetry> telemetryBuffer = new MappedBuffer<rF2Telemetry>(rFactor2Constants.MM_TELEMETRY_FILE_NAME, true /*partial*/, true /*skipUnchanged*/);
MappedBuffer<rF2Scoring> scoringBuffer = new MappedBuffer<rF2Scoring>(rFactor2Constants.MM_SCORING_FILE_NAME, true /*partial*/, true /*skipUnchanged*/);

rF2Telemetry telemetry = new rF2Telemetry();
rF2Scoring scoring = new rF2Scoring();




bool connected = false;

while (!connected)
{
    try
    {
        // telemetryBuffer.Connect();
        scoringBuffer.Connect();
        connected = true;
    }
    catch (Exception)
    {
        Disconnect();
    }
}

// while (connected)
// {
//     telemetryBuffer.GetMappedData(ref telemetry);
//     var mqttTest = new MQTTTest();
//     await mqttTest.SendTelemetry(telemetry);
// }

while (connected)
{
    scoringBuffer.GetMappedData(ref scoring);

    
    var mqttTest = new MQTTTest();
    await mqttTest.SendScoring(scoring);
}

void Disconnect()
{
    telemetryBuffer.Disconnect();
    connected = false;
}