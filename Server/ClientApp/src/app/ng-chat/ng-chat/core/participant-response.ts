import { ParticipantMetadata } from "./participant-metadata";
import { Group } from './group';

export class ParticipantResponse
{
    public participant: Group;
    public metadata: ParticipantMetadata;
}
