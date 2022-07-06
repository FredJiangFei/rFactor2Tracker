
import { rF2ScoringInfo } from "./rF2Models/rF2ScoringInfo";
import { createClient } from 'redis';
import { ScoringInfo } from './models/ScoringInfo';
import connectDb from './startup/connectDb';
import mqttClient from './startup/mqttClient';

const db = 'mongodb://localhost/points-scoring';
connectDb({db});

const subscribeTopic = (topic: string) => {
  mqttClient.on('connect', () => {
    console.log('MQTT Connected')
  
    mqttClient.subscribe([topic], () => {
      console.log(`Subscribe to topic '${topic}'`)
    })
  })
  
  mqttClient.on('message', async (topic: string, payload: string) => {
    const scoringInfo: rF2ScoringInfo = JSON.parse(payload);

    const redisClient = createClient();
    await redisClient.connect();

    var key = 'mID' + scoringInfo.mID;
    if(scoringInfo.mGamePhase === 5){
      await redisClient.set(key, scoringInfo.mPlace)
    }

    if(scoringInfo.mGamePhase === 8){
      const startPlace = await redisClient.get(key);
      console.log("startPlace", startPlace);
      console.log("endPlace", scoringInfo.mPlace);

      const si = {
        Id: scoringInfo.mID,
        StartPosition: startPlace,
        Position: scoringInfo.mPlace,
        Score: 100,
        ImprovingStartPosition: true,
        LoosingStartPosition: false
      };

      mqttClient.publish(`/nodejs/mqtt/Scoring/callback`, JSON.stringify(si), { qos: 0, retain: false });

      await ScoringInfo.create(si);
    }
  })
}

subscribeTopic('/nodejs/mqtt/Scoring');
// subscribeTopic('/nodejs/mqtt/Telemetry');