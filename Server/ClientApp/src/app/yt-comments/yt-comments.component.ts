import { Component } from '@angular/core';
import { YtComment } from './model';
import { ArrayDataSource } from '@angular/cdk/collections';

const data: YtComment[] = [
    {
        name: "Segin",
        id: '1',
        parent_id: null,
        comment: 'This highlights are great. 🍫 <br> I think I might do more of it. 🚗 ✈ Test Text Test Text Test Text Test Text Test Text Test Text Test Text Test Text Test Text Test Text Test Text Test Text Test Text Test Text Test Text Test Text Test Text',
        like: 10,
        dislike: 0,

        replies: [
            {
                name: "John",
                id: '3',
                parent_id: '1',
                comment: 'You might want to consider to widen your options.😋',
                like: 10,
                dislike: 0,
                replies: []
            },
            {
                name: "Peter",
                id: '4',
                parent_id: '1',
                comment: 'You might want to consider to widen your options.',
                like: 10,
                dislike: 0,
                replies: []
            },
        ]
    },
    {
        name: "Peter",
        id: '2',
        parent_id: null,
        comment: 'This highlights are great. I think I might do more of it. 😁',
        like: 10,
        dislike: 0,
        replies: []
    }, 
    {
        name: "John",
        id: '5',
        parent_id: null,
        comment: 'This highlights are great. I think I might do more of it. 🤣',
        like: 10,
        dislike: 0,
        replies: []
    },
    {
        name: "Karen",
        id: '6',
        parent_id: null,
        comment: 'This highlights are great. I think I might do more of it. ',
        like: 10,
        dislike: 0,
        replies: []
    },
    {
        name: "John",
        id: '7',
        parent_id: null,
        comment: 'This highlights are great. I think I might do more of it. ',
        like: 10,
        dislike: 0,
        replies: []
    },
    {
        name: "John",
        id: '8',
        parent_id: null,
        comment: 'This highlights are great. I think I might do more of it. ',
        like: 10,
        dislike: 0,
        replies: []
    },
    {
        name: "John",
        id: '9',
        parent_id: null,
        comment: 'This highlights are great. I think I might do more of it. ',
        like: 10,
        dislike: 0,
        replies: []
    },
    {
        name: "Peter",
        id: '10',
        parent_id: null,
        comment: 'This highlights are great. I think I might do more of it. ',
        like: 10,
        dislike: 0,
        replies: []
    },
    {
        name: "John",
        id: '11',
        parent_id: null,
        comment: 'This highlights are great. I think I might do more of it. ',
        like: 10,
        dislike: 0,
        replies: []
    }
];

@Component({
    selector: 'app-yt-comments',
    templateUrl: './yt-comments.component.html',
    styleUrls: ['./yt-comments.component.scss']
})

export class YtCommentsComponent {

    dataSource = data;

    constructor() {

    }
}
