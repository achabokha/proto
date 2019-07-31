import { ActivatedRoute } from '@angular/router';
import { ConfirmEmail } from './../../../models/user';
import { SignUpService } from './../../../services/signup.service';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-confirm-email',
    templateUrl: './confirm-email.component.html',
    styleUrls: ['./confirm-email.component.css']
})
export class ConfirmEmailComponent implements OnInit {
    user: ConfirmEmail = new ConfirmEmail();

    spinner: boolean = false;
    error: boolean = false;

    constructor(private singUpService: SignUpService, private route: ActivatedRoute) { }

    ngOnInit(): void {
        let userId = this.route.snapshot.queryParamMap.get('userId');
        let code = this.route.snapshot.queryParamMap.get('code');

        if (!code || !userId) {
            this.error = true;
            return;
        }
        this.user.code = code;
        this.user.userId = userId;

        this.spinner = true;
        this.singUpService.confirmEmail(this.user).subscribe(
            () => { this.spinner = false },
            error => {
                this.error = true, this.spinner = false;
            });
    }
}
