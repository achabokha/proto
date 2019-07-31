import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, Subject } from "rxjs";
import { map, catchError, retry } from 'rxjs/operators';

import { Account, Application, Transaction } from "../models";
import { AuthService } from "./auth.service";
import { SetPIN } from "../models/kokard";

@Injectable()
export class AccountsService {

    public stateChange: Subject<any> = new Subject<any>();

    constructor(private http: HttpClient, private authService: AuthService) {
    }

    getDashboardInfo(): Observable<any> {
        return this.http.get('/api/Accounts/GetDashboardInfo', this.authService.authJsonHeaders()).pipe(
            map(result => {
                return result;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getCardAccounts(): Observable<any> {
        return this.http.get('api/Accounts/GetCardAccounts', this.authService.authJsonHeaders()).pipe(
            map(result => {
                return result;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getBalance(accountId: string): Observable<any> {
        return this.http.get('api/Accounts/GetBalance/' + accountId, this.authService.authJsonHeaders()).pipe(
            map(result => {
                return result['balance'];
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getNewAddress(currencyCode: string, accountId: string): Observable<any> {
        return this.http.get('api/Accounts/GetNewAddress/' + currencyCode + '/' + accountId, this.authService.authJsonHeaders()).pipe(
            map(result => {
                return result;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getAccounts(): Observable<Account[]> {
        return this.http.get('api/Accounts/GetAccounts', this.authService.authJsonHeaders()).pipe(
            map(result => {
                var accounts = result as Account[];
                return accounts;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getLoadTransactions(): Observable<Transaction[]> {
        return this.http.get<Transaction[]>('api/Accounts/GetLoadTransactions', this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getAffiliateTransactions(): Observable<any> {
        return this.http.get('api/Accounts/GetAffiliateTransactions', this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getAllTransactions(): Observable<any> {
        return this.http.get('api/Accounts/GetAllTransactions', this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    IsRoleAffiliate(): Observable<any> {
        return this.http.get('api/Accounts/IsRoleAffiliate', this.authService.authJsonHeaders()).pipe(
            map(result => {
                return result;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    setLostCard(accountId: string): Observable<any> {
        return this.http.get('api/Accounts/SetLostCard/' + accountId, this.authService.authJsonHeaders()).pipe(
            map(result => {
                return result;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    setCardPIN(setPIN: SetPIN): Observable<any> {
        return this.http.post('api/Accounts/SetCardPIN/', setPIN, this.authService.authJsonHeaders()).pipe(
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

    getFormattedCardNumber(number: string) {
        return `${number.slice(0, 4)} ${number.slice(4, 6)}** **** ${number.slice(12, 16)}`;
    }
}
