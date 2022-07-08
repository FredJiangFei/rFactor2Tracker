import redis from '../startup/redis';
import { rF2Telemetry } from '../rF2Models/rF2Telemetry';
import { Session } from '../models/session';
import * as _ from 'lodash';

const setSessionSatrtPlace = async (
  topic: string,
  sessionId: string,
  telemetry: rF2Telemetry
) => {
  const isSessionStart = telemetry.mGamePhase === 5;
  if(!isSessionStart) return;

  const cache = {
    Topic: topic,
    Id: telemetry.mID,
    OnTarmacTime: 0,
    StartPosition: telemetry.mPlace,
    ColisionsCount: 0,
    Points: [],
    Speeds: []
  };
  redis.hSet(sessionId, telemetry.mID, JSON.stringify(cache));
};

const setSessionEndPlace = async (
  sessionId: string,
  telemetry: rF2Telemetry,
  sendPointsBack: Function
) => {
  const isSessionEnd = telemetry.mGamePhase === 8;
  if(!isSessionEnd) return;

  const driverCache = await redis.hGet(sessionId, telemetry.mID.toString());

  const endPosition = telemetry.mPlace;

  const speedSum = _.sum(driverCache.Speeds);
  
  redis.hSet(sessionId, telemetry.mID, JSON.stringify({
    ...driverCache,
    EndPosition: endPosition,
    ImprovingStartPosition: driverCache.StartPosition > endPosition,
    LoosingStartPosition: driverCache.StartPosition < endPosition,
    AverageSpeed : speedSum / driverCache.Speeds.length
  }));

  const isLastDriver = telemetry.mID === 1;
  if (isLastDriver) {
    const sessionCache = await redis.client.hGetAll(sessionId);
    const dirvers = Object.values(sessionCache).map((val) => JSON.parse(val));

    const sortByColisions =_.sortBy(dirvers, d => d.ColisionsCount);
    sortByColisions[0].IsFewestColisions = true;
    sortByColisions[0].Points.push({ Point: 1, Count: 1, Reason: 'FewestColisions' })

    const sortBySpeed =_.sortBy(dirvers, d => d.AverageSpeed);
    sortBySpeed[0].IsLowestAverageSpeed = true;
    sortBySpeed[0].Points.push({ Point: 1, Count: 1, Reason: 'LowestAverageSpeed' })

    dirvers.forEach(d => {
      const onTarmacPoint = Math.floor(d.OnTarmacTime / 30);
      if(onTarmacPoint > 0) {
        d.Points.push({ Point: 1, Count: onTarmacPoint, Reason: 'Total Time on Tarmac' })
      }

      sendPointsBack(d.Topic, d.Points);
    });
    
    const session = { Id: sessionId, Drivers: dirvers };
    await Session.create(session);
  }
};

export default {
    setSessionSatrtPlace,
    setSessionEndPlace
};
