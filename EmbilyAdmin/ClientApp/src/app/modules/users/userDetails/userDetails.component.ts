import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from '../../../services/user.service';

import { AccountsService } from '../../../services/accounts.service';
import { RoleService } from '../../../services/roles.service';


@Component({
    selector: 'app-user-details',
    templateUrl: './userDetails.component.html',
    styleUrls: ['./userDetails.component.css']
})

export class UserDetailsComponent implements OnInit {

    public userDetails: any;
    public transactions: any;
    public affiliates: any;

    public data: any;
    public showuserDetails: boolean = true;
    public userId: string = "";
    public allRoles: any = [];
    public avRoles: any = [];

    isUSDAffiliateAccount: any;
    isEURAffiliateAccount: any;

    loadingIndicator: boolean = false;

    selected = []; 

    transactionStatus: string[] = [];
    headerText: string = "";

    constructor(private router: Router,
        private route: ActivatedRoute,
        private roleService: RoleService,
        private userService: UserService,
        private accountsService: AccountsService) {

        this.route.params.subscribe(params => {
            this.userId = params.userId;
            if (this.userId) {
                this.getData(this.userId);
            }
        });
    }

    ngOnInit(): void {
        let userId = this.route.snapshot.paramMap.get('userId');
        this.getData(userId);
    }

    getData(userId: string) {
        this.userId = userId;
        this.userService.getUserDetails(this.userId).subscribe(result => {
            this.userDetails = result.user;
            this.transactions = result.transactions;
            this.affiliates = result.affiliates;
            this.headerText = this.userDetails.firstName + " " + this.userDetails.lastName;
            this.isUSDAffiliateAccount = this.userDetails.accounts.find(a => a.accountTypeString == 'Affiliate' && a.currencyCodeString == 'USD');
            this.isEURAffiliateAccount = this.userDetails.accounts.find(a => a.accountTypeString == 'Affiliate' && a.currencyCodeString == 'EUR');
            this.getUserRoles();
        });
    }


    getAvailibleNewRoles() {
        if (this.userDetails && this.allRoles && this.userDetails.roles) {
            this.avRoles = [];
            this.allRoles.forEach((element: any) => {
                if (!this.userDetails.roles.find(d => d == element.name)) {
                    this.avRoles.push(element);
                }
            });
        } else {
            this.avRoles = [];
        }
    }

    getUserRoles() {
        this.roleService.getAll().subscribe(result => {
            this.allRoles = result;
            this.getAvailibleNewRoles();
        });

        this.roleService.getUserRoles(this.userId).subscribe(result => {
            this.userDetails.roles = result;
            this.getAvailibleNewRoles();
        })
    }

    removeRole(roleName: string) {
        this.roleService.removeRole(this.userId, roleName).subscribe(result => {
            this.userDetails.roles = this.userDetails.roles.filter((r: string) => r != roleName)
            this.getAvailibleNewRoles();
        });
    }

    addRole(roleName: string) {
        this.roleService.setRole(this.userId, roleName).subscribe(result => {
            this.userDetails.roles.push(roleName);
            this.getAvailibleNewRoles();
        });
    }

    updateUserDetails(): void {
        this.userService.updateUser(this.userDetails).subscribe(result => {
            this.userDetails = result.user;
            this.headerText = this.userDetails.firstName + " " + this.userDetails.lastName;
        });
    }

    createAffiliateAccount(currencyCode) {
        this.accountsService.createAffiliateAccount(currencyCode, this.userId).subscribe(() => this.getData(this.userId));
    }

    onSelectAccounts({ selected }) {
        this.router.navigate(["accounts", this.selected[0].accountId]);
    }

    onSelectUsers({ selected }) {
        this.router.navigate(["users/details", this.selected[0].id]);
    }
}
