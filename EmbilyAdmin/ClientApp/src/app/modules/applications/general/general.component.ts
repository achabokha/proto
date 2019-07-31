import { Component, OnInit, Input } from '@angular/core';
import { DataService } from '../../../services/data.service';
import { ApplicationService } from '../../../services/application.service';

@Component({
    selector: 'app-app-general',
    templateUrl: './general.component.html',
    styleUrls: ['./general.component.css']
})
export class GeneralComponent implements OnInit {

    @Input() appUser: any;
    @Input() appDetails: any; 

    constructor(public data: DataService, private applicationService: ApplicationService) {
        
    }

    ngOnInit(): void {

    }

    update(): void {
        this.applicationService.updateApplicationInfo(this.data.selectApp).subscribe(result => {
           
        });
    }

}
