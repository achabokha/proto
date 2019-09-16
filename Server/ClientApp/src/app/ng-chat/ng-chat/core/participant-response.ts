import { ParticipantMetadata } from "./participant-metadata";
import { Group } from './group';
import { IChatParticipant } from './chat-participant';

export class ParticipantResponse
{
    public participants: IChatParticipant[];
    public metadata: ParticipantMetadata;
}
