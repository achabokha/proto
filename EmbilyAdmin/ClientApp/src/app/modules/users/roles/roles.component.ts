import { Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { RoleService } from '../../../services/roles.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-roles',
    templateUrl: './roles.component.html',
    styleUrls: ['./roles.component.css']
})

export class RolesComponent implements OnInit {

    loadingIndicator: boolean = false;
    public roles: any;
    public data: any;
    public roleDetails: any;
    public dbupdate: any;
    editing = {};
    rows = [];
    nameRole: any;

    constructor(private modalService: NgbModal, private roleService: RoleService, private router: Router, private authService: AuthService) {
    }

    ngOnInit(): void {
        this.getData();
    }

    getData() {
        this.dbupdate = false;

        this.roleService.getAll().subscribe(result => {
            this.roles = result;
        });
    }

    goToUsers() {
        this.router.navigate(['users']);
    }

    rowClicked(data: any) {
        this.roleDetails = data.data;
        console.log(this.roleDetails);

        return data;
    }

    rowUpdate(event, cell, rowIndex) {

        this.editing[rowIndex + '-' + cell] = false;
        this.roles[rowIndex][cell] = event.target.value;
        this.roles = [...this.roles]

        this.roleService.updateUserRole(this.roles[rowIndex]).subscribe(result => {
            console.log('Roles database update successful!');
        });
    }

    rowCreate(event: any) {

        event.confirm.resolve(event.newData);

        this.data = event.newData;
        console.log(this.data); // This can be removed. Just checking result.

        
        //this.http.post('/api/roles/insert', this.data, this.authService.authJsonHeaders())
        //    .subscribe(data => {
        //        console.log('Roles database insert successful!');
        //        // this.users.load(this.users); // Method to reload data after update, not clear on 'source' set to 'users'
        //    }, error => {
        //        console.log(error.json());
        //    });
    }

    updateroleDetails(roleDetails: any): void {
        console.log(this.roleDetails);

        //this.http.post('/api/roles/update', this.roleDetails, this.authService.authJsonHeaders())
        //    .subscribe(response => {
        //        console.log('User database update successful!');
        //        this.roleDetails = false;
        //        this.dbupdate = true;
        //    }, error => {
        //        console.log(error.json());
        //    });
    }
    showModal(content) {     
        this.modalService.open(content);
    }

    add() {
        //this.modalService.open(content);
        this.roles.unshift({ name: this.nameRole });
        this.roles = this.roles.slice();

        this.roleService.addUserRole({ Name: this.nameRole }).subscribe(result => {
            console.log('Roles database update successful!');
        });
        //this.roles.splice(0, 0, "");
    }
}
