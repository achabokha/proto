import { Component, Input } from '@angular/core';
import { AffiliateService } from '../../../services/affiliate.service';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-affiliate-tokens',
    templateUrl: './affiliate-tokens.component.html',
    styleUrls: ['./affiliate-tokens.component.css']
})
export class AffiliateTokensComponent {
    
    @Input() tokens: any;

    token: any;
    descriptionToken: string = "";
    spinner: boolean = false;
    status: string = 'none';

    constructor(private affiliateService: AffiliateService, public modalService: NgbModal) {
        this.getData();
    }

    getData(): any {
        this.tokens = null;
        this.affiliateService.getTokenList().subscribe(tl => {
            this.tokens = tl.tokenList;
        });
    }

    createToken() {
        this.spinner = true;
        this.affiliateService.createToken(this.descriptionToken).subscribe(t => {
            this.descriptionToken = "";
            this.tokens = t.tokenList;
            this.spinner = false;
        });
    }

    deactivateToken(tokenId: string): void {
        this.affiliateService.deactivateToken(tokenId).subscribe(t => {
            this.tokens = t.tokenList;
        });
    }

    copyTokenLink(token: string): void {
        let selBox = document.createElement('input'); // hidden input, no need for extra hiding activities --
        selBox.setAttribute('value', `https://services.embily.com/home/${token}`);
        document.body.appendChild(selBox);
        selBox.select();
        document.execCommand('copy');
        document.body.removeChild(selBox);
    }
}
