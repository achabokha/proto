import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { HttpErrorHandler, HandleError } from '../../services/http-error-handler.service';




@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {

  private handleError: HandleError;

  signUpForm = new FormGroup({
    email: new FormControl('',
      [Validators.required,
      Validators.email
      ]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
      Validators.pattern('^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=?*()/<>.!:;_-]).*$')
    ]),
    confirmPassword: new FormControl('', [Validators.required])
  }, (c) => {
    if (c.get('password').value !== c.get('confirmPassword').value) {
      c.get('confirmPassword').setErrors({ nonMatching: true });

      return null;
    } else {
      c.get('confirmPassword').setErrors(null);
      return null;
    }
  });

  constructor(private router: Router,
    private http: HttpClient,
    private httpErrorHandler: HttpErrorHandler
  ) {
    this.handleError = httpErrorHandler.createHandleError('SignUpComponent');
  }

  ngOnInit(): void {

  }

  resolved(captchaResponse: string) {
  }

  onSubmit() {
    // TODO: Use EventEmitter with form value
    this.http.post('/api/signup/register', this.signUpForm.value).pipe(
      catchError(this.handleError('signup/register', this.signUpForm.value))
    ).subscribe(d => {
      console.log(d)
      this.router.navigateByUrl('/dashboard');
    });
  }

  getEmailErrorMessage() {
    return this.signUpForm.controls.email.hasError('required') ? 'You must enter a value' :
      this.signUpForm.controls.email.hasError('email') ? 'Not a valid email' :
        '';
  }

  getPasswordErrorMessage() {
    return this.signUpForm.controls.password.hasError('required') ? 'You must enter a value' :
      this.signUpForm.controls.password.hasError('minLength') ? 'Password needs to be at least 6 characters long' :
        this.signUpForm.controls.password.hasError('pattern') ?
          'Password should include 1 special character, 1 number, 1 uppercase and 1 lowercase character' :
          '';
  }

  getConfirmPasswordErrorMessage() {

    console.log(this.signUpForm.controls.confirmPassword.hasError('required') ? 'You must enter a value' :
      this.signUpForm.controls.confirmPassword.hasError('nonMatching') ? 'Passwords are not matching' :
        '');
    return this.signUpForm.controls.confirmPassword.hasError('required') ? 'You must enter a value' :
      this.signUpForm.controls.confirmPassword.hasError('nonMatching') ? 'Passwords are not matching' :
        '';
  }



}
