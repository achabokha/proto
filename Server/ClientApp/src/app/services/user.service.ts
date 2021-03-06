import { BaseService } from "./base.service";
import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map, catchError, retry } from "rxjs/operators";
import { Observable, Subject, throwError } from "rxjs";

import { AuthService } from "./auth.service";
import { ChangePassword, ForgotPassword, ResetPassword, User } from "../models";
import { environment } from "src/environments/environment";

@Injectable()
export class UserService extends BaseService {

    public stateChange: Subject<any> = new Subject<any>();

    constructor(private http: HttpClient, private authService: AuthService) {
        super();
    }

    updateUser(userDetails: any): Observable<any> {
        return this.http.post(environment.apiUrl + "/api/users/update", userDetails, this.authService.authJsonHeaders()).pipe(
            map((result: any) => {
                this.stateChange.next({ status: result.status, message: result.message });
                return result;
            }),
            catchError((error: any) => {
                console.log("Error: ", error);
                this.stateChange.next({ status: error.error.status, message: error.error.message });
                return throwError(error.message || "Server error");
            })
        );
    }

    getUserDetails(userId: string): Observable<any> {
        return this.http.get("api/users/getDetails?userId=" + userId, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
	}

	getCurrentUser(): Observable<any> {
        return this.http.get(environment.apiUrl + "/api/users/getDetails?user", this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
	}

    getUsers(): Observable<User[]> {
        return this.http.get<User[]>(environment.apiUrl + "/api/Users/GetAll", this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getSettings(): Observable<any> {
        return this.http.get(environment.apiUrl + "/api/User/GetSettings", this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    changePassword(user: ChangePassword): Observable<any> {
        return this.http.post(environment.apiUrl + "/api/User/ChangePassword", user, this.authService.authJsonHeaders()).pipe(
            catchError(super.errorCheck)
        );
    }

    forgotPassword(user: ForgotPassword): Observable<any> {
        return this.http.post(environment.apiUrl + "/api/User/ForgotPassword", user).pipe(
            catchError(super.errorCheck)
        );
    }

    resetPassword(user: ResetPassword): Observable<any> {
        return this.http.post(environment.apiUrl + "/api/User/ResetPassword", user).pipe(
            catchError(super.errorCheck)
        );
    }

}
