import { BaseService } from './base.service';
import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { map, catchError, retry } from 'rxjs/operators';
import { Observable, Subject } from "rxjs";

import { AuthService } from "./auth.service";
import { ChangePassword, ForgotPassword, ResetPassword } from './../models';
import { ConfirmPassword } from '../models/kokard';

@Injectable()
export class UserService extends BaseService {

    public stateChange: Subject<any> = new Subject<any>();

    constructor(private http: HttpClient, private authService: AuthService) {
        super();
    }

    getSettings(): Observable<any> {
        return this.http.get('/api/User/GetSettings', this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }
    
    changePassword(user: ChangePassword): Observable<any> {
        return this.http.post('/api/User/ChangePassword', user, this.authService.authJsonHeaders()).pipe(
            catchError(super.errorCheck)
        );
    }

    forgotPassword(user: ForgotPassword): Observable<any> {
        return this.http.post('/api/User/ForgotPassword', user).pipe(
            catchError(super.errorCheck)
        );
    }

    resetPassword(user: ResetPassword): Observable<any> {
        return this.http.post('/api/User/ResetPassword', user).pipe(
            catchError(super.errorCheck)
        );
    }

    confirmPassword(confirm: ConfirmPassword): Observable<any> {
        return this.http.post('api/User/ConfirmPassword/', confirm, this.authService.authJsonHeaders()).pipe(
            map((result: any) => {
                this.stateChange.next({ status: result.status, message: result.message });
                return result;
            }),
            catchError((error: any) => {
                console.log('Error: ', error);
                this.stateChange.next({ status: error.error.status, message: error.error.message });
                return Observable.throw(error.json().message || 'Server error')
            })
        );
    }

    getInvitees(): Observable<any> {
        return this.http.get('api/User/GetInvitees/', this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }
}
