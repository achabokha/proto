import { Component, OnInit, Input } from '@angular/core';
import { DataService } from '../../../services/data.service';
import { ApplicationService } from '../../../services/application.service';

@Component({
    selector: 'app-app-integration',
    templateUrl: './integration.component.html',
    styleUrls: ['./integration.component.css']
})
export class IntegrationComponent implements OnInit {

    constructor(public data: DataService, private applicationService: ApplicationService) {
    }

    ngOnInit(): void {
    }

    update(): void {
        this.applicationService.assignCard(this.data.selectApp).subscribe(result => {
        });
    }

}
