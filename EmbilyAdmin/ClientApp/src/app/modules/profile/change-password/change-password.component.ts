import { NgForm } from '@angular/forms';
import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { isPlatformBrowser, isPlatformServer } from '@angular/common';
import { PLATFORM_ID } from '@angular/core';
import { Http } from "@angular/http";
import { UserService } from './../../../services/user.service';
import { ChangePassword } from './../../../models/';

@Component({
    selector: 'app-change-password',
    templateUrl: './change-password.component.html',
    styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

    user: ChangePassword = new ChangePassword();
    error: boolean = false;
    complete: boolean = false;
    spinner: boolean = false;

    constructor(private userService: UserService) { }

    @ViewChild('pwdForm')
    private pwdForm: NgForm;

    ngOnInit() {
    }

    onSubmit(pwdForm: NgForm) {
        this.error = false;
        this.spinner = true;
        this.userService.changePassword(this.user)
            .subscribe(
            () => { this.complete = true; this.spinner = false; },
            error => { this.error = true; this.spinner = false; },
        );
    }
}
