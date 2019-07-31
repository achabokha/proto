import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map, catchError, retry } from 'rxjs/operators';
import { Observable } from "rxjs";

import { AuthService } from "./auth.service";
import { BaseService } from "./base.service";
import { DataService } from "./data.service";
import { User, ConfirmEmail } from './../models';

@Injectable()
export class SignUpService extends BaseService {

    constructor(private http: HttpClient, private data: DataService, private authService: AuthService) {
        super();
    }

    register(user: User): Observable<any> {
        if (this.data.promoToken)
            user.token = this.data.promoToken;

        return this.http.post('/api/SignUp/Register', user).pipe(
            catchError(super.errorCheck)
        );
    }

    confirmEmail(user: ConfirmEmail): Observable<any> {
        return this.http.post('/api/SignUp/ConfirmEmail', user).pipe(
            catchError(super.errorCheck)
        );
    }
}
