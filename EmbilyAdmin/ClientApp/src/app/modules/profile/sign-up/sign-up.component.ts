
import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { User } from "../../../models/user";
import { SignUpService } from "../../../services/signup.service";
import { DataService } from '../../../services/data.service';

@Component({
    selector: 'app-signup',
    templateUrl: './sign-up.component.html',
    styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {
    user: User = new User();

    recaptcha: any;

    spinner: boolean = true;

    resloved: boolean = false;


    constructor(private signUpService: SignUpService, private route: ActivatedRoute, private data: DataService) {

    }

    ngOnInit(): void {
        let token = this.route.snapshot.paramMap.get('token') || null
        if (token) this.data.promoToken = token;
    }

    resolved(captchaResponse: string) {
        this.resloved = true;
        //console.log(`Resolved captcha with response ${captchaResponse}:`);
    }

    onSubmit() {
        if (this.resloved) {
            this.signUpService.register(this.user).subscribe();
        }
    }


}
