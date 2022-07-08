
import connectDb from './startup/connectDb';
import mqttClient from "./startup/mqttClient";
import { rF2Telemetry } from "./rF2Models/rF2Telemetry";
import realTimeMetricService from './services/realTimeMetricService';
import postRaceMetricService from './services/postRaceMetricService';
import { IPoint } from './models/point';

connectDb();

export interface Session {
  Id: string;
  Sims: string[];
}

const startSession = (session: Session) => {
  const mqtt = mqttClient.connect(session.Sims);

  mqtt.on('message', async (topic: string, payload: string) => {
      const telemetry: rF2Telemetry = JSON.parse(payload);

      postRaceMetricService.setSessionSatrtPlace(topic, session.Id, telemetry);

      // realTimeMetricService.calcTimeOnTarmac(session.Id, telemetry);
      realTimeMetricService.countColision(session.Id, telemetry);
      realTimeMetricService.saveSpeed(session.Id, telemetry);

      postRaceMetricService.setSessionEndPlace(session.Id, telemetry, (t: string, points: IPoint[]) => {
        mqtt.publish(`${t}/callback`, JSON.stringify(points));
      });
  });
}

const session: Session = {
  Id: "sessionID",
  Sims: ["SIM-1", "SIM-2"]
};
startSession(session);