export class User {
    email: string | undefined;
    password: string | undefined;
    confirmPassword: string | undefined;
    firstName: string | undefined;
    lastName: string | undefined;
    token: string | undefined;
}

export class ConfirmEmail {
    userId: string;
    code: string;
}