import tb from 'ts-backend';

const session = {
  Id: "sessionID123",
  Sims: ["SIM-1", "SIM-2"]
};
tb.startSession(session);

