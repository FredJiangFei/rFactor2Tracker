import { createClient } from 'redis';

const client = createClient();
client.connect().then((value) => console.log('Redis connected'));
client.flushAll().then((value) => console.log('Redis flush'));

const hGet = async (key: string, field: string) => {
  const cache = await client.hGet(key, field);
  if (cache === undefined) return {};

  const cacheObj = JSON.parse(cache);
  return cacheObj;
};

const hSet = (key: string, field: any, data: string) => {
  client.hSet(key, field, data);
};

export default {
  client,
  hGet,
  hSet,
};
