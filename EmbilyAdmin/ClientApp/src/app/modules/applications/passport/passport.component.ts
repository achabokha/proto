import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../../services/data.service';
import { ApplicationService } from '../../../services/application.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
    selector: 'app-app-passport',
    templateUrl: './passport.component.html',
    styleUrls: ['./passport.component.css']
})
export class PassportComponent implements OnInit {

    images: string[] = new Array();
    //pdf: any;

    @Input() appDetails: any;
    //@ViewChild('pdfViewer') pdfViewer

    constructor(public data: DataService,
        private applicationService: ApplicationService,
        public modalService: NgbModal) {
    }

    ngOnInit(): void {
        this.getFiles();
    }

    update(): void {
        this.applicationService.updateApplicationInfo(this.data.selectApp).subscribe(result => {
        });
    }

    open(modalContent) {
        this.modalService.open(modalContent, { size: 'lg' })
    }

    getFiles(): void {
        this.data.selectApp.documents.forEach(value => {
            if (value.documentType == 'ProofOfID' || value.documentType == 'Selfie') {
                //if (value.fileType = "application/pdf") {
                //    this.applicationService.getFilePdf(value.documentId).subscribe(result => {
                //        this.pdfViewer.pdfSrc = result.pdf;
                //        this.pdfViewer.refresh();
                //    });
                //}
                if (value.fileType == "image/jpeg" || value.fileType == "image/png") {
                    this.applicationService.getFileImage(value.documentId).subscribe(result => {
                        this.images[value.documentType] = result;
                    });
                }

            }
        }); 
    }
}
