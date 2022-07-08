import { createClient } from 'redis';
import { rF2Telemetry } from '../rF2Models/rF2Telemetry';

const client = createClient();
client.connect().then((value) => console.log('Redis connected'));

let sessionIdCache: string;
let mID: number;
let timerStop: boolean = true;
let onTarmacTime: number = 0;

setInterval(async () => {
    if(timerStop) return;
    onTarmacTime++;

    const cache = await client.hGet(sessionIdCache, mID.toString());
    if (cache === undefined) return;

    const driverCache = JSON.parse(cache);
    client.hSet(sessionIdCache, mID, JSON.stringify({ ...driverCache, OnTarmacTime: onTarmacTime }));
}, 1000);

const calcTimeOnTarmac = async (sessionId: string, telemetry: rF2Telemetry) => {
  if(telemetry.mWheels === undefined || telemetry.mWheels === null || telemetry.mWheels.length === 0) return;

  sessionIdCache = sessionId;
  mID = telemetry.mID;
  const outTarmacWheels = telemetry.mWheels.filter(t => t.mSurfaceType > 1);

  const isSessionEnd = telemetry.mGamePhase === 8;
  timerStop = outTarmacWheels.length !== 0 || isSessionEnd;
};

export default {
    calcTimeOnTarmac
};
