import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProgramsService } from '../../../services/programs.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Program } from 'src/app/models/program';


@Component({
    selector: 'app-program-details',
    templateUrl: './programDetails.component.html',
    styleUrls: ['./programDetails.component.css']
})

export class ProgramDetailsComponent implements OnInit {

    programId: string;
    programDetails: Program;

    loadingIndicator: boolean = false;

    constructor(private route: ActivatedRoute,
        public programsService: ProgramsService,
        public modalService: NgbModal) {
    }

    ngOnInit(): void {
        this.programId = this.route.snapshot.paramMap.get('programId');
        this.getData();
    }

    getData() {
        //this.accountDetails = null;
        this.programsService.getProgramDetails(this.programId).subscribe(result => {
            this.programDetails = result as Program;
        });
    }    

    update() {
        this.programsService.updateProgram(this.programDetails).subscribe(result => {

        });
    }

}
