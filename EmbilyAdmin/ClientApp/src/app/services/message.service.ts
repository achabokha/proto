import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class MessageService {

    public newMassage: Subject<any> = new Subject<any>();

    messages: string[] = [];

    constructor() {}

    add(message: string, ...args) {
        console.log(message);
        this.messages.push(message);

        this.newMassage.next({ message: message, args: args});
    }

    clear() {
        this.messages = [];
    }
}
