import { Document, Schema, model } from 'mongoose';
import { IPoint } from './point';

export interface IDriver extends Document {
  Id: number;
  StartPosition: number;
  EndPosition: number;
  ImprovingStartPosition: boolean;
  LoosingStartPosition: boolean;
  Points: IPoint[]
}

const DriverSchema = new Schema<IDriver>({
  Id: {
    type: Number
  },
  StartPosition: {
    type: Number
  },
  EndPosition: {
    type: Number
  },
  ImprovingStartPosition: {
    type: Boolean
  },
  LoosingStartPosition: {
    type: Boolean
  },
  Points: []
});

export const Driver = model<IDriver>("Driver", DriverSchema);