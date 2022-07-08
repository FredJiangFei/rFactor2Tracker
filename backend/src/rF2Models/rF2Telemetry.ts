import { rF2Wheel } from './rF2Wheel';

export interface rF2Telemetry {
  mSession: number;
  mID: number;

  // Game phases:
  // 0 Before session has begun
  // 1 Reconnaissance laps (race only)
  // 2 Grid walk-through (race only)
  // 3 Formation lap (race only)
  // 4 Starting-light countdown has begun (race only)
  // 5 Green flag
  // 6 Full course yellow / safety car
  // 7 Session stopped
  // 8 Session over
  // 9 Paused (tag.2015.09.14 - this is new, and indicates that this is a heartbeat call to the plugin)
  mGamePhase: number;

  mPlace: number;

  mWheels: rF2Wheel[];
}