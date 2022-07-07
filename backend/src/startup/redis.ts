import { createClient } from 'redis';

let client: any;

const connect = async () => {
    client = createClient();
    await client.connect();
}

const set = async (key: string, value: any) => {
   return await client.set(key, value);
}

const get = async (key: string) => {
  return await client.get(key);
}

export default {
    connect,
    set,
    get
}