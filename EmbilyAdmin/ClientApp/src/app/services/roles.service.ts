import { BaseService } from './base.service';
import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { map, catchError, retry } from 'rxjs/operators';
import { Observable } from "rxjs";
import { AuthService } from "./auth.service";
import { ChangePassword, ForgotPassword, ResetPassword } from './../models';

@Injectable()
export class RoleService extends BaseService {

    constructor(private http: HttpClient, private authService: AuthService) {
        super();
    }

    updateUserRole(data: any): Observable<any> {
        return this.http.post('/api/roles/update', data, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    addUserRole(data: any): Observable<any> {
        return this.http.post('/api/roles/insert', data, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }
    

    getAll(): Observable<any> {
        return this.http.get('api/Roles/GetAll', this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );

    }

    getUserRoles(userId: string): Observable<any> {
        return this.http.get('api/Roles/GetUserRoles?userId=' + userId, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    setRole(userId: string, roleName: string): Observable<any> {
        return this.http.post('api/Roles/SetRole', { "userId": userId, "roleName": roleName }, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    removeRole(userId: string, roleName: string): Observable<any> {
        return this.http.post('api/Roles/RemoveRole', { "userId": userId, "roleName": roleName }, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

}
