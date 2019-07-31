import { NgForm } from '@angular/forms';
import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { isPlatformBrowser, isPlatformServer } from '@angular/common';
import { PLATFORM_ID } from '@angular/core';
import { Http } from "@angular/http";
import { UserService } from "../../../services/user.service";
import { ResetPassword } from "../../../models";

@Component({
    selector: 'app-reset-password',
    templateUrl: './reset-password.component.html',
    styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {

    user: ResetPassword = new ResetPassword();
    code: string;
    error: boolean = false;
    complete: boolean = false;
    spinner: boolean = false;

    constructor(private route: ActivatedRoute, private userService: UserService) { }

    @ViewChild('pwdForm')
    private pwdForm: NgForm;

    ngOnInit() {
        //let code = this.route.snapshot.paramMap.get('code');
        let code = this.route.snapshot.queryParams['code'];

        if (!code) {
            this.error = true;
            this.complete = true;
        }
        else this.code = code;
    }

    onSubmit(user: ResetPassword) {
        this.spinner = true;
        this.user.code = this.code;
        this.userService.resetPassword(this.user).subscribe(
            () => { this.complete = true, this.spinner = false },
            error => { this.error = true, this.spinner = false; }
        );
    }

}
