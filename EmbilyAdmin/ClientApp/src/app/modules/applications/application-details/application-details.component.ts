import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { DataService } from '../../../services/data.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ApplicationService } from '../../../services/application.service';


@Component({
    selector: 'app-application-details',
    templateUrl: './application-details.component.html',
    styleUrls: ['./application-details.component.css']
})
export class ApplicationDetailsComponent implements OnInit {

    appId: any;
    reasonDesc: any;

    reasons = {
        '': `choose templated instructions if any from dropdown... `,
        'Proof of ID does not meet requirements':
            `<ul>
                <li>Ensure it is government issued and internationally recognizable.</li>
                <li>Must be valid (not expired), legible and in color.</li>
                <li>must not be edited or censored in any way.</li>
                <li>must include your signature, where applicable.</li>
            </ul>`,
        'Proof of Address Document is not a Valid legal statement':
            `<ul>
                <li>The address page of a bank statement</li>
                <li>The address page of a power bill</li>
                <li>The address page of a fixed line telephone bill or Internet</li>
                <li>A letter written by an official government body</li>
             </ul>`,
        'Proof of Address needs to have your name and address in Latin':
            `<ul>
                <li>The proof of address must be in Latin characters</li>
                <li>Documents in other characters sets such as Cyrillic or Japanese must be translated into English and notarized by an official government body.</li>
                <li>Your document must not be cropped or censored in any way</li>
                <li>It must be in full color</li>
             </ul>`,
        'Proof of Address is older that the maximum requested of 3 months':
            `<ul>
                <li>If you have recently moved and you have not updated your address yet, the old address can be accepted as long as the submitted document's date stamp is less than 3 months old</li>
                <li>P.O. box addresses are not accepted</li>
            </ul>`,
    }

    reasonsKeys: string[];

    rejectReason: string = "";

    moreInfo: string = "";

    constructor(public data: DataService, private modalService: NgbModal, public applicationService: ApplicationService, private route: ActivatedRoute, private router: Router, private authService: AuthService) {
        this.reasonsKeys = Object.keys(this.reasons);       
    }

    ngOnInit(): void {

        this.rejectReason = null;
        this.reasonDesc = null;

        this.appId = this.route.snapshot.paramMap.get('appId') || "start";

        this.getDetails();
    }

    getDetails() {

        this.applicationService.getApplicationInfo(this.appId).subscribe((r) => {
            this.data.selectApp = r;
        });

        this.applicationService.getUserApp(this.appId).subscribe((r) => {
            this.data.user = r.user;
            this.data.isAdmin = r.isAdmin;
        });

        this.applicationService.getAddress(this.appId).subscribe((r) => {
            this.data.address = r;
        });

        this.applicationService.getShippingAddress(this.appId).subscribe((r) => {
            this.data.shippingAddress = r;
        });

        this.applicationService.getCardOrderList(this.appId).subscribe((r) => {
            this.data.cardOrders = r;
        });

    }

    paid() {
        this.updateAppStatus('Paid');
        this.data.selectApp.status = 'Paid';
    }

    private updateAppStatus(status: string) {
        this.applicationService.updateStatus({ applicationId: this.appId, status: status, comments: this.data.selectApp.comments }).subscribe((r) => {
            
        });
    }

    registerAndSendForKYCApproval() {
        this.applicationService.controlKYC('api/applications/registerAndSendForKYCApproval',{ applicationId: this.appId }).subscribe((r) => {
            this.data.selectApp.providerUserId = r.clientId;
        });
    }

    createAccountKYC() {
        this.applicationService.controlKYC('api/applications/createAccountKYC', this.data.selectApp).subscribe((r) => {
        });
    }

    resendForKYCApproval() {
        this.applicationService.controlKYC('api/applications/resendForKYCApproval', { applicationId: this.appId }).subscribe((r) => {
            this.data.selectApp.status = r.appStatus;
            this.data.selectApp.comments = r.comments
        });
    }

    shipped(): void {
        this.updateAppStatus('Shipped');
    }

    delete() {
        this.applicationService.controlKYC('api/applications/delete', { applicationId: this.appId }).subscribe((r) => {

        });
    }
    
    registerAndCreateAccountNonKYC() {
        this.applicationService.controlKYC('api/applications/registerAndCreateAccountNonKYC', { applicationId: this.appId }).subscribe((r) => {
            this.data.selectApp.status = r.appStatus;
            this.data.selectApp.providerUserId = r.clientId;
        });
    }

    showModal(content) {
        this.modalService.open(content);
    }

    showModalDelete(contentDelete) {
        this.modalService.open(contentDelete);
    }

    reject() {
        this.applicationService.updateStatus({
            applicationId: this.appId, status: 'Rejected',
            statusDesc: {
                    reason: this.rejectReason,
                    description: this.rejectReason == '' ? '' : this.reasons[this.rejectReason],
                    moreInfo: this.moreInfo
            }
        }).subscribe((r) => {
            this.data.selectApp.status = r.appStatus;
            this.data.selectApp.comments = r.appStatusComments;
        });
    }
}
