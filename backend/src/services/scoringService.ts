import { rF2Telemetry } from '../rF2Models/rF2Telemetry';
import { Session } from '../models/session';
import { createClient } from 'redis';

const client = createClient();
client.connect().then((value) => console.log('Redis connected'));

const setSessionSatrtPlace = async (
  sessionId: string,
  telemetry: rF2Telemetry
) => {
  const isSessionStart = telemetry.mGamePhase === 5;
  if(!isSessionStart) return;

  const cache = {
    StartPosition: telemetry.mPlace,
    Points: []
  };
  client.hSet(sessionId, telemetry.mID, JSON.stringify(cache));
};

const setSessionEndPlace = async (
  sessionId: string,
  telemetry: rF2Telemetry,
  sessionEndCallBack: Function
) => {
  const isSessionEnd = telemetry.mGamePhase === 8;
  if(!isSessionEnd) return;

  const cache = await client.hGet(sessionId, telemetry.mID.toString());
  if (cache === undefined) return;

  const driverCache = JSON.parse(cache);
  const startPosition = +driverCache.StartPosition;
  const endPosition = telemetry.mPlace;

  const points = [
    { Point: 1, Count: 3, Reason: 'Overtaking 3 Driver' },
    { Point: 1, Count: 3, Reason: 'Fastest Start' }
  ];

  const onTarmacPoint = Math.floor((driverCache.OnTarmacTime ?? 0) / 30);
  if(onTarmacPoint > 0) {
    points.push({ Point: 1, Count: onTarmacPoint, Reason: 'On Tarmac' })
  }

  const driver = {
    Id: telemetry.mID,
    StartPosition: startPosition,
    EndPosition: endPosition,
    ImprovingStartPosition: startPosition > endPosition,
    LoosingStartPosition: startPosition < endPosition,
    OnTarmacTime: driverCache.OnTarmacTime ?? 0,
    Points: [...driverCache.Points, ...points]
  };
  client.hSet(sessionId, telemetry.mID, JSON.stringify(driver));

  await calcWhenSessionEnd(sessionId, telemetry);

  if (sessionEndCallBack) sessionEndCallBack(driver.Points);
};


const calcWhenSessionEnd = async (sessionId: string, telemetry: rF2Telemetry) => {
  const isLastDriver = telemetry.mID === 1;
  if (isLastDriver) {
    const sessionCache = await client.hGetAll(sessionId);
    const dirvers = Object.values(sessionCache).map((val) => JSON.parse(val));
    const session = { Id: sessionId, Drivers: dirvers };
    await Session.create(session);
  }
}

export default {
  setSessionSatrtPlace,
  setSessionEndPlace,
};
