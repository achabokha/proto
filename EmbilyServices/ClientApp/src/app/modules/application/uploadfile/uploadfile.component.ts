import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { createViewContainerData } from '@angular/core/src/view/refs';
import { ApplicationService } from '../../../services/application.service';
import { resetFakeAsyncZone } from '@angular/core/testing';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-uploadfile',
    templateUrl: './uploadfile.component.html',
    styleUrls: ['./uploadfile.component.css']
})
export class UploadfileComponent implements OnInit {

    docInfo: any;
    images: any;
    showUploadButton: boolean = false;

    @ViewChild('fileInput') fileInput: any;
    @ViewChild('uploadPreview') uploadPreview: any;

    public fileLabel: string = "Choose a file:";
    public filename: string = "";

    public progressStatus: string = 'select';
    public progressErrorMessage: string;
    public progressSuccessMessage: string;

    public continueDisabled: boolean = true;

    constructor(private applicationService: ApplicationService, public modalService: NgbModal) {
    }

    ngOnInit(): void {

    }

    ngOnDestroy(): void {
    }

    previewImage() {
        this.showUploadButton = true;

        var fileReader = new FileReader();
        let fileBrowser = this.fileInput.nativeElement;
        let uploadPreview = this.uploadPreview.nativeElement;

        if (fileBrowser.files[0].type.startsWith('image')) {
            fileReader.onload = function (event: any) {
                uploadPreview.src = event.target.result;
            };

            fileReader.readAsDataURL(fileBrowser.files[0]);
        }
        this.fileLabel = 'File: ' + fileBrowser.files[0].name;
        this.filename = fileBrowser.files[0].name;
    }

    getFile() {
        let fileBrowser = this.fileInput.nativeElement;

        if (fileBrowser.files && fileBrowser.files[0])
            return fileBrowser.files[0];

        return null;
    }

    upload() {
        let file = this.getFile();

        if (file) {
            const formData = new FormData();
            formData.append("documentId", this.docInfo.documentId);
            formData.append("image", file);
            formData.append("documentType", this.docInfo.documentType);
            formData.append("order", "0");
            formData.append("applicationId", this.docInfo.applicationId);

            this.progressStatus = 'progress';

            this.applicationService.uploadFile(formData)
                .subscribe(response => {
                    this.continueDisabled = false;
                    this.docInfo = response.docInfo;
                    this.progressStatus = "success";
                    this.progressSuccessMessage = "File uploaded successfully";
                    this.showUploadButton = false;
                    this.filename = "";
                    this.fileLabel = "Choose another file:";
                    this.hideImgPreview();
                    this.hideProgressBox();
                }, error => {
                    console.log(error.json());
                    this.progressStatus = "error";
                    this.progressErrorMessage = error.json().message;
                });
        }


    }

    hideImgPreview() {
        let uploadPreview = this.uploadPreview.nativeElement;
        uploadPreview.src = '';
    }

    hideProgressBox(): void {
        setTimeout(() => this.progressStatus = 'select', 5000);
    }

    open(modalContent) {

        if (this.docInfo.fileType == "image/jpeg" || this.docInfo.fileType == "image/png") {
            this.applicationService.getFileImage(this.docInfo.documentId).subscribe(result => {
                this.images = result;
            });
        }

        this.modalService.open(modalContent, { size: 'lg' })

    }
}
