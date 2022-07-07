import { rF2ScoringInfo } from "./rF2ScoringInfo";
import redis from '../startup/redis';
import { Session } from "../models/session";

const setSessionSatrtPlace = async (sessionId: string, scoringInfo: rF2ScoringInfo) => {
    var key = sessionId + '_' + scoringInfo.mID;
    const isSessionStart = scoringInfo.mGamePhase === 5;
    if(isSessionStart){
      await redis.set(key, scoringInfo.mPlace)
    }
}

const setSessionEndPlace = async (sessionId: string, scoringInfo: rF2ScoringInfo) => {
    var key = sessionId + '_' + scoringInfo.mID;

    const isSessionEnd = scoringInfo.mGamePhase === 8;
    if(isSessionEnd){
      const startPlace = await redis.get(key);
      const endPlace = scoringInfo.mPlace;
  
      const session = {
        Id: sessionId,
        Drivers: [
          {
            Id: scoringInfo.mID,
            StartPosition: +startPlace,
            EndPosition: endPlace,
            ImprovingStartPosition: +startPlace > endPlace,
            LoosingStartPosition: +startPlace < endPlace
          }
        ]
      }
      await Session.create(session);
    //   mqtt.publish(`${topic}/callback`, JSON.stringify(session), { qos: 0, retain: false });
    }
}

export default {
    setSessionSatrtPlace,
    setSessionEndPlace
}