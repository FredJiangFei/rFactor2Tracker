
import { rF2Telemetry } from '../rF2Models/rF2Telemetry';
import redis from '../startup/redis';

const calcTimeOnTarmac = async (sessionId: string, telemetry: rF2Telemetry) => {
  if(telemetry.mWheels === undefined || telemetry.mWheels === null || telemetry.mWheels.length === 0) return;

  const driverCache = await redis.hGet(sessionId, telemetry.mID.toString());

  // TODO: cal OnTarmacTime
  redis.hSet(sessionId, telemetry.mID, JSON.stringify({ ...driverCache, OnTarmacTime: 120 }));
};

const countColision = async (sessionId: string, telemetry: rF2Telemetry) => {
  const noImpact = telemetry.mLastImpactET === 0;
  if(noImpact) return;

  const driverCache = await redis.hGet(sessionId, telemetry.mID.toString());
  const lastImpactETUnchange = driverCache.LastImpactET === telemetry.mLastImpactET;
  if(lastImpactETUnchange) return;

  redis.hSet(sessionId, telemetry.mID, JSON.stringify({ 
    ...driverCache, 
    ColisionsCount: driverCache.ColisionsCount + 1,
    LastImpactET: telemetry.mLastImpactET
  }));
};

const saveSpeed = async (sessionId: string, telemetry: rF2Telemetry) => {
  const driverCache = await redis.hGet(sessionId, telemetry.mID.toString());

  redis.hSet(sessionId, telemetry.mID, JSON.stringify({ 
    ...driverCache, 
    Speeds: [...driverCache.Speeds, telemetry.speed]
  }));
};

export default {
    calcTimeOnTarmac,
    countColision,
    saveSpeed
};
