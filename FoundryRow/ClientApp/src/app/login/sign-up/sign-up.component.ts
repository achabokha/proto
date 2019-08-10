import { Component, OnInit } from '@angular/core';

class User {
  email: string | undefined;
  password: string | undefined;
  confirmPassword: string | undefined;
  firstName: string | undefined;
  lastName: string | undefined;
  token: string | undefined;
}

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.sass']
})
export class SignUpComponent implements OnInit {


  user: User = new User();

  constructor() {

  }

  ngOnInit(): void {

  }

  resolved(captchaResponse: string) {
  }

  onSubmit() {
  }

}
