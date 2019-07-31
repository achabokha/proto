import { BaseService } from './base.service';
import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { map, catchError, retry } from 'rxjs/operators';
import { Observable, Subject } from "rxjs";

import { AuthService } from "./auth.service";

@Injectable()
export class ProgramsService {

    public stateChange: Subject<any> = new Subject<any>();

    constructor(private http: HttpClient, private authService: AuthService) {
    }

    getPrograms(): Observable<any> {
        return this.http.get('/api/Programs/GetPrograms', this.authService.authJsonHeaders()).pipe(
            map(result => {
                return result;
            }),
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    getProgramDetails(programId: string): Observable<any> {
        return this.http.get(`api/Programs/Get/${programId}`, this.authService.authJsonHeaders()).pipe(
            catchError((error: any) => this.authService.errorCheck(error))
        );
    }

    updateProgram(program: any): Observable<any> {
        return this.http.post('api/Programs/UpdateProgram', program, this.authService.authJsonHeaders()).pipe(
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
