import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '../../services/data.service';
import { AuthService } from '../../services/auth.service';


import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { timer } from 'rxjs';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

    private connection: HubConnection | undefined;

    constructor(private route: ActivatedRoute, private router: Router, private data: DataService, private authService: AuthService) {

    }

    ngOnInit(): void {

        //timer(0, 500, () => this.connection.invoke('Echo', 'hello'));

        if (this.authService.isLoggedIn) this.router.navigate(['/dashboard']);

        let token = this.route.snapshot.paramMap.get('token') || null
        if (token) this.data.promoToken = token;
    }

}

