import { Component, OnInit, Input } from '@angular/core';
import { DataService } from '../../../services/data.service';

@Component({
    selector: 'app-products',
    templateUrl: './products.component.html',
    styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

    @Input() buttonToDisplay: string = 'getthiscard';

    constructor(public data: DataService) { }

    ngOnInit() {
    }
}
