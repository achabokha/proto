import { Component, Inject, forwardRef } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from "@angular/material";
import { User } from "src/app/models";
import { UserService } from "src/app/services/user.service";
import { NgxAuthFirebaseUIConfig } from "src/app/interfaces/config.interface";
import { NgxAuthFirebaseUIConfigToken } from "src/app/services/auth-process.service";
import { FormControl, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: "app-edit-user-dialog",
  templateUrl: "./edit-user-dialog.component.html",
  styleUrls: ["./edit-user-dialog.component.scss"]
})
export class EditUserDialogComponent {

  userForm: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<EditUserDialogComponent>,
    private userService: UserService,
    private snackBar: MatSnackBar,

    @Inject(forwardRef(() => NgxAuthFirebaseUIConfigToken)) public config: NgxAuthFirebaseUIConfig,
    @Inject(MAT_DIALOG_DATA) public data: User) {

    this.userForm = new FormGroup({
      email: new FormControl(this.data.email, [
        Validators.required
      ]),
      firstName: new FormControl(this.data.firstName, [
        Validators.required
      ]),
      lastName: new FormControl(this.data.lastName, [
        Validators.required
      ]),
      phoneNumber: new FormControl(this.data.phoneNumber, []),
      dateLastAccessed: new FormControl({ value: this.data.dateLastAccessed, disabled: true }, []),
      phoneNumberConfirmed: new FormControl({ value: this.data.phoneNumberConfirmed, disabled: true }, []),
      emailConfirmed: new FormControl({ value: this.data.emailConfirmed, disabled: true }, [])
    });
  }

  get email() { return this.userForm.get("email"); }
  get firstName() { return this.userForm.get("firstName"); }
  get lastName() { return this.userForm.get("lastName"); }

  onNoClick(): void {
    this.dialogRef.close();
  }

  updateUser(): void {
    if (this.userForm.valid) {
      const data = Object.assign(this.data, this.userForm.value);
      this.userService.updateUser(data).subscribe(
        (d) => {
          this.dialogRef.close(data);
          this.snackBar.open(
            `${d.message}`,
            "OK",
            { duration: 2000 });
        }, (error) => {
          this.dialogRef.close();
          this.snackBar.open(
            `${error}`,
            "OK",
            { duration: 2000 });
        }
      );
    }
  }

}
