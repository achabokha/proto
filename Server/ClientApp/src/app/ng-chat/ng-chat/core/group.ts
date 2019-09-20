import { Guid } from "./guid";
import { User } from "./user";
import { ChatParticipantStatus } from "./chat-participant-status.enum";
import { IChatParticipant } from "./chat-participant";
import { ChatParticipantType } from "./chat-participant-type.enum";

export class Group
{
    constructor(participants: IChatParticipant[])
    {   
        this.chattingTo = participants;
        this.status = ChatParticipantStatus.Online;
        this.groupId = "";

        // TODO: Add some customization for this in future releases
        this.displayName = participants[0].displayName;
    }

    public hubContextId: string = "";
    public chattingTo: IChatParticipant[];

    public title: string | null;
    
    public status: ChatParticipantStatus;
    public avatar: string | null;
    public displayName: string;
    public groupId: string;
    public unreadMessages: number | null;
}
