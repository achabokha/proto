import { ChatParticipantStatus } from "./chat-participant-status.enum";
import { IChatParticipant } from "./chat-participant";
import { ChatParticipantType } from "./chat-participant-type.enum";

export class User implements IChatParticipant
{
    public readonly participantType: ChatParticipantType = ChatParticipantType.User;
    public hubContextId: any;
    public displayName: string;
    public status: ChatParticipantStatus;
    public avatar: string;
    public userId: string;
}
