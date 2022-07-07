import { rF2ScoringInfo } from "../rF2Models/rF2ScoringInfo";
import rF2ScoringService from "../rF2Models/rF2ScoringService";
import mqttClient from "../startup/mqttClient";
import { Session } from "./models/session";

const startSession = (session: Session) => {
    const mqtt = mqttClient.connect(session.Sims);
    mqtt.on('message', async (topic: string, payload: string) => {
        console.log(topic);

        const scoringInfo: rF2ScoringInfo = JSON.parse(payload);
        console.log(scoringInfo);

        rF2ScoringService.setSessionSatrtPlace(session.Id, scoringInfo);
        rF2ScoringService.setSessionEndPlace(session.Id, scoringInfo);
        // mqtt.publish(`${topic}/callback`, JSON.stringify(session), { qos: 0, retain: false });
    });
}

export default {
    startSession
}