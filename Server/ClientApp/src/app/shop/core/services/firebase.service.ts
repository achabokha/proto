import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from 'selenium-webdriver/http';



@Injectable()
export class FirebaseService {
    constructor(private http: HttpClient) { }
    
}