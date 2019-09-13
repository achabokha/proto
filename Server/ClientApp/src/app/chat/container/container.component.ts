import { Component, OnInit } from "@angular/core";
import { SignalRGroupAdapter } from "./signalr-group-adapter";
import { HttpClient } from "@angular/common/http";
import { AuthService } from "src/app/services/auth.service";
import { ChatAdapter } from "src/app/ng-chat";
import { Platform } from "@ionic/angular";

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

  isMobile = false;
  isBrowser = false;

  signalRAdapter: SignalRGroupAdapter;

  constructor(private http: HttpClient,
              private authService: AuthService,
              private platform: Platform) { 
                this.platform.ready().then(
                  () => {
                    if (this.platform.is("cordova")) {
                      this.isMobile = true;
                    } else {
                      this.isMobile = true;
                    }
                  }
                )
              }

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
