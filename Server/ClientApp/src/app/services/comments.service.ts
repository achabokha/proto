import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { of } from "rxjs";
import { delay } from 'rxjs/operators';
import { commentTreeMock } from './mocks/commets.mock';


export interface TreeNode {
    id: string;
    parentId: string;
    name: string;
    content: string;
    datetime: string;
    likes: number;
    dislikes: number;
    children: TreeNode[];
}


@Injectable({
    providedIn: "root"
})
export class CommentsService {
    constructor(private httpClient: HttpClient) {
        
    }

    getCommentTree() {
         return of(commentTreeMock).pipe(delay(0));
    }
}

