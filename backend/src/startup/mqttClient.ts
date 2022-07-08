import mqtt from 'mqtt';

const host = 'broker.hivemq.com';
const port = '1883';
const clientId = `mqtt_${Math.random().toString(16).slice(3)}`;
const connectUrl = `mqtt://${host}:${port}`;

const connect = (topics: string[]) => {
  const client = mqtt.connect(connectUrl, {
    clientId,
    clean: true,
    connectTimeout: 4000,
    username: 'emqx',
    password: 'public',
    reconnectPeriod: 1000,
  });

  client.on('connect', () => {
    console.log('MQTT Connected');
    client.subscribe(topics, () => {
      topics.forEach((t) => console.log(`Subscribe to topic '${t}'`));
    });
  });

  return client;
};

export default {
  connect,
};
