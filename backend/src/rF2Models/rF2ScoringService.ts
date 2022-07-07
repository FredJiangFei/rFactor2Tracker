import { rF2ScoringInfo } from "./rF2ScoringInfo";
import { Session } from "../models/session";
import { createClient } from 'redis';

const client = createClient();
client.connect().then((value) => console.log("Redis connected"));

const setSessionSatrtPlace = async (sessionId: string, scoringInfo: rF2ScoringInfo) => {
    const isSessionStart = scoringInfo.mGamePhase === 5;
    if(isSessionStart){
      const cache = {
        StartPosition: scoringInfo.mPlace
      }
      client.hSet(sessionId, scoringInfo.mID, JSON.stringify(cache));
    }
}

const setSessionEndPlace = async (sessionId: string, scoringInfo: rF2ScoringInfo, sessionEndCallBack: Function) => {
    const isSessionEnd = scoringInfo.mGamePhase === 8;
    if(isSessionEnd){

      const cache = await client.hGet(sessionId, scoringInfo.mID.toString());
      if(cache === undefined) return;

      const driverCache = JSON.parse(cache);
      const startPosition = +driverCache.StartPosition;

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
      client.hSet(sessionId, scoringInfo.mID, JSON.stringify(driver));

      if(isLastDriver) {
        const sessionCache = await client.hGetAll(sessionId);
        const dirvers = Object.values(sessionCache).map((val) => JSON.parse(val));
        const session = {
          Id: sessionId,
          Drivers: dirvers
        }
        await Session.create(session);
      }
      if(sessionEndCallBack) sessionEndCallBack(driver.Points);
    }
}

export default {
    setSessionSatrtPlace,
    setSessionEndPlace
}