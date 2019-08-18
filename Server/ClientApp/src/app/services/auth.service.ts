import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http';

import { Router } from '@angular/router';
import { Observable, Subject, throwError, of, BehaviorSubject } from 'rxjs';
import { map, catchError, retry } from 'rxjs/operators';

import { AuthTokenModel } from '../models/security/auth-token-model';
import { LoginModel } from '../models/security/login-model';

import { ProviderAst } from '@angular/compiler';
import { AuthProvider } from '../enums';
import { resolve } from 'q';
import { EventEmitter } from 'events';
import { environment } from 'src/environments/environment';

declare var FB: any;
// TODO: implement token refresh at some point --




@Injectable()
export class AuthService {
	isLoginError: boolean;

	authState: any = {
		user: new BehaviorSubject<any>(null),
		currentUser: {
			name: '',
			email: '',
			phoneNumber: '',
			displayName: '',
			userID: '',
			delete: (): Promise<any> => new Promise((res) => res()),
			updateProfile: (displayName: string, email: string, phone: string): Promise<any> => this.updateDisplayName.call(this, displayName, email, phone)
		},
		signOut: (): Promise<any> => {

			return new Promise<any>(resolve => {
				this.logout();
				resolve(this.router.navigateByUrl("/"));
			});
		},
		checkAuthStatus: () => { this.checkAuthorization(); },
		signInAnonymously: (): Promise<any> => this.signInAnonymously.call(this),
		signInWithEmailAndPassword: (email: string, password: string): Promise<any> => this.login.call(this, email, password),
		signInWithPopup: (authProvider: any): Promise<any> => this.loginWithFaceBook.call(this, authProvider),
		sendPasswordResetEmail: (email: string): Promise<any> => this.sendPasswordReset.call(this, email),
		createUserWithEmailAndPassword: (userName: string, email: string, password: string): Promise<any> => this.createUserWithEmailAndPassword.call(this, userName, email, password)
	};

	currentUser$ = this.authState.user;
	currentUser: any;

	constructor(private http: HttpClient, private router: Router) {
		this.authState.checkAuthStatus();
		this.currentUser$.subscribe(d => {
			console.log(d);
			this.currentUser = d;
		})
	}

	checkAuthorization() {
		const token = this.getToken();
		if (token) {
			this.parseToken(token);
		}
	}

	getToken(): AuthTokenModel {
		const tokenJson = localStorage.getItem('auth-token');
		if (tokenJson) {
			const token = JSON.parse(tokenJson) as AuthTokenModel;
			return token;
		}
		return null;
	}

	updateDisplayName(displayName: string, email, phone): Promise<any> {
		return new Promise((resolve) => {
			this.http.post(environment.apiUrl + '/api/User/UpdateProfile', { displayName, email, phone } , this.authJsonHeaders()).subscribe(d => {
				resolve(this.authState.user.next(d));
			});
		});
	}

