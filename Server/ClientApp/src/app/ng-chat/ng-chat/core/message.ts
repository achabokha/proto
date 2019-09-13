import { MessageType } from './message-type.enum';
import { User } from 'src/app/models';
import { IChatParticipant } from './chat-participant';

export class Message
{
    public type?: MessageType = MessageType.Text;
    public groupId: string;
    public message: string;
    public dateSent?: Date;
    public dateSeen?: Date;
    public fromUser: IChatParticipant;
}
