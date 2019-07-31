import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/data.service';

@Component({
  selector: 'app-all',
  templateUrl: './all.component.html',
  styleUrls: ['./all.component.scss']
})
export class AllComponent implements OnInit {

    loadingIndicator: boolean = false;

    constructor(public data: DataService) {
    }

    ngOnInit(): void {
    }

}
