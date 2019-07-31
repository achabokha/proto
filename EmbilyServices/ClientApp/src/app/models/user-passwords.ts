export class ChangePassword {
    currentPassword: string;
    newPassword: string;
    confirmPassword: string;
}

export class ResetPassword {
    email: string;
    password: string;
    confirmPassword: string; 
    code: string;
}

export class ForgotPassword {
    email: string;
}

export class Login {
    email: string;
    password: string;
}