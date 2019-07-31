import { Component, OnInit } from '@angular/core';
import { AffiliateService } from '../../../services/affiliate.service';
import { NgForm } from '@angular/forms'; 
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from '../../../services/user.service';

@Component({
    selector: 'app-affiliate-invite',
    templateUrl: './affiliate-invite.component.html',
    styleUrls: ['./affiliate-invite.component.css']
})
export class AffiliateInviteComponent implements OnInit {
   

    spinner: boolean = false;
    status: string = 'none';
    message: string | undefined;
    emailInvite: string = "";
    invitees: any;
    countInvite: number;
    countRegistered: number;
    countApproved: number;
    countTransacting: number;

    constructor(private affiliateService: AffiliateService,
        private userService: UserService,
        public modalService: NgbModal) {

        
    }

    ngOnInit(): void {

    }

    invite() {
        if (this.emailInvite == "") {
            this.status = 'error';
            this.message = 'email is not specified';
            this.emailInvite = "";
            return;
        }

        this.status = '';
        this.spinner = true;

        this.affiliateService.sendInvite(this.emailInvite)
            .subscribe(r => {
                this.status = 'success';
                this.message = r.messaga;
                this.emailInvite = "";
                this.spinner = false;
            }, error => {
                this.status = 'error';
                this.message = error
                this.emailInvite = "";
                this.spinner = false;
            });
    }

    open(modalContent) {
        this.getInvitees();
        this.modalService.open(modalContent, { size: 'lg' })
    }

    getInvitees(){
        this.userService.getInvitees().subscribe(result => {

            this.invitees = result.customInvitees;
            this.countInvite = result.countInvite;
            this.countRegistered = result.countRegistered;
            this.countApproved = result.countApproved;
            this.countTransacting = result.countTransacting;
        });
    }
}
