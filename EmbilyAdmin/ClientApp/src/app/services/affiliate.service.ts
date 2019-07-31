import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable } from "rxjs";
import { AuthService } from "./auth.service";
import { BaseService } from "./base.service";
import { map, catchError, retry } from 'rxjs/operators';

@Injectable()
export class AffiliateService extends BaseService {

    constructor(private http: HttpClient, private authService: AuthService) {
        super();
    }

    getAffiliateInvite(): Observable<any> {
        return this.http.get('api/affiliate/GetAffiliateInvite', this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getAffiliateTokens(): Observable<any> {
        return this.http.get('api/Affiliate/GetTokens', this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getAffiliateUsers(): Observable<any> {
        return this.http.get('api/Affiliate/GetUsers', this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }
}
