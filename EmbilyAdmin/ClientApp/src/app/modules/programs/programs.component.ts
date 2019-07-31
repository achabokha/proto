import { Component, ViewChild } from '@angular/core';
import { DataService } from '../../services/data.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ProgramsService } from 'src/app/services/programs.service';

@Component({
    selector: 'app-programs',
    templateUrl: './programs.component.html',
    styleUrls: ['./programs.component.scss']
})

export class ProgramsComponent {

    loadingIndicator: boolean = false;
    selected = [];
    public newOnly: boolean = false;
    style: string = "table_embily_light";

    temp = [];

    constructor(public data: DataService, private programsService: ProgramsService, private router: Router, private route: ActivatedRoute) {

    }

    ngOnInit(): void {
        this.getData();
    }

    getData(): any {
        this.data.programs = null;
        this.programsService.getPrograms().subscribe((r) => {
            this.data.programs = r;
            this.temp = r;
        });
    }

    onSelect({ selected }) {
        this.router.navigate(["programs", this.selected[0].programId]);
    }

    updateFilter(event, columnName) {
        this.data.programs = this.temp;
        const val = event.target.value.toLowerCase();

        if (val == '') {
            return;
        }

        const temp = this.temp.filter(function (d) {
            if (d[columnName]) {
                return d[columnName].toString().toLowerCase().indexOf(val) !== -1 || !val;
            }
        });

        this.data.programs = temp;
    }
}
