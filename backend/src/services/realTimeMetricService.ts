
import { rF2Telemetry } from '../rF2Models/rF2Telemetry';
import redis from '../startup/redis';

let sessionIdCache: string;
let mID: number;
let timerStop: boolean = true;
let onTarmacTime: number = 0;

setInterval(async () => {
    if(timerStop) return;
    onTarmacTime++;

    const cache = await redis.hGet(sessionIdCache, mID.toString());
    if (cache === undefined) return;

    const driverCache = JSON.parse(cache);
    redis.hSet(sessionIdCache, mID, JSON.stringify({ ...driverCache, OnTarmacTime: onTarmacTime }));
}, 1000);

const calcTimeOnTarmac = async (sessionId: string, telemetry: rF2Telemetry) => {
  if(telemetry.mWheels === undefined || telemetry.mWheels === null || telemetry.mWheels.length === 0) return;
  // sessionIdCache = sessionId;
  // mID = telemetry.mID;
  const outTarmacWheels = telemetry.mWheels.filter(t => t.mSurfaceType > 1);
  const isSessionEnd = telemetry.mGamePhase === 8;
  timerStop = outTarmacWheels.length !== 0 || isSessionEnd;
};

const countColision = async (sessionId: string, telemetry: rF2Telemetry) => {
  const noImpact = telemetry.mLastImpactET === 0;
  if(noImpact) return;

  const cache = await redis.hGet(sessionId, telemetry.mID.toString());
  if (cache === undefined) return;

  const driverCache = JSON.parse(cache);
  const lastImpactETUnchange = driverCache.LastImpactET === telemetry.mLastImpactET;
  if(lastImpactETUnchange) return;

  await redis.hSet(sessionId, telemetry.mID, JSON.stringify({ 
    ...driverCache, 
    ColisionsCount: driverCache.ColisionsCount + 1,
    LastImpactET: telemetry.mLastImpactET
  }));
};

export default {
    calcTimeOnTarmac,
    countColision
};
