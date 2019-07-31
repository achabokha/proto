import { Component, OnInit } from '@angular/core';
import { UserService } from "../../../services/user.service";
import { ForgotPassword } from './../../../models';

@Component({
    selector: 'app-forgot-password',
    templateUrl: './forgot-password.component.html',
    styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {

    user: ForgotPassword = new ForgotPassword();
    spinner: boolean = false;

    constructor(private userService: UserService) { }

    ngOnInit() {
    }

    onSubmit() {
        this.spinner = false;
        this.userService.forgotPassword(this.user).subscribe(null, null, () => this.spinner = false);
    }

}
