import { Component, ViewChild, Input, OnInit } from '@angular/core';
import { YtComment } from '../model/yt-comment.model';

@Component({
    selector: 'ytc-header',
    templateUrl: './ytc-header.component.html',
    styleUrls: ['./ytc-header.component.scss']
})

export class YtcHeaderComponent implements OnInit {
   
    @Input() dataSource: YtComment[];

    private newMessage: YtComment;
    newComment: string;

    constructor() {

    }

    ngOnInit(): void {

    }

    public renderCountComments(count) {
        if (count) {
            const countString = count.toString();
            return countString.replace(/(\d{1,3}(?=(?:\d\d\d)+(?!\d)))/g, "$1" + ' ');
        }
    }

    addComment(): void {
        this.newMessage = new YtComment;
        this.newMessage.id = this.dataSource.length ? Math.max.apply(Math, this.dataSource.map((e) => e.id)) + 1 : 1;
        this.newMessage.name = 'John Doe';
        this.newMessage.comment = this.newComment;
        this.newMessage.replies = [];
        this.newMessage.like = 0;
        this.newMessage.dislike = 0;
        this.dataSource.push(this.newMessage)

        this.dataSource = this.dataSource.sort((one, two) => (one.id > two.id ? -1 : 1));

        console.info(this.dataSource);
    }

    cancel() {
        this.newComment = '';
    }
}
