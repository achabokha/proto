import { Component, ViewChild, NgZone, Input } from '@angular/core';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { take } from 'rxjs/operators';
import { YtComment } from '../model';

@Component({
    selector: 'ytc-header',
    templateUrl: './ytc-header.component.html',
    styleUrls: ['./ytc-header.component.scss']
})

export class YtcHeaderComponent {

    @Input() dataSource: YtComment[];

    constructor() {

    }

    public renderCountComments(count) {
        if (count) {
            const countString = count.toString();
            return countString.replace(/(\d{1,3}(?=(?:\d\d\d)+(?!\d)))/g, "$1" + ' ');
        }
    }
}
