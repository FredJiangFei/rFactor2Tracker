import mongoose from 'mongoose';

type TInput = {
  db: string;
}

export default ({db}: TInput) => {
  const connect = () => {
    mongoose.connect(db).then(() => console.log(`Mongodb Connected to ${db}...`));
  };
  connect();
};
