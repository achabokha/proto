import { Observable } from 'rxjs';
import { Message, MessageSeen } from "./message";
import { User } from "./user";
import { ParticipantResponse } from "./participant-response";
import { IChatParticipant } from './chat-participant';
import { Group } from './group';
import { AuthService } from 'src/app/services/auth.service';

export abstract class ChatAdapter {
    // ### Abstract adapter methods ###

    public abstract listFriends(): Observable<ParticipantResponse[]>;
    public abstract userList(search: string): Observable<ParticipantResponse[]>;

    public abstract getMessageHistory(participant: string): Observable<Message[]>;
    public abstract getGroupMessageHistory(groupId: string): Observable<Message[]>;

    public abstract sendMessage(message: Message): void;
    public abstract markMessagesAsRead(arrSeen: MessageSeen[]): void;

    public abstract get authService(): AuthService;

    public abstract createNewGroup(participants: Group): Observable<string>;

    // ### Adapter/Chat income/ingress events ###

    public onFriendsListChanged(participant: IChatParticipant): void {
        this.friendsListChangedHandler(participant);
    }

    public onMessageReceived(message: Message): void {
        this.messageReceivedHandler(message);
    }

    // Event handlers
    friendsListChangedHandler: (participant: IChatParticipant) => void = (participantsResponse: IChatParticipant) => { };
    messageReceivedHandler: (message: Message) => void = (message: Message) => { };
}
