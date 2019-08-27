import { Component, Input, OnInit, AfterViewInit } from '@angular/core';
import { YtComment } from '../model';

@Component({
    selector: 'ytc-thread',
    templateUrl: './ytc-thread.component.html',
    styleUrls: ['./ytc-thread.component.scss']
})

export class YtcThreadComponent implements OnInit, AfterViewInit {
   
   

    @Input() dataSource: YtComment[];

    isCommentCollapsed: boolean = false;

    constructor() {

    }

    ngOnInit(): void {
        console.info("OnInit: " + this.dataSource);
    }

    ngAfterViewInit(): void {
        console.info("AfterViewInit: " + this.dataSource);
    }

    openComment() {
        if (!this.isCommentCollapsed) {
            this.isCommentCollapsed = true;
            //this._onChange()
        }
    }

    closeComment() {
        if (this.isCommentCollapsed) {
            this.isCommentCollapsed = false;
            //this._onChange()
        }
    }

    toggleComment() {
        this.isCommentCollapsed ? this.closeComment() : this.openComment();
        //this._onChange();
    }
}
