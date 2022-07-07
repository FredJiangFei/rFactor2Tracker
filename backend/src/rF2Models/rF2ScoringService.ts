import { rF2ScoringInfo } from "./rF2ScoringInfo";
import { Session } from "../models/session";
import redis from '../startup/redis';

redis.connect();

const setSessionSatrtPlace = async (sessionId: string, scoringInfo: rF2ScoringInfo) => {
    var key = sessionId + '_' + scoringInfo.mID;

    const isSessionStart = scoringInfo.mGamePhase === 5;
    if(isSessionStart){
      
      const driverCache = {
        StartPosition: scoringInfo.mPlace
      }

      await redis.set(key, scoringInfo.mPlace);
    }
}

const setSessionEndPlace = async (sessionId: string, scoringInfo: rF2ScoringInfo) => {
    var key = sessionId + '_' + scoringInfo.mID;

    const isSessionEnd = scoringInfo.mGamePhase === 8;
    if(isSessionEnd){
      const startPosition = +await redis.get(key);
      const endPosition = scoringInfo.mPlace;
      
      const isLastDriver = scoringInfo.mID === 1;

      const driver = {
        Id: scoringInfo.mID,
        StartPosition: startPosition,
        EndPosition: endPosition,
        ImprovingStartPosition: startPosition > endPosition,
        LoosingStartPosition: startPosition < endPosition,
        Points: isLastDriver ? [
          { Amount: 3, Reason: 'Overtaking 3 Driver' },
          { Amount: 1, Reason: 'Fastest Start' }
        ] : [] 
      };   

      if(isLastDriver) {
        const session = {
          Id: sessionId,
          Drivers: [
            {
              Id: 1,
              StartPosition: 3,
              EndPosition: 10,
              ImprovingStartPosition: 0,
              LoosingStartPosition: 1
            },
            driver
          ]
        }
        await Session.create(session);
      }
      //   mqtt.publish(`${topic}/callback`, JSON.stringify(session), { qos: 0, retain: false });
    }
}

export default {
    setSessionSatrtPlace,
    setSessionEndPlace
}