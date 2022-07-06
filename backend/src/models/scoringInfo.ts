import { Document, Schema, model } from 'mongoose';

export interface IScoringInfo extends Document {
  Id: number;
  StartPosition: number;
  Position: number;
  Score: number;
  ImprovingStartPosition: boolean;
  LoosingStartPosition: boolean;
}

const ScoringInfoSchema = new Schema<IScoringInfo>({
  Id: {
    type: Number
  },
  StartPosition: {
    type: Number
  },
  Position: {
    type: Number
  },
  Score: {
    type: Number
  },
  ImprovingStartPosition: {
    type: Boolean
  },
  LoosingStartPosition: {
    type: Boolean
  }
});

export const ScoringInfo = model<IScoringInfo>("ScoringInfo", ScoringInfoSchema);