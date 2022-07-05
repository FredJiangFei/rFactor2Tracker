import mqtt from 'mqtt';

const host = 'broker.emqx.io'
const port = '1883'
const clientId = `mqtt_${Math.random().toString(16).slice(3)}`
const connectUrl = `mqtt://${host}:${port}`

const client = mqtt.connect(connectUrl, {
  clientId,
  clean: true,
  connectTimeout: 4000,
  username: 'emqx',
  password: 'public',
  reconnectPeriod: 1000,
})

const subscribeTopic = (topic: string) => {
  client.on('connect', () => {
    console.log('Connected')
  
    client.subscribe([topic], () => {
      console.log(`Subscribe to topic '${topic}'`)
    })
  })
  
  client.on('message', (topic, payload) => {
    console.log('Received Message:', topic, payload)
  })
}

subscribeTopic('/nodejs/mqtt/Telemetry');