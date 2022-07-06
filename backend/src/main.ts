import mqtt from 'mqtt';
import { rF2ScoringInfo } from "./rF2Models/rF2ScoringInfo";
import { createClient } from 'redis';
import { ScoringInfo } from './models/ScoringInfo';
import connectDb from './startup/connectDb';

const db = 'mongodb://localhost/points-scoring';
connectDb({db});

const host = 'broker.emqx.io'
const port = '1883'
const clientId = `mqtt_${Math.random().toString(16).slice(3)}`
const connectUrl = `mqtt://${host}:${port}`

const client = mqtt.connect(connectUrl, {
  clientId,
  clean: true,
  connectTimeout: 4000,
  username: 'emqx',
  password: 'public',
  reconnectPeriod: 1000,
})

const subscribeTopic = (topic: string) => {
  client.on('connect', () => {
    console.log('Connected')
  
    client.subscribe([topic], () => {
      console.log(`Subscribe to topic '${topic}'`)
    })
  })
  
  client.on('message', async (topic: string, payload: string) => {
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

      await ScoringInfo.create({
        Id: scoringInfo.mID,
        StartPosition: startPlace,
        Position: scoringInfo.mPlace,
        Score: 100,
        ImprovingStartPosition: true,
        LoosingStartPosition: false
      });
    }
  })
}

subscribeTopic('/nodejs/mqtt/Scoring');
// subscribeTopic('/nodejs/mqtt/Telemetry');