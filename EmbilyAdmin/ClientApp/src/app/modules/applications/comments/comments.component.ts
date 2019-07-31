import { Component, OnInit, Input, OnChanges } from '@angular/core';

@Component({
    selector: 'app-app-comments',
    templateUrl: './comments.component.html',
    styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnChanges {

    @Input() comments: string;

    commentsObj: any;

    constructor() {
    }

    ngOnChanges(): void {
        if (this.comments) this.commentsObj = JSON.parse(this.comments);
    }
}