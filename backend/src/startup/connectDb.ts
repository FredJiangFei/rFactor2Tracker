import mongoose from 'mongoose';

const connectDb = () => {
  const db = 'mongodb://localhost/points-scoring';
  mongoose.connect(db).then(() => console.log(`Mongodb Connected to ${db}...`));
};

export default connectDb;
