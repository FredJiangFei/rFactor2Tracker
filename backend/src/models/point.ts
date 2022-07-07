import { Document, Schema, model } from 'mongoose';

export interface IPoint extends Document {
  Amount : number;
  Reason: string;
}

const PointSchema = new Schema<IPoint>({
  Amount: {
    type: Number
  },
  Reason: {
    type: String
  }
});

export const Point = model<IPoint>("Point", PointSchema);