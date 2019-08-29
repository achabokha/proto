import { Component, OnInit } from "@angular/core";
import { UserService } from "../../services/user.service";
import { AuthService } from "src/app/services/auth.service";
import { MatDialog } from "@angular/material";
import { User } from "../../models";
import { EditUserDialogComponent } from "../edit-user-dialog/edit-user-dialog.component";

@Component({
  selector: "app-user-list",
  templateUrl: "./user-list.component.html",
  styleUrls: ["./user-list.component.scss"]
})
export class UserListComponent implements OnInit {

  data: User[];

  displayedColumns: string[] = ["email", "firstName", "lastName"];

  constructor(private userService: UserService,
              private authService: AuthService,
              public dialog: MatDialog) { }

  ngOnInit(): void {
    this.getUsers();
  }

  openDialog(row): void {
    const dialogRef = this.dialog.open(EditUserDialogComponent, {
      minWidth: "300px",
      width: "50%",
      maxWidth: "700px",
      data: row
    });
  }

  getUsers() {
    this.userService.getUsers().subscribe((r) => {
      this.data = r;
    });
  }

}
