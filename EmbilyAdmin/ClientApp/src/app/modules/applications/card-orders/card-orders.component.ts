import { Component, Input, OnInit, Inject } from '@angular/core';
import { DataService } from '../../../services/data.service';
import { ApplicationService } from '../../../services/application.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
    selector: 'app-app-card-orders',
    templateUrl: './card-orders.component.html',
    styleUrls: ['./card-orders.component.css']
})
export class CardOrdersComponent implements OnInit {
    
    appId: string;
    loadingIndicator: boolean = false;
    
    constructor(public data: DataService, private applicationService: ApplicationService, private route: ActivatedRoute, private router: Router, private authService: AuthService) {
    }

    getData() {
        this.appId = this.route.snapshot.paramMap.get('appId');
        this.applicationService.getCardOrderList(this.appId).subscribe((r) => {
            this.data.cardOrders = r;
        });
    }

    ngOnInit(): void {
        //this.appId = this.route.snapshot.paramMap.get('appId');
    }
}
