export class User {
    email: string | undefined;
    password: string | undefined;
    confirmPassword: string | undefined;
    firstName: string | undefined;
    lastName: string | undefined;
	id: string | undefined;
	roles: string | undefined;
	dateLastAccessed: Date | undefined;
	emailConfirmed: boolean | undefined;
	phoneNumberConfirmed: boolean | undefined;
	phoneNumber: string | undefined;
}

export class ConfirmEmail {
    userId: string;
    code: string;
}