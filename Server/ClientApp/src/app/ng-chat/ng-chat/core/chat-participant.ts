import { ChatParticipantStatus } from "./chat-participant-status.enum";
import { ChatParticipantType } from "./chat-participant-type.enum";

export interface IChatParticipant {
    readonly participantType: ChatParticipantType;
    readonly hubContextId: any;
    status: ChatParticipantStatus;
    readonly avatar: string | null;
    readonly displayName: string;
    userId: string;
}
