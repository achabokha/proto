import { Component, OnInit, ViewChild } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { ActivatedRoute } from '@angular/router';
import { NgbTabset } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-legal',
    templateUrl: './legal.component.html',
    styleUrls: ['./legal.component.scss'],
})

export class LegalComponent implements OnInit {

    @ViewChild('tabs') public tabs: any;


    constructor(private route: ActivatedRoute) {
        setTimeout(() =>
            this.route.params.subscribe(params => {
                //console.log(params);
                let section = params.section;
                if (section) {
                    this.tabs.select(section)
                }
            }), 250); // hack, tabset is too large, takes time to build up dom -- 
    }

    ngOnInit(): void {
    }
}
