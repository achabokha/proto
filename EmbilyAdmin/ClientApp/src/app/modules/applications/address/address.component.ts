import { Component, OnInit, Input } from '@angular/core';
import { DataService } from '../../../services/data.service';
import { ApplicationService } from '../../../services/application.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';


@Component({
    selector: 'app-app-address',
    templateUrl: './address.component.html',
    styleUrls: ['./address.component.css']
})

export class AddressComponent implements OnInit {

    images = [];

    constructor(public data: DataService,
        private applicationService: ApplicationService,
        public modalService: NgbModal) {
    }

    ngOnInit(): void {
        this.getFiles();
    }

    update() {
        this.applicationService.updateAddress(this.data.address).subscribe(result => {

        });
    }

    open(modalContent) {
        this.modalService.open(modalContent, { size: 'lg' })
    }

    getFiles(): void {
        this.data.selectApp.documents.forEach(value => {
            if (value.documentType == 'ProofOfAddress') {
                //if (value.fileType = "application/pdf") {
                //    this.applicationService.getFilePdf(value.documentId).subscribe(result => {
                //        this.pdfViewer.pdfSrc = result.pdf;
                //        this.pdfViewer.refresh();
                //    });
                //}
                if (value.fileType == "image/jpeg" || value.fileType == "image/png") {
                    this.applicationService.getFileImage(value.documentId).subscribe(result => {
                        this.images = result;
                    });
                }

            }
        });
    }
}
