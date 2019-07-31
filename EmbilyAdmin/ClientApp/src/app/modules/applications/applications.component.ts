import { Component, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../services/data.service';
import { ApplicationService } from '../../services/application.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-applications',
    templateUrl: './applications.component.html',
    styleUrls: ['./applications.component.scss']
})

export class ApplicationsComponent implements OnInit {

    loadingIndicator: boolean = false;
    selected = [];
    public newOnly: boolean = false;
    style: string = "table_embily_light";

    temp = [];

    constructor(public data: DataService, private applicationService: ApplicationService, private router: Router, private route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.newOnly = !!this.route.snapshot.queryParamMap.get('new') || false;
        this.getData();
    }

    getData() {
        this.data.applications = null;
        if (this.newOnly) {
            this.applicationService.getAppNew().subscribe((r) => {
                this.data.applications = r;
                this.temp = r;
            });
        } else {
            this.applicationService.getAppAll().subscribe((r) => {
                this.data.applications = r;
                this.temp = r;
            });
        }
       
    }

    onSelect({ selected }) {
        //debugger;
        this.data.selectApp = this.selected[0];
        this.router.navigate(["applications", this.selected[0].applicationId]);
    }

    updateFilter(event, columnName) {
        this.data.applications = this.temp;
        const val = event.target.value.toLowerCase();

        if (val == '') {
            return;
        }

        const temp = this.temp.filter(function (d) {
            if (d[columnName]) {
                return d[columnName].toString().toLowerCase().indexOf(val) !== -1 || !val;
            }
        });

        this.data.applications = temp;
    }
}
