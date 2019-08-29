import { Component, Inject, forwardRef } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from "@angular/material";
import { User } from "src/app/models";
import { UserService } from "src/app/services/user.service";
import { NgxAuthFirebaseUIConfig } from "src/app/interfaces/config.interface";
import { NgxAuthFirebaseUIConfigToken } from "src/app/services/auth-process.service";

@Component({
  selector: "app-edit-user-dialog",
  templateUrl: "./edit-user-dialog.component.html",
  styleUrls: ["./edit-user-dialog.component.scss"]
})
export class EditUserDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<EditUserDialogComponent>,
    private userService: UserService,
    private snackBar: MatSnackBar,

    @Inject(forwardRef(() => NgxAuthFirebaseUIConfigToken)) public config: NgxAuthFirebaseUIConfig,
    @Inject(MAT_DIALOG_DATA) public data: User) { }

  onNoClick(): void {
    this.dialogRef.close();
  }

  updateUser(): void {
    this.userService.updateUser(this.data).subscribe(
      (d) => {
        this.snackBar.open(
          `${d.message} Updated`,
          "OK",
          { duration: 2000 });
      }
    );
  }

}
