import { Component } from '@angular/core';
import { MessageService } from 'src/app/services/message.service';

@Component({
    selector: 'app-top-notification',
    templateUrl: './top-notification.component.html',
    styleUrls: ['./top-notification.component.scss']
})
export class TopNotificationComponent {

    message: string;
    state: string;
    alertType: string;
    timer: any;
    toastClass: string = '';
    showSpinner: boolean = false;

    constructor(private messageService: MessageService) {

        this.messageService.newMassage.subscribe((msg) => {
            this.message = msg.message;
            this.state = this.toUpperCaseFirst(msg.args[0]);
            if (this.timer) clearTimeout(this.timer);

            this.alertType = this.getAlertType(msg.args[0]);

            this.showSpinner = this.state == 'Processing';

            switch (this.state) {
                case 'Success':
                    this.timer = setTimeout(() => this.toastClass = 'hide', 5000);
                    break;
                case 'Error':
                    this.timer = setTimeout(() => this.toastClass = 'hide', 20000);
                    break;
            }

            this.toastClass = 'show';
        });
    }


    close() {
        this.toastClass = 'hide';
        if (this.timer) clearTimeout(this.timer);
    }

    getAlertType(state: any): string {
        switch (state) {
            case 'processing': return 'info';
            case 'success': return 'success';
            case 'error': return 'danger';
            default: return 'info';
        }
    }

    toUpperCaseFirst(string: string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
    }
}
