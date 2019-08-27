export class YtComment {
    id: string;
    parent_id: string;
    name: string;
    comment: string;
    like: number;
    dislike: number;

    replies: YtComment[];
}
