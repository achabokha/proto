import { ChatAdapter, IChatGroupAdapter, User, Group, Message, ChatParticipantStatus, ParticipantResponse, ParticipantMetadata, ChatParticipantType, IChatParticipant } from "src/app/ng-chat";
import { Observable, of, throwError } from "rxjs";
import { map, catchError } from "rxjs/operators";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";

import * as signalR from "@aspnet/signalr";
import { AuthService } from "src/app/services/auth.service";

export class SignalRGroupAdapter extends ChatAdapter implements IChatGroupAdapter {

  constructor(private username: string, private http: HttpClient,
    private authService: AuthService) {
    super();

    this.initializeConnection();
  }
  public static serverBaseUrl: string = environment.apiUrl + "/hubs/"; // Set this to 'https://localhost:5001/' if running locally
  public userId: string;

  private hubConnection: signalR.HubConnection;

  private initializeConnection(): void {
    console.log(`${SignalRGroupAdapter.serverBaseUrl}groupchat`);
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${SignalRGroupAdapter.serverBaseUrl}groupchat`)
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

    this.hubConnection.on("messageReceived", (participant, message) => {
      // Handle the received message to ng-chat
      this.onMessageReceived(participant, message);
    });

    this.hubConnection.on("friendsListChanged", (participantsResponse: Array<ParticipantResponse>) => {
      // Use polling for the friends list for this simple group example
      // If you want to use push notifications you will have to send filtered messages through your hub instead of using the "All" broadcast channel
      // this.onFriendsListChanged(participantsResponse.filter(x => x.participant.id != this.userId));
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

  getMessageHistory(destinataryId: any): Observable<Message[]> {
    // This could be an API call to your web application that would go to the database
    // and retrieve a N amount of history messages between the users.
    return of([]);
  }

  sendMessage(message: Message): void {
    if (this.hubConnection && this.hubConnection.state == signalR.HubConnectionState.Connected) {
      this.hubConnection.send("sendMessage", message);
    }
  }

  groupCreated(group: Group): void {
    this.hubConnection.send("groupCreated", group);
  }
}
