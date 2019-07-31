import { BaseService } from './base.service';
import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { map, catchError, retry } from 'rxjs/operators';
import { Observable, Subject } from "rxjs";

import { Account, Transaction } from "../models";
import { AuthService } from "./auth.service";
import { Page } from '../models/page';

@Injectable()
export class AccountsService {

    public stateChange: Subject<any> = new Subject<any>();

    constructor(private http: HttpClient, private authService: AuthService) {
    }

    getAccounts(): Observable<any> {
        return this.http.get('/api/Accounts/GetAccounts', this.authService.authJsonHeaders()).pipe(
            map(result => {
                return result;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    //getAccounts(): Observable<any> {
    //    return this.http.get('/api/Accounts/GetAccounts', this.authService.authJsonHeaders()).pipe(
    //        catchError((error: any) => this.authService.errorCheck(error))
    //    );
    //}

    public getTransactions(page: Page): Observable<any> {
        return this.http.post('api/Accounts/GetTransactions/', page, this.authService.authJsonHeaders()).pipe(
             map(result => {
                return result;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getAccountDetails(accountId: string): Observable<any> {
        return this.http.get(`api/accounts/get/${accountId}`, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getNewCryptoAddress(accountId: string, currencyCode: string): Observable<any> {
        return this.http.get(`api/accounts/GetNewCryptoAddress/${accountId}/${currencyCode}`, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    setTransactionComplete(txnId: string): Observable<any> {
        return this.http.get('api/Accounts/SetTransactionComplete/' + txnId, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    createAffiliateAccount(currencyCode: string, userId: string): Observable<any> {
        return this.http.post('/api/accounts/CreateAffiliateAccount', { currencyCode: currencyCode, userId: userId }, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    setAccountStatus(accountId: string, accountStatus: string): Observable<any> {
        return this.http.post('/api/accounts/SetAccountSatus', { accountId, accountStatus }, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    updateAccount(account: any): Observable<any> {
        return this.http.post('api/Accounts/UpdateAccount', account, this.authService.authJsonHeaders()).pipe(
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

    loadAmount(load: any): Observable<any> {
        return this.http.post('api/Accounts/Load', load, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    errorCheck(error: any) {
        console.error("Account Services error");
        console.error(error);
        return Observable.throw(error.json().error || 'Server error')
    }

}
