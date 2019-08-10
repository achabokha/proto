import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http';

import { Router } from "@angular/router";
import { Observable, Subject, throwError } from 'rxjs';
import { map, catchError, retry} from 'rxjs/operators';

import { AuthTokenModel } from '../models/security/auth-token-model';
import { LoginModel } from '../models/security/login-model';

//TODO: implement token refresh at some point --

@Injectable()
export class AuthService {
	isLoginError: boolean;

	loginStatusChange: Subject<boolean> = new Subject<boolean>();

	constructor(private http: HttpClient, private router: Router) { }

	login(username: string, password: string): Observable<boolean> {

		const httpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/x-www-form-urlencoded'
			})
		};

		const params = new HttpParams()
			.append("grant_type", "password")
			.append("username", username)
			.append("password", password)
			.append('scope', 'openid email phone profile offline_access');

		let requestBody = params.toString();

		// this call will give 401 (access denied HTTP status code) if login unsuccessful)
		return this.http.post<boolean>('/connect/token', requestBody, httpOptions).pipe(
			map((value:any) => {
				//console.log('response: ', value)
				let token = (value as AuthTokenModel);
				localStorage.setItem("refresh-token", token.refresh_token);
				this.parseToken(token);
				return true;
			}),
			catchError(error => this.handleError(error))
		);
	}

	loginWithFacebook(userId: string, accessToken: string): Observable<boolean> {

		const httpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/x-www-form-urlencoded'
			})
		};

		const params = new HttpParams()
			.append("grant_type", "urn:ietf:params:oauth:grant-type:facebook_access_token")
			.append("assertion", userId)
			.append("access_token", accessToken)
			.append('scope', 'openid email phone profile offline_access');

		let requestBody = params.toString();

		// this call will give 401 (access denied HTTP status code) if login unsuccessful)
		return this.http.post<boolean>('/connect/token', requestBody, httpOptions).pipe(
			map((value:any) => {
				//console.log('response: ', value)
				let token = value as AuthTokenModel;
				localStorage.setItem("refresh-token", token.refresh_token);
				this.parseToken(token);
				return true;
			}),
			catchError(error => this.handleError(error))
		);
	}

	refreshToken(): Observable<any> {
		let header = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });

		let refreshToken = localStorage.getItem("refresh-token");
		//this.getToken().refresh_token

		let params = new HttpParams()
			.append('refresh_token', refreshToken)
			.append('grant_type', 'refresh_token');
		//.append('scope', 'openid email phone profile offline_access');

		let requestBody = params.toString();

		return this.http.post<boolean>('/connect/token', requestBody, { headers: header }).pipe(
			map((value:any) => {
				//console.log('response: ', value)
				let token = value as AuthTokenModel;
				this.parseToken(token);
				return true;
			}),
			catchError(error => {
				return this.handleError(error);
			}));
	}

	parseToken(token: AuthTokenModel) {
		const now = new Date();
		token.expiration_date = new Date(now.getTime() + token.expires_in * 1000).getTime().toString();

		this.storeToken(token);
		this.loginStatusChange.next(true);
	}

	private handleError(error: HttpErrorResponse) {
		if (error.error instanceof ErrorEvent) {
			// A client-side or network error occurred. Handle it accordingly.
			console.error('An error occurred:', error.error.message);
		} else {
			// The backend returned an unsuccessful response code.
			// The response body may contain clues as to what went wrong,
			console.error(
				`Backend returned code ${error.status}, ` +
				`body was: ${error.error}`);
		}
		// return an observable with a user-facing error message
		return throwError(
			'Something bad happened; please try again later.');
	};

	storeToken(token: AuthTokenModel) {
		localStorage.setItem('auth-token', JSON.stringify(token));
	}

	getToken(): AuthTokenModel {
		let tokenJson = localStorage.getItem("auth-token");
		if (tokenJson) {
			let token = <AuthTokenModel>JSON.parse(tokenJson);
			return token;
		}
		return null;
	}

	getAccessToken(): string {
		let tokenJson = localStorage.getItem("auth-token");
		if (tokenJson) {
			let token = <AuthTokenModel>JSON.parse(tokenJson);
			return token.access_token;
		}
		return "not found";
	}

	removeToken() {
		localStorage.removeItem('auth-token');
	}

	logout(): void {
		this.removeToken();
		this.loginStatusChange.next(false);
	}

	get isLoggedIn(): boolean {
		if (typeof window !== 'undefined') { // hack for server side rendering, server side dose not have --
			let tokenJson = localStorage.getItem('auth-token');
			if (!tokenJson) return false;

			let token = <AuthTokenModel>JSON.parse(tokenJson);
			// console.log("Token: ", token);

			return (new Date().getTime() < +token.expiration_date);
		}
		return false;
	}

	// for requesting secure data using json
	authJsonHeaders() {
		let token = this.getAccessToken();

		let options = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
				'Accept': 'application/json',
				'Authorization': 'Bearer ' + token
			})
		};

		return options;
	}

	// for requesting secure data from a form post
	authFormHeaders() {
		let token = this.getAccessToken();

		let options = {
			headers: new HttpHeaders({
				'Content-Type': 'application/x-www-form-urlencoded',
				'Accept': 'application/json',
				'Authorization': 'Bearer ' + token
			})
		};

		return options;
	}

	errorCheck(error: any) {
		console.error("error");
		console.error("status: " + error.status);
		console.error(error);
		if (error.status == 401) {
			this.router.navigateByUrl('/login');
		}
		return Observable.throw(error.json().message || 'Server error')
	}
}