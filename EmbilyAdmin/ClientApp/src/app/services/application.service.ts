import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, timer, Subject } from 'rxjs';
import { map, catchError, retry } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { Application, User, Address } from "../models";
import { ResponseContentType } from "@angular/http";

@Injectable()
export class ApplicationService {


    public stateChange: Subject<any> = new Subject<any>();

    constructor(private http: HttpClient, private authService: AuthService) {
    }

    getUser(userId: string): Observable<any> {
        //debugger;
        return this.http.get('/api/users/GetUser/' + userId, this.authService.authJsonHeaders()).pipe(
            map(result => {
                return result;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getUserApp(appId: string): Observable<any> {
        return this.http.get('/api/users/GetUserApp/' + appId, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }
    
    getAppAll(): Observable<any> {
        return this.http.get('/api/Applications/GetAll', this.authService.authJsonHeaders()).pipe(
            map(result => {
                return result;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getAppNew(): Observable<any> {
        return this.http.get('/api/Applications/GetNew', this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getApplicationInfo(appId: string): Observable<Application> {
        return this.http.get(`api/applications/get/${appId}`, this.authService.authJsonHeaders()).pipe(
            map(result => {
                return result as Application;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getUserInfo(userId: string): Observable<User> {
        return this.http.get(`api/users/get?userId=${userId}`, this.authService.authJsonHeaders()).pipe(
            map(result => {
                return result as User;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    updateApplicationInfo(appDetails: any): Observable<any> {
        return this.http.post('api/applications/update', appDetails, this.authService.authJsonHeaders());

        //return this.http.post('api/applications/update', appDetails, this.authService.authJsonHeaders()).pipe(
        //    map((result: any) => {
        //        this.stateChange.next({ status: result.status, message: result.message });
        //            return result;
        //        }),
        //    catchError((error: any) => this.authService.errorCheck(error))
        //);
    }

    assignCard(appDetails: any): Observable<any> {
        return this.http.post('api/applications/assignCard', appDetails, this.authService.authJsonHeaders());

        //return this.http.post('api/applications/assignCard', appDetails, this.authService.authJsonHeaders()).pipe(
        //    map((result: any) => {
        //        this.stateChange.next({ status: result.status, message: result.message });
        //        return result;
        //    }),
        //    catchError((error: any) => this.authService.errorCheck(error))
        //);
    }

    getAddress(appId: string): Observable<Address> {
        return this.http.get(`api/addresses/getAddress/${appId}`, this.authService.authJsonHeaders()).pipe(
            map(result => {
                return result as Address;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getFileImage(documentId: string): Observable<any> {
        return this.http.get(`api/applications/GetFileImage/${documentId}`, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getFilePdf(documentId: string): Observable<any> {
        return this.http.get(`api/applications/GetFilePdf/${documentId}`, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    updateAddress(address: Address): Observable<any> {
        return this.http.post('api/addresses/update', address, this.authService.authJsonHeaders()).pipe(
             map((result: any) => {
                this.stateChange.next({ status: result.status, message: result.message });
                return result;
            }),
            catchError((error: any) => {
                this.stateChange.next({ status: error.error.status, message: error.error.message });
                return Observable.throw(error.json().message || 'Server error')
            })
        );
    }

    getShippingAddress(appId: string): Observable<Address> {
        return this.http.get(`api/addresses/getShippingAddress/${appId}`, this.authService.authJsonHeaders()).pipe(
            map(result => {
                return result as Address;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getCardOrderList(appId: string): Observable<any> {
        return this.http.get('/api/applications/getCardOrderList/' + appId, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    updateStatus(obj: any): Observable <any> {
        return this.http.post('/api/applications/updateStatus/', obj, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    controlKYC(api: string, obj: any): Observable<any> {
        return this.http.post(api, obj, this.authService.authJsonHeaders()).pipe(
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
}
