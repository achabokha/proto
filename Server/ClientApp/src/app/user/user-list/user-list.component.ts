import { Component, OnInit, OnDestroy, ViewChild } from "@angular/core";
import { UserService } from "../../services/user.service";
import { AuthService } from "src/app/services/auth.service";
import { MatDialog, MatTable } from "@angular/material";
import { User } from "../../models";
import { EditUserDialogComponent } from "../edit-user-dialog/edit-user-dialog.component";
import { MediaObserver, MediaChange } from "@angular/flex-layout";
import { Subscription } from "rxjs";
import { trigger, state, style, transition, animate, group } from "@angular/animations";

@Component({
  selector: "app-user-list",
  templateUrl: "./user-list.component.html",
  styleUrls: ["./user-list.component.scss"],
  animations: [
    trigger("detailExpand", [
      state("collapsed", style({ height: "0px", minHeight: "0", overflow: "hidden" })),
      state("expanded", style({ height: "*", overflow: "hidden" })),
      transition("expanded <=> collapsed",
        group([
          animate("225ms cubic-bezier(0.4, 0.0, 0.2, 1)")
        ])
      )
    ]),
  ],
})
export class UserListComponent implements OnInit, OnDestroy {


  constructor(private userService: UserService,
    private authService: AuthService,
    public dialog: MatDialog,
    private mediaObserver: MediaObserver) {
    this.setupTable();
    this.flexMediaWatcher = mediaObserver.asObservable().subscribe((change: MediaChange[]) => {
      change.forEach(m => {
        if (m.mqAlias === "md" || m.mqAlias === "lt-md" || m.mqAlias === "gt-md") {
          this.currentScreenWidth = m.mqAlias;
          this.setupTable();
        }
      });
    });
  }

  data: User[];
  @ViewChild(MatTable, { static: false }) table: MatTable<any>;
  currentScreenWidth = "";
  flexMediaWatcher: Subscription;

  displayedColumns: string[];
  expandedElement: any;

  isExpansionDetailRow = (i: number, row: Object) => {
    console.log(this.expandedElement);
    return this.expandedElement === row;
  }



  ngOnInit(): void {
    this.getUsers();
  }

  setupTable() {
    this.displayedColumns = ["email", "firstName", "lastName", "phoneNumber"];
    if (this.currentScreenWidth === "lt-md") {
      this.displayedColumns = ["email", "firstName", "lastName"]; // remove 'internalId'
    }
  }



  openDialog(row): void {
    const dialogRef = this.dialog.open(EditUserDialogComponent, {
      minWidth: "300px",
      width: "50%",
      maxWidth: "700px",
      data: { ...row }
    });
    dialogRef.afterClosed().subscribe(d => {
      if (d) {
        this.data.splice(this.data.findIndex(i => i.id === d.id), 1, d);
        this.table.renderRows();
      }
    });
  }

  updateData(user: User) {
    if (user != null) {
      this.data.splice(this.data.findIndex(i => i.id === user.id), 1, user);
      this.table.renderRows();
    }
    this.expandedElement = null;
  }

  getUsers() {
    this.userService.getUsers().subscribe((r) => {
      this.data = r;
    });
  }

  ngOnDestroy(): void {
    this.flexMediaWatcher.unsubscribe();
  }

}
