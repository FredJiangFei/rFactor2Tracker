import { Document, Schema, model } from 'mongoose';
import { IDriver } from './driver';

export interface ISession extends Document {
  Id: string;
  Drivers: IDriver[];
}

const SessionSchema = new Schema<ISession>({
  Id: {
    type: String,
  },
  Drivers: [],
});

export const Session = model<ISession>('Session', SessionSchema);
