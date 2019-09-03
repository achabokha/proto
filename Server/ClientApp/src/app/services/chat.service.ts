import { Injectable } from "@angular/core";
import { Observable, Subscription, timer } from "rxjs";
import { HubConnection } from "@aspnet/signalr";
import * as signalR from "@aspnet/signalr";
import { environment } from "src/environments/environment";

@Injectable({
  providedIn: "root"
})
export class ChatService {
  private connection: HubConnection;
  constructor() {

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(environment.apiUrl + "/hubs/chat")
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.connection.start();

    this.connection.on("sendToAll", data => {
      console.log(data);
    });
  }

  sendMessage(user: string, message: string) {
    this.connection.invoke("sendToAll", user, message);
  }
}
