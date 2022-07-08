
import { rF2Telemetry } from '../rF2Models/rF2Telemetry';
import redis from '../startup/redis';

const calcTimeOnTarmac = async (sessionId: string, telemetry: rF2Telemetry) => {
  if(telemetry.Wheels === undefined || telemetry.Wheels === null || telemetry.Wheels.length === 0) return;

  const driverCache = await redis.hGet(sessionId, telemetry.DriverId.toString());

  // TODO: cal OnTarmacTime
  redis.hSet(sessionId, telemetry.DriverId, JSON.stringify({ ...driverCache, OnTarmacTime: 120 }));
};

const countColision = async (sessionId: string, telemetry: rF2Telemetry) => {
  const noImpact = telemetry.LastImpactET === 0;
  if(noImpact) return;

  const driverCache = await redis.hGet(sessionId, telemetry.DriverId.toString());
  const lastImpactETUnchange = driverCache.LastImpactET === telemetry.LastImpactET;
  if(lastImpactETUnchange) return;

  redis.hSet(sessionId, telemetry.DriverId, JSON.stringify({ 
    ...driverCache, 
    ColisionsCount: driverCache.ColisionsCount + 1,
    LastImpactET: telemetry.LastImpactET
  }));
};

const saveSpeed = async (sessionId: string, telemetry: rF2Telemetry) => {
  const driverCache = await redis.hGet(sessionId, telemetry.DriverId.toString());

  redis.hSet(sessionId, telemetry.DriverId, JSON.stringify({ 
    ...driverCache, 
    Speeds: [...driverCache.Speeds, telemetry.Speed]
  }));
};

export default {
    calcTimeOnTarmac,
    countColision,
    saveSpeed
};
