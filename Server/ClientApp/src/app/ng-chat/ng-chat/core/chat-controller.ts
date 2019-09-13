import { User } from './user';
import { Group } from './group';

export interface IChatController
{
    triggerOpenChatWindow(user: Group): void;

    triggerCloseChatWindow(userId: any): void;

    triggerToggleChatWindowVisibility(userId: any): void;
}
