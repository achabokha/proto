import { ChatAdapter, IChatGroupAdapter, User, Group, Message, ChatParticipantStatus, ParticipantResponse, ParticipantMetadata, ChatParticipantType, IChatParticipant, MessageSeen } from "src/app/ng-chat";
import { Observable, of, throwError } from "rxjs";
import { map, catchError } from "rxjs/operators";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";

import * as signalR from "@aspnet/signalr";
import { AuthService } from "src/app/services/auth.service";

export class SignalRGroupAdapter extends ChatAdapter implements IChatGroupAdapter {

  constructor(private username: string, private http: HttpClient,
    public authService: AuthService) {
    super();
    this.authService.currentUser$.subscribe(() => {
      this.initializeConnection();
    });
  }
  public static serverBaseUrl: string = environment.apiUrl + "/hubs/"; // Set this to 'https://localhost:5001/' if running locally
  public userId: string;

  private hubConnection: signalR.HubConnection;

  private initializeConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${SignalRGroupAdapter.serverBaseUrl}groupchat`, { accessTokenFactory: this.authService.getAccessToken })
      .build();

    this.hubConnection
      .start()
      .then(() => {
        this.joinRoom();

        this.initializeListeners();
      })
      .catch(err => console.log(`Error while starting SignalR connection: ${err}`));
  }

  private initializeListeners(): void {
    this.hubConnection.on("generatedUserId", (userId) => {
      // With the userId set the chat will be rendered
      this.userId = userId;
    });

    this.hubConnection.on("messageReceived", (message) => {
      // Handle the received message to ng-chat
      this.onMessageReceived(message);
    });

    this.hubConnection.on("friendsListChanged", (participant: IChatParticipant) => {
      // Use polling for the friends list for this simple group example
      // If you want to use push notifications you will have to send filtered messages through your hub instead of using the "All" broadcast channel
      this.onFriendsListChanged(participant);
    });
  }

  joinRoom(): void {
    if (this.hubConnection && this.hubConnection.state == signalR.HubConnectionState.Connected) {
      this.hubConnection.send("join", this.username);
    }
  }

  listFriends(): Observable<ParticipantResponse[]> {
    // List connected users to show in the friends list
    // Sending the userId from the request body as this is just a demo
    return this.http
      .post(`${SignalRGroupAdapter.serverBaseUrl}listFriends`, { currentUserId: this.userId }, this.authService.authJsonHeaders())
      .pipe(
        map((res: any) => res),
        catchError((error: any) => throwError(error.error || "Server error"))
      );
  }

  userList(search: string): Observable<ParticipantResponse[]> {
    return this.http
      .post(`${SignalRGroupAdapter.serverBaseUrl}userList`, { searchText: search }, this.authService.authJsonHeaders())
      .pipe(
        map((res: any) => res),
        catchError((error: any) => throwError(error.error || "Server error"))
      );
  }

  getMessageHistory(participant: string): Observable<Message[]> {
    return this.http
      .post(`${SignalRGroupAdapter.serverBaseUrl}messageHistory`,
        { mailA: this.authService.currentUser.email, mailB: participant },
        this.authService.authJsonHeaders())
      .pipe(
        map((res: any) => res),
        catchError((error: any) => throwError(error.error || "Server error"))
      );
  }

  getGroupMessageHistory(groupId: string): Observable<Message[]> {
    return this.http
      .post(`${SignalRGroupAdapter.serverBaseUrl}groupMessageHistory`,
        { groupId },
        this.authService.authJsonHeaders())
      .pipe(
        map((res: any) => res),
        catchError((error: any) => throwError(error.error || "Server error"))
      );
  }

  createNewGroup(participants: Group): Observable<string> {
    if (participants.groupId === null || participants.groupId === "") {
      return this.http
        .post(`${SignalRGroupAdapter.serverBaseUrl}createGroup`,
          { group: participants },
          this.authService.authJsonHeaders())
        .pipe(
          map((res: any) => res.chatRoomId),
          catchError((error: any) => throwError(error.error || "Server error"))
        );
    } else {
      return of(participants.groupId);
    }
  }

  sendMessage(message: Message): void {
    if (this.hubConnection && this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.send("sendMessage", message);
    }
  }

  markMessagesAsRead(arrSeen: MessageSeen[]): void {
    if (this.hubConnection && this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.send("chatMessageSeen", arrSeen);
    }
  }

  groupCreated(group: Group): void {
    this.hubConnection.send("groupCreated", group);
  }
}
