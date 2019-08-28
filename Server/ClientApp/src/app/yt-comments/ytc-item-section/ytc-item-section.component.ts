import { Component, Input } from '@angular/core';
import { YtComment } from '../model/yt-comment.model';

@Component({
    selector: 'ytc-item-section',
    templateUrl: './ytc-item-section.component.html',
    styleUrls: ['./ytc-item-section.component.scss']
})

export class YtcItemSectionComponent {

    @Input() dataSource: YtComment[];

    constructor() {

    }
}
