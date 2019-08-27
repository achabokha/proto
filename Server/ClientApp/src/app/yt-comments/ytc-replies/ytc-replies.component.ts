import { Component, Input } from '@angular/core';
import { YtComment } from '../model/yt-comment.model';

@Component({
    selector: 'ytc-replies',
    templateUrl: './ytc-replies.component.html',
    styleUrls: ['./ytc-replies.component.scss']
})

export class YtcRepliesComponent {

    @Input() replies: YtComment[];

    isCommentCollapsed: boolean = false;

    constructor() {

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
