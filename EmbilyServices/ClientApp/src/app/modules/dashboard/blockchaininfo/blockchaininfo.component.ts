import { Component, OnInit } from '@angular/core';
import { BlockchaininfoService } from 'src/app/services/blockchaininfo.service';

@Component({
    selector: 'app-blockchaininfo',
    templateUrl: './blockchaininfo.component.html',
    styleUrls: ['./blockchaininfo.component.css']
})
export class BlockchaininfoComponent implements OnInit {

    info: any;

    searchInput: string | undefined;
    findmessage: string = "";

    constructor(private service: BlockchaininfoService) {
    }

    ngOnInit(): void {
        this.service.getBlockchainInfo().subscribe((info) => {
            this.info = info;
        });
    }

    gotoExplorer() {
        if (window && this.searchInput) {
            let type = this.searchInput.length > 35 ? 'tx' : 'address';
            window.open(`https://live.blockcypher.com/btc/${type}/${this.searchInput}/`, "_blank");
        }
    }
}
