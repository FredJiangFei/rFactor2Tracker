
import redis from './startup/redis';
import connectDb from './startup/connectDb';
import sessionService from "./sessionManagement/sessionService";
import { Session } from './sessionManagement/models/session';

connectDb();
redis.connect();

const session: Session = {
  Id: "sessionID123",
  Sims: ["SIM-1", "SIM-2"]
};
sessionService.startSession(session);