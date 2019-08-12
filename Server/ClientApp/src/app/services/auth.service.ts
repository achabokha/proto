import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http';

import { Router } from '@angular/router';
import { Observable, Subject, throwError, of } from 'rxjs';
import { map, catchError, retry } from 'rxjs/operators';

import { AuthTokenModel } from '../models/security/auth-token-model';
import { LoginModel } from '../models/security/login-model';

import { ProviderAst } from '@angular/compiler';
import { AuthProvider } from '../enums';
import { resolve } from 'q';

declare var FB: any;
// TODO: implement token refresh at some point --


interface IAuthState {
	user: Observable<any>;
	currentUser: {
		name: string,
		email: string,
		phoneNumber: string,
		displayName: string,
		delete(): Promise<any>;
		updatePhoneNumber(phone: string): Promise<any>;
		updateEmail(email: string): Promise<any>;
		updateProfile(props: any): Promise<any>;
	};
	checkAuthStatus: () => void;
	signOut(): Promise<any>;
	signInAnonymously(): Promise<any>;
	signInWithEmailAndPassword(email: string, password: string): Promise<any>;
	signInWithPopup(authProvider: any): Promise<any>;
	sendPasswordResetEmail(email: string): Promise<any>;
	createUserWithEmailAndPassword(email: string, password: string): Promise<any>;
}

@Injectable()
export class AuthService {
	isLoginError: boolean;

	public authState: IAuthState = {
		user: new Observable(),
		currentUser: {
			name: '',
			email: '',
			phoneNumber: '',
			displayName: '',
			delete: (): Promise<any> => new Promise((res) => res()),
			updatePhoneNumber: (phone: string): Promise<any> => new Promise((res) => res()),
			updateEmail: (email: string): Promise<any> => new Promise((res) => res()),
			updateProfile: (props: any): Promise<any> => new Promise((res) => res())
		},
		signOut: (): Promise<any> => {

			return new Promise<any>(resolve => {
				this.logout();
				resolve();
			});
		},
		checkAuthStatus: () => { this.checkAuthorization(); },
		signInAnonymously: (): Promise<any> => new Promise((res) => res()),
		signInWithEmailAndPassword: (email: string, password: string): Promise<any> => this.login.bind(this),
		signInWithPopup: (authProvider: any): Promise<any> => this.loginWithFaceBook.call(this, authProvider),
		sendPasswordResetEmail: (email: string): Promise<any> => new Promise((res) => res()),
		createUserWithEmailAndPassword: (email: string, password: string): Promise<any> => this.createUserWithEmailAndPassword.bind(this)
	};


	constructor(private http: HttpClient, private router: Router) {
		this.authState.checkAuthStatus();
	}

	checkAuthorization() {
		const token = this.getToken();
		this.authState.user.subscribe(d => {
			console.log(`We have a user ${d}`);
			this.authState.currentUser = d;
		});
		this.authState.user = of(token ? token.user : null);
	}

	getToken(): AuthTokenModel {
		const tokenJson = localStorage.getItem('auth-token');
		if (tokenJson) {
			const token = JSON.parse(tokenJson) as AuthTokenModel;
			return token;
		}
		return null;
	};

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
		return this.http.post<any>('/connect/token', requestBody, httpOptions).pipe(map(d => {
			d.user = d.user || {};
			d.user.displayName = username;
			this.authState.user.lift(d.user);
			return d;
		})).toPromise();
	}

	createUserWithEmailAndPassword(email: string, password: string): Promise<any> {
		return this.http.post('/api/signup/register', { email, password }).toPromise();
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
					this.http.post<boolean>('/connect/token', requestBody, httpOptions).pipe<boolean>(
						map((d: any) => {
							d.user = d.user || {};
							d.user.displayName = d.userName;
							this.authState.user = of(d.user);
							return d;
						})
					).toPromise()
						.then(d => resolve(d));
				});
			});
		}
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
		return this.http.post<boolean>('/connect/token', requestBody, httpOptions).pipe(
			map((d: any) => {
				// console.log('response: ', value)
				const token = d as AuthTokenModel;
				localStorage.setItem('refresh-token', token.refresh_token);
				this.parseToken(token);

				d.user = d.user || {};
				d.user.displayName = userId;
				this.authState.user = of(d.user);
				return d;
			})
		).toPromise();
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

		return this.http.post<boolean>('/connect/token', requestBody, { headers: header }).pipe(
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
		this.authState.user = this.http.get('/api/User/GetSettings', this.authJsonHeaders()).pipe(
			catchError((error: any) => this.errorCheck(error))
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
		this.authState.user = of(null);
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
