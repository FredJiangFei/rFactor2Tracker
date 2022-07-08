import { createClient } from 'redis';

const client = createClient();
client.connect().then((value) => console.log('Redis connected'));
client.flushAll().then((value) => console.log('Redis flush'));

export default client;