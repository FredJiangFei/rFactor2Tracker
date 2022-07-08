import { Document, Schema, model } from 'mongoose';

export interface IPoint extends Document {
  Points: number;
  Count: number;
  Reason: string;
}

const PointSchema = new Schema<IPoint>({
  Points: {
    type: Number,
  },
  Count: {
    type: Number,
  },
  Reason: {
    type: String,
  },
});

export const Point = model<IPoint>('Point', PointSchema);
