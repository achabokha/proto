import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AccountsService } from '../../../services/accounts.service';
import { DataService } from '../../../services/data.service';
import { Account } from "../../../models";
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SetPIN, ConfirmPassword } from '../../../models/kokard';
import { UserService } from '../../../services/user.service';
import { ProcessingComponent } from '../../shared/processing/processing.component';

@Component({
    selector: 'app-setpin',
    templateUrl: './setpin.component.html',
    styleUrls: ['./setpin.component.scss']
})

export class SetpinComponent implements OnInit {

    @Input() account: Account;
    @ViewChild('processing') processing: ProcessingComponent | undefined;

    pin: any;
    confirmPassword: ConfirmPassword;
    password: any;
    setPINFlag: boolean;
    confirmFlag: boolean;
    setCardPIN: SetPIN;
    message: any;

    constructor(public modalService: NgbModal,
        private router: Router,
        public accountsService: AccountsService,
        private userServices: UserService,
        public data: DataService){
        
    }

    ngOnInit(): void {

        this.setPINFlag = false;
        this.confirmFlag = false;
        this.pin = '';
    }

    getFormattedCardNumber(number: string) {
        return this.accountsService.getFormattedCardNumber(number);
    }

    setPIN() {
        this.setPINFlag = true;
    }

    confirmPIN() {
        this.confirmPassword = new ConfirmPassword;
        this.setCardPIN = new SetPIN;
        this.confirmPassword.userId = this.account.userId;
        this.confirmPassword.password = this.password;
        this.userServices.confirmPassword(this.confirmPassword).subscribe(response => {
            this.setCardPIN.newPIN = this.pin;
            this.setCardPIN.cardReferenceID = "000320825257"; //test id
            this.accountsService.setCardPIN(this.setCardPIN).subscribe(response => {
                this.confirmFlag = true;
                this.message = response.message
            });
        });
    }

    closeModal() {
        this.setPINFlag = false;
        this.confirmFlag = false;
        this.pin = '';
        this.password = '';
        this.modalService.dismissAll();
    }
}