	login(username: string, password: string): Promise<any> {

		const httpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/x-www-form-urlencoded'
			})
		};

		const params = new HttpParams()
			.append('grant_type', 'password')
			.append('username', username)
			.append('password', password)
			.append('scope', 'openid email phone profile offline_access');

		const requestBody = params.toString();

		// this call will give 401 (access denied HTTP status code) if login unsuccessful)
		return this.http.post<any>(environment.apiUrl + '/connect/token', requestBody, httpOptions).pipe(map(d => {
			return d;
		})).toPromise();
	}

	createUserWithEmailAndPassword(userName: string, email: string, password: string): Promise<any> {
		return new Promise((resolve) => {
			this.http.post(environment.apiUrl + '/api/signup/register', { userName, email, password }).subscribe(d => {
				resolve(this.login(email, password));
			});
		});
	}

	signInAnonymously(): Promise<any> {
		return new Promise(async (resolve) => {
			const httpOptions = {
				headers: new HttpHeaders({
					'Content-Type': 'application/x-www-form-urlencoded'
				})
			};

			const params = new HttpParams()
				.append('grant_type', 'urn:ietf:params:oauth:grant-type:guest_user')
				.append('access_token', 'guestUser')
				.append('scope', 'openid email phone profile offline_access');

			const requestBody = params.toString();

			// this call will give 401 (access denied HTTP status code) if login unsuccessful)
			this.http.post<boolean>(environment.apiUrl + '/connect/token', requestBody, httpOptions).toPromise()
				.then(d => resolve(d));
		});
	}

	loginWithFaceBook(authProvider: any): Promise<any> {
		if (authProvider === AuthProvider.Facebook) {
			return new Promise(async (resolve) => {
				FB.login((response) => {
					const httpOptions = {
						headers: new HttpHeaders({
							'Content-Type': 'application/x-www-form-urlencoded'
						})
					};

					const params = new HttpParams()
						.append('grant_type', 'urn:ietf:params:oauth:grant-type:facebook_access_token')
						.append('assertion', response.authResponse.userID)
						.append('access_token', response.authResponse.accessToken)
						.append('scope', 'openid email phone profile offline_access');

					const requestBody = params.toString();

					// this call will give 401 (access denied HTTP status code) if login unsuccessful)
					this.http.post<boolean>(environment.apiUrl + '/connect/token', requestBody, httpOptions).toPromise()
						.then(d => resolve(d));
				});
			});
		}
	}

	resetPassword(user: any): Promise<any> {
		return new Promise((resolve) => {
			this.http.post(environment.apiUrl + '/api/User/ResetPassword', user).subscribe(d => {
				resolve(d);
			});
		});
	}


	sendPasswordReset(email: string): Promise<any> {
		return new Promise((resolve) => {
			this.http.post(environment.apiUrl + '/api/User/ForgotPassword', { email }).subscribe(d => {
				resolve(d);
			});
		});
	}



	loginWithFacebookToken(userId: string, accessToken: string): Promise<any> {

		const httpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/x-www-form-urlencoded'
			})
		};

		const params = new HttpParams()
			.append('grant_type', 'urn:ietf:params:oauth:grant-type:facebook_access_token')
			.append('assertion', userId)
			.append('access_token', accessToken)
			.append('scope', 'openid email phone profile offline_access');

		const requestBody = params.toString();

		// this call will give 401 (access denied HTTP status code) if login unsuccessful)
		return this.http.post<boolean>(environment.apiUrl + '/connect/token', requestBody, httpOptions).toPromise();
	}

	refreshToken(): Observable<any> {
		const header = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });

		const refreshToken = localStorage.getItem('refresh-token');
		// this.getToken().refresh_token

		const params = new HttpParams()
			.append('refresh_token', refreshToken)
			.append('grant_type', 'refresh_token');
		// .append('scope', 'openid email phone profile offline_access');

		const requestBody = params.toString();

		return this.http.post<boolean>(environment.apiUrl + '/connect/token', requestBody, { headers: header }).pipe(
			map((value: any) => {
				// console.log('response: ', value)
				const token = value as AuthTokenModel;
				this.parseToken(token);
				return true;
			}),
			catchError(error => {
				return this.handleError(error);
			}));
	}

	parseRefreshToken(token: AuthTokenModel) {
		localStorage.setItem('refresh-token', token.refresh_token);
	}

	parseToken(token: AuthTokenModel) {
		const now = new Date();
		token.expiration_date = new Date(now.getTime() + token.expires_in * 1000).getTime().toString();

		this.storeToken(token);
		this.http.get(environment.apiUrl + '/api/User/GetSettings', this.authJsonHeaders()).subscribe(
			d => {
				this.authState.user.next(d);
			}
		);
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
	}

	storeToken(token: AuthTokenModel) {
		localStorage.setItem('auth-token', JSON.stringify(token));
	}



	getAccessToken(): string {
		const tokenJson = localStorage.getItem('auth-token');
		if (tokenJson) {
			const token = JSON.parse(tokenJson) as AuthTokenModel;
			return token.access_token;
		}
		return 'not found';
	}

	removeToken() {
		localStorage.removeItem('auth-token');
	}

	logout(): void {
		this.removeToken();
		this.authState.user.next(null);
	}

	get isLoggedIn(): boolean {
		if (typeof window !== 'undefined') { // hack for server side rendering, server side dose not have --
			const tokenJson = localStorage.getItem('auth-token');
			if (!tokenJson) { return false; }

			const token = JSON.parse(tokenJson) as AuthTokenModel;
			// console.log("Token: ", token);

			return (new Date().getTime() < +token.expiration_date);
		}
		return false;
	}

	// for requesting secure data using json
	authJsonHeaders() {
		const token = this.getAccessToken();

		const options = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
				Accept: 'application/json',
				Authorization: 'Bearer ' + token
			})
		};

		return options;
	}

	// for requesting secure data from a form post
	authFormHeaders() {
		const token = this.getAccessToken();

		const options = {
			headers: new HttpHeaders({
				'Content-Type': 'application/x-www-form-urlencoded',
				Accept: 'application/json',
				Authorization: 'Bearer ' + token
			})
		};

		return options;
	}

	errorCheck(error: any) {
		console.error('error');
		console.error('status: ' + error.status);
		console.error(error);
		if (error.status === 401) {
			this.router.navigateByUrl('/login');
		}
		return Observable.throw(error.json().message || 'Server error');
	}
}
