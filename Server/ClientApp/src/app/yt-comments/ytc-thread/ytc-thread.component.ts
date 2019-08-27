import { Component, Input, OnInit, AfterViewInit } from '@angular/core';
import { YtComment } from '../model';

@Component({
    selector: 'ytc-thread',
    templateUrl: './ytc-thread.component.html',
    styleUrls: ['./ytc-thread.component.scss']
})

export class YtcThreadComponent implements OnInit, AfterViewInit {
   
   

    @Input() dataSource: YtComment[];

    

    constructor() {

    }

    ngOnInit(): void {
        console.info("OnInit: " + this.dataSource);
    }

    ngAfterViewInit(): void {
        console.info("AfterViewInit: " + this.dataSource);
    }


}
