import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable, Subject } from "rxjs";
import { Router } from "@angular/router";
import { map, catchError, retry } from 'rxjs/operators';

import { AuthService } from "./auth.service";

@Injectable()
export class AffiliateService {

    initState: any = {
        state: 'none',
        message: '',
    };

    public stateChange: Subject<any> = new Subject<any>();

    constructor(private http: HttpClient, private authService: AuthService, private router: Router) {
    }

    sendInvite(email: string): Observable<any> {
        return this.http.post('/api/Affiliate/InviteNewUser', { email }, this.authService.authJsonHeaders()).pipe(
            catchError((error) => this.authService.errorCheck(error))
        );
    }

    createToken(description: string): Observable<any> {
        return this.http.post('/api/Affiliate/CreateToken', { description }, this.authService.authJsonHeaders()).pipe(
            catchError((error) => this.authService.errorCheck(error))
        );
    }

    deactivateToken(tokenId: string): Observable<any> {
        return this.http.post('/api/Affiliate/DeactivateToken', { tokenId }, this.authService.authJsonHeaders()).pipe(
            catchError((error) => this.authService.errorCheck(error)));
    }

    getTokenList(): Observable<any> {
        return this.http.get('/api/Affiliate/GetTokens', this.authService.authJsonHeaders()).pipe(
            catchError((error) => this.authService.errorCheck(error))
        );
    }

    redeemAffliateBalance(accountFromId: string, accountToId: string): Observable<any> {
        this.stateChange.next({ status: 'processing', message: "" });
        return this.http.post('/api/Affiliate/RedeemBalance',
            {
                sourceAccountId: accountFromId,
                destinationAccountId: accountToId
            },
            this.authService.authJsonHeaders()).pipe(
                map((result: any) => {
                    this.stateChange.next({ status: 'success', message: result.message });
                    return result;
                }),
                catchError((error: any) => {
                    console.log('Error: ', error);
                    if (error.status == 401) {
                        this.router.navigateByUrl('/login');
                    }
                    if (error.status == 500) {
                        this.stateChange.next({ status: 'error', message: "Please, try again later" });
                        return Observable.throw(error.json().message || 'Server error')
                    }
                    let e = error.json();
                    this.stateChange.next({ status: 'error', message: error.message });
                    return Observable.throw(error.json().message || 'Server error')
                })
            );
    }
}
