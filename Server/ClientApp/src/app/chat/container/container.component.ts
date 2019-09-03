import { Component, OnInit } from "@angular/core";
import { ChatService } from "src/app/services/chat.service";
import { SignalRGroupAdapter } from "./signalr-group-adapter";
import { HttpClient } from "@angular/common/http";
import { AuthService } from "src/app/services/auth.service";
import { ChatAdapter } from 'ng-chat';
import { SignalRAdapter } from './signaIr-adapter';

@Component({
  selector: "app-container",
  templateUrl: "./container.component.html",
  styleUrls: ["./container.component.scss"]
})
export class ContainerComponent implements OnInit {

  userId = "offline-demo";
  username: string;
  title = "app";
  currentTheme = "dark-theme";
  triggeredEvents = [];

  signalRAdapter: SignalRGroupAdapter ;

  constructor(private http: HttpClient, private authService: AuthService) { }

  ngOnInit() {
    this.authService.currentUser$.subscribe(d => {
      if (d) {
        this.signalRAdapter = new SignalRGroupAdapter(d.email, this.http, this.authService);
      }
    });
  }

  onEventTriggered(event: string): void {
    this.triggeredEvents.push(event);
  }



}
