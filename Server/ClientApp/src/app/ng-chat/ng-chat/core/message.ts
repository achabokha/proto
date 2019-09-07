import { MessageType } from './message-type.enum';
import { User } from 'src/app/models';

export class Message
{
    public type?: MessageType = MessageType.Text;
    public fromId: any;
    public toId: any;
    public toEmail: string;
    public message: string;
    public dateSent?: Date;
    public dateSeen?: Date;
    public fromUser: User;
    public toUser: User;
}
