using System.Text;
using F1ArcadeOverlay.Models;
using MQTT;
using Newtonsoft.Json;
using rF2SMMonitor.rFactor2Data;
using static rF2SMMonitor.rFactor2Constants;

namespace rF2SMMonitor
{
    internal partial class TransitionTracker
    {
        MqttSender sender;

        public TransitionTracker()
        {
            sender = new MqttSender();
            sender.ConnectAsync();
        }

        public void TrackTelemetry(ref rF2Scoring scoring, ref rF2Telemetry telemetry)
        {
            if (scoring.mScoringInfo.mNumVehicles == 0) return;

            var idsToTelIndices = new Dictionary<long, int>();
            for (int i = 0; i < telemetry.mNumVehicles; ++i)
            {
                if (!idsToTelIndices.ContainsKey(telemetry.mVehicles[i].mID))
                    idsToTelIndices.Add(telemetry.mVehicles[i].mID, i);
            }

            var playerVeh = GetPlayerScoring(ref scoring);
            if (playerVeh.mIsPlayer != 1)
                return;

            var scoringPlrId = playerVeh.mID;
            if (!idsToTelIndices.ContainsKey(scoringPlrId))
                return;

            var resolvedIdx = idsToTelIndices[scoringPlrId];
            var playerVehTelemetry = telemetry.mVehicles[resolvedIdx];

            var speed = Math.Sqrt((playerVeh.mLocalVel.x * playerVeh.mLocalVel.x)
               + (playerVeh.mLocalVel.y * playerVeh.mLocalVel.y)
               + (playerVeh.mLocalVel.z * playerVeh.mLocalVel.z));

            WriteFiles("rF2Scoring", scoring);
            WriteFiles("rF2Telemetry", telemetry);

            var trackTelemetry = new TrackTelemetryModel
            {
                SessionId = scoring.mScoringInfo.mSession,
                DriverId = playerVeh.mID,
                Place = playerVeh.mPlace,
                GamePhase = (rF2GamePhase)scoring.mScoringInfo.mGamePhase,
                LastImpactET = playerVehTelemetry.mLastImpactET,
                Wheels = playerVehTelemetry.mWheels.Select(x => new TrackTelemetryWheel
                {
                    SurfaceType = x.mSurfaceType,
                    Wear = x.mWear
                }).ToArray(),
                Speed = speed,
                Fuel = playerVehTelemetry.mFuel,
                EngineOilTemp = playerVehTelemetry.mEngineOilTemp
            };
            WriteFiles("TrackTelemetryModel", trackTelemetry);
            var topic = "SIM-1";
            sender.Send(topic, trackTelemetry).GetAwaiter();
        }

        public async Task SendFileData(string topic, rF2MqttModel scoring)
        {
           await sender.Send(topic, scoring);
        }

        private rF2VehicleScoring GetPlayerScoring(ref rF2Scoring scoring)
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
   
   
        
        private readonly string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\logs";
        static int elemetryNum = 0;

        private void WriteFiles<T>(string category, T data)
        {
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            string path = $"{basePath}\\${category}_{elemetryNum}.log";
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var fileBytes = Encoding.Default.GetBytes(json);
            using (var stream = File.Create(path))
            {
                stream.Write(fileBytes, 0, fileBytes.Length);
            }
            elemetryNum++;
        }
    }
}
