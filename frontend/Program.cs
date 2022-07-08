// See https://aka.ms/new-console-template for more information
using MQTT;
using rF2SMMonitor;
using rF2SMMonitor.rFactor2Data;

MappedBuffer<rF2Scoring> scoringBuffer = new MappedBuffer<rF2Scoring>(rFactor2Constants.MM_SCORING_FILE_NAME, true /*partial*/, true /*skipUnchanged*/);
rF2Scoring scoring = new rF2Scoring();

bool connected = true;

await TestScoring("SIM-1");
await TestScoring("SIM-2");

async Task TestScoring(string directory){
    var mqttTest = new MQTTTest();
    string[] allfiles = Directory.GetFiles(directory);
    Array.Sort(allfiles, StringComparer.InvariantCulture);
    foreach (var file in allfiles)
    {
        string json = File.ReadAllText(file);
        var sc = Newtonsoft.Json.JsonConvert.DeserializeObject<rF2MqttModel>(json);
        await mqttTest.TrackScoring(directory, sc);
    }
}

// await Connect();
async Task Connect()
{
    // while (!connected)
    // {
    //     try
    //     {
    //         scoringBuffer.Connect();
    //         connected = true;
    //     }
    //     catch (Exception)
    //     {
    //         Disconnect();
    //     }
    // }

    while (connected)
    {
        // scoringBuffer.GetMappedData(ref scoring);
        // var mqttTest = new MQTTTest();
        // await mqttTest.SendScoring("telemetry/SIM-1", scoring);
        await TestScoring("SIM-1");
        await TestScoring("SIM-2");
    }
}

//  var mqttTest = new MQTTTest();

//         await mqttTest.TrackScoring("SIM-1", new rF2Scoring {
//             mScoringInfo = new rF2ScoringInfo{
//                 mSession = 0,
//                 mGamePhase = 5,
//                 mNumVehicles = 1
//             },
//             mVehicles = new rF2VehicleScoring[]{
//                 new rF2VehicleScoring {
//                     mID = 0,
//                     mTotalLaps = 2,
//                     mSector = 1,
//                     mFinishStatus = 0,
//                     mIsPlayer = 1,
//                     mControl = 1,
//                     mPlace = 6
//                 }
//             }
//         });

//         await mqttTest.TrackScoring("SIM-1", new rF2Scoring {
//             mScoringInfo = new rF2ScoringInfo{
//                 mSession = 0,
//                 mGamePhase = 8,
//                 mNumVehicles = 1
//             },
//             mVehicles = new rF2VehicleScoring[]{
//                 new rF2VehicleScoring {
//                     mID = 0,
//                     mTotalLaps = 2,
//                     mSector = 1,
//                     mFinishStatus = 0,
//                     mIsPlayer = 1,
//                     mControl = 1,
//                     mPlace = 3
//                 }
//             }
//         });


//         await mqttTest.TrackScoring("SIM-2", new rF2Scoring {
//             mScoringInfo = new rF2ScoringInfo{
//                 mSession = 0,
//                 mGamePhase = 3,
//                 mNumVehicles = 1
//             },
//             mVehicles = new rF2VehicleScoring[]{
//                 new rF2VehicleScoring {
//                     mID = 1,
//                     mTotalLaps = 2,
//                     mSector = 1,
//                     mFinishStatus = 0,
//                     mIsPlayer = 1,
//                     mControl = 1,
//                     mPlace = 7
//                 }
//             }
//         });

//         await mqttTest.TrackScoring("SIM-2", new rF2Scoring {
//             mScoringInfo = new rF2ScoringInfo{
//                 mSession = 0,
//                 mGamePhase = 8,
//                 mNumVehicles = 1
//             },
//             mVehicles = new rF2VehicleScoring[]{
//                 new rF2VehicleScoring {
//                     mID = 1,
//                     mTotalLaps = 2,
//                     mSector = 1,
//                     mFinishStatus = 0,
//                     mIsPlayer = 1,
//                     mControl = 1,
//                     mPlace = 10
//                 }
//             }
//         });

void Disconnect()
{
    scoringBuffer.Disconnect();
    connected = false;
}