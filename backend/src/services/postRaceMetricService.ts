import redis from '../startup/redis';
import { rF2Telemetry } from '../rF2Models/rF2Telemetry';
import { Session } from '../models/session';
import { IPoint } from '../models/point';
import * as _ from 'lodash';

const setSessionSatrtPlace = async (
  sessionId: string,
  telemetry: rF2Telemetry
) => {
  const isSessionStart = telemetry.mGamePhase === 5;
  if(!isSessionStart) return;

  const cache = {
    Id: telemetry.mID,
    StartPosition: telemetry.mPlace,
    ColisionsCount: 0
  };
  redis.hSet(sessionId, telemetry.mID, JSON.stringify(cache));
};

const setSessionEndPlace = async (
  sessionId: string,
  telemetry: rF2Telemetry,
  sessionEndCallBack: Function
) => {
  const isSessionEnd = telemetry.mGamePhase === 8;
  if(!isSessionEnd) return;

  const cache = await redis.hGet(sessionId, telemetry.mID.toString());
  if (cache === undefined) return;

  const driverCache = JSON.parse(cache);
  const endPosition = telemetry.mPlace;

  const finalPoints: IPoint[] = [];

  // const points = [];
  // const onTarmacPoint = Math.floor((driverCache.OnTarmacTime ?? 0) / 30);
  // if(onTarmacPoint > 0) {
  //   points.push({ Point: 1, Count: onTarmacPoint, Reason: 'Total Time on Tarmac' })
  // }
  // console.log(dirvers);

  redis.hSet(sessionId, telemetry.mID, JSON.stringify({
    ...driverCache,
    EndPosition: endPosition,
    ImprovingStartPosition: driverCache.StartPosition > endPosition,
    LoosingStartPosition: driverCache.StartPosition < endPosition,
    Points: finalPoints
  }));

  await calcWhenSessionEnd(sessionId, telemetry);

  if (sessionEndCallBack) sessionEndCallBack(finalPoints);
};

const calcWhenSessionEnd = async (sessionId: string, telemetry: rF2Telemetry) => {
  const isLastDriver = telemetry.mID === 1;

  if (isLastDriver) {
    const sessionCache = await redis.hGetAll(sessionId);
    const dirvers = Object.values(sessionCache).map((val) => JSON.parse(val));

    const result =_.sortBy(dirvers, d => d.ColisionsCount);
    result[0].IsFewestColisions = true;
    
    const session = { Id: sessionId, Drivers: result };
    await Session.create(session);
  }
}

export default {
    setSessionSatrtPlace,
    setSessionEndPlace,
};
