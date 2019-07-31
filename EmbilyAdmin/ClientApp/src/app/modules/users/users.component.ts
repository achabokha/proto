import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { DataService } from '../../services/data.service';
import { UserService } from '../../services/user.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.scss']
})

export class UsersComponent implements OnInit {

    loadingIndicator: boolean = false;
    selected = [];
    userdetails: any;
    dbupdate: boolean = false;

    temp = [];

    constructor(private modalService: NgbModal, public data: DataService, private userService: UserService, private router: Router, private route: ActivatedRoute) {

    }

    ngOnInit(): void {
        this.getUsers();
    }

    getUsers() {
        this.userService.getUsers().subscribe((r) => {
            this.data.users = r;
            this.temp = r;
        });
    }

    goToRoles() {
        this.router.navigate(['users/roles/']);
    }

    onSelect({ selected }) {
        this.router.navigate(["users/details", this.selected[0].id]);
    }

    //onActivate(event) {
    //    console.log('Activate Event', event);
    //    this.userdetails = event.row;
    //}

    updateUserDetails(userdetails: any): void {

        this.userService.updateUser(userdetails).subscribe((r) => {
            this.dbupdate = true;
        });
    }

    showModal(content) {
        this.modalService.open(content);
    }

    updateFilter(event, columnName) {
        this.data.users = this.temp;
        const val = event.target.value.toLowerCase();

        if (val == '') {
            return;
        }

        const temp = this.temp.filter(function (d) {
            if (d[columnName]) {
                return d[columnName].toString().toLowerCase().indexOf(val) !== -1 || !val;
            }
        });

        this.data.users = temp;
    }
}
