import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, timer } from 'rxjs';
import { map, catchError, retry } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { Dashboard } from "../models/dashboard";

@Injectable()
export class DashboardService {

    constructor(private http: HttpClient,  private authService: AuthService) {
    }

    GetAll(): Observable<Dashboard> {
        return this.http.get('api/Dashboard/GetAll', this.authService.authJsonHeaders()).pipe(
            map(result => {
                return result as Dashboard;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    GetTransactions(hours: any, currency: any): Observable<any> {
        return this.http.get(`api/Dashboard/GetAllTransactions?hours=${hours}&currency=${currency}`, this.authService.authJsonHeaders()).pipe(
        map(result => {
            return result;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    GetApplications(hours: any): Observable<any> {
        return this.http.get(`api/Dashboard/GetApplications?hours=${hours}`, this.authService.authJsonHeaders()).pipe(
        map(result => {
            return result;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    GetTransactionDistribution(hours: any): Observable<any> {
        return this.http.get(`api/Dashboard/GetTransactionDistribution?hours=${hours}`, this.authService.authJsonHeaders()).pipe(
        map(result => {
            return result;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

}
