import { createClient } from 'redis';

const connect = async () => {
  const client = createClient();
  await client.connect();

  return client;
};

export default {
  connect,
};
