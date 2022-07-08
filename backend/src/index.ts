import connectDb from './startup/connectDb';
import { Session } from './sessionManagement/session';
import mqttClient from './startup/mqttClient';
import { rF2ScoringInfo } from './rF2Models/rF2ScoringInfo';
import rF2ScoringService from './rF2Models/rF2ScoringService';

connectDb();

const startSession = (session: Session) => {
  const mqtt = mqttClient.connect(session.Sims);

  mqtt.on('message', async (topic: string, payload: string) => {
    const scoringInfo: rF2ScoringInfo = JSON.parse(payload);
    rF2ScoringService.setSessionSatrtPlace(session.Id, scoringInfo);

    rF2ScoringService.setSessionEndPlace(
      session.Id,
      scoringInfo,
      (points: any[]) => {
        mqtt.publish(`${topic}/callback`, JSON.stringify(points));
      }
    );
  });
};

const session: Session = {
  Id: 'sessionID123',
  Sims: ['SIM-1', 'SIM-2'],
};
startSession(session);

export default { startSession };
