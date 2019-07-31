import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { map, catchError, retry } from 'rxjs/operators';

import { AuthService } from "./auth.service";
import { BaseService } from "./base.service";

import { User, ConfirmEmail } from './../models';
import { Address } from './../models/address';
import { PassportInfo } from './../models/passport-info';

@Injectable()
export class ApplicationService extends BaseService {

    constructor(private http: HttpClient, private authService: AuthService) {
        super();
    }

    startApplication(currencyCode: string): Observable<string> {
        return this.http.post('/api/Application/StartApplication', { CurrencyCode: currencyCode }, this.authService.authJsonHeaders()).pipe(
            map((result: any) => {
                return result.applicationId;
            }),
            catchError(super.errorCheck)
        );
    }

    getComments(appId: string): Observable<string> {
        return this.http.get(`/api/Application/GetComments/${appId}`, this.authService.authJsonHeaders()).pipe(
            map((result: any) => {
                return result;
            }), catchError(super.errorCheck));
    }

    getOrderCost(appId: string): Observable<any> {
        return this.http.get(`/api/Application/GetOrderCost/${appId}`, this.authService.authJsonHeaders()).pipe(
            map((result: any) => {
                return result;
            })
            , catchError(super.errorCheck));
    }

    getOrderDetails(appId: string, cryptoCurrencyCode: string): Observable<any> {
        return this.http.post('/api/Application/GetOrderDetails', { appId: appId, cryptoCurrencyCode: cryptoCurrencyCode }, this.authService.authJsonHeaders()).pipe(
            map((result: any) => {
                return result;
            })
            , catchError(super.errorCheck));
    }

    getDocumentInfo(appId: string, docType: string): Observable<any> {
        return this.http.get('/api/Application/GetDocumentInfo/' + appId + '/' + docType, this.authService.authJsonHeaders()).pipe(
            map((result: any) => {
                return result;
            })
            , catchError(super.errorCheck));
    }

    uploadFile(image: FormData): Observable<any> {
        let token = this.authService.getAccessToken();

        let options = {
            headers: new HttpHeaders({
                //'Content-Type': 'application/x-www-form-urlencoded',
                'enctype': 'multipart/form-data',
                'Accept': 'application/json',
                'Authorization': 'Bearer ' + token
            })
        };

        return this.http.post('/api/Application/UpdateDocument', image, options).pipe(
            map((result: any) => {
                return result;
            })
            , catchError(super.errorCheck));
    }

    getPassportInfo(appId: string): Observable<PassportInfo> {
        return this.http.get(`/api/Application/GetPassportInfo/${appId}`, this.authService.authJsonHeaders()).pipe(
            map((result: any) => {
                return result;
            })
            , catchError(super.errorCheck));
    }

    updatePassportInfo(passportInfo: PassportInfo): Observable<any> {
        return this.http.post('/api/Application/UpdateApplication', passportInfo, this.authService.authJsonHeaders()).pipe(
            catchError(super.errorCheck));
    }

    getAddress(appId: string): Observable<Address> {
        return this.http.get('/api/Application/GetAddress/' + appId, this.authService.authJsonHeaders()).pipe(
            map((result: any) => {
                return result;
            })
            , catchError(super.errorCheck));
    }

    updateAddress(address: Address): Observable<any> {
        return this.http.post('/api/Application/UpdateAddress', address, this.authService.authJsonHeaders()).pipe(
            catchError(super.errorCheck));
    }

    getShippingAddressAndShippingOptions(appId: string): Observable<any> {
        return this.http.get('/api/Application/GetShippingAddressAndShippingOptions/' + appId, this.authService.authJsonHeaders()).pipe(
            map((result: any) => {
                return result;
            })
            , catchError(super.errorCheck));
    }

    updateShippingAddressAndShippingOptions(applicationId: string, address: Address, shippingDetail: any, selectedShippingOption: number): Observable<any> {
        return this.http.post('/api/Application/UpdateShippingAddressAndShippingOptions',
            { applicationId, address, shippingDetail, selectedShippingOption },
            this.authService.authJsonHeaders()).pipe(
                catchError(super.errorCheck)
            );
    }

    awaitingPayment(appId: string, orderId: string): Observable<any> {
        return this.http.post('/api/Application/AwaitingPayment', { ApplicationId: appId, OrderId: orderId }, this.authService.authJsonHeaders()).pipe(
            catchError(super.errorCheck)
        );
    }

    submit(appId: string): Observable<any> {
        return this.http.post('/api/Application/Submit', { ApplicationId: appId }, this.authService.authJsonHeaders()).pipe(
            catchError(super.errorCheck)
        );
    }

    getShippingOptions(applicationId: string, currencyCode: string, postalCode: string, countryCode: string): Observable<any> {
        return this.http.post('/api/Application/GetShippingOptions',
            { applicationId, currencyCode, postalCode, countryCode },
            this.authService.authJsonHeaders()).pipe(
                map((result: any) => {
                    return result;
                })
                , catchError(super.errorCheck));
    }

    getFileImage(documentId: string): Observable<any> {
        return this.http.get(`api/Application/GetFileImage/${documentId}`, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

}
