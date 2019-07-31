import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LegalAndHelpRoutingModule } from './legal-and-help-routing.module';
import { SharedModule } from '../shared/shared.module';
import { FeesComponent } from './fees/fees.component';
import { FaqComponent } from './faq/faq.component';
import { LegalComponent } from './legal/legal.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        CommonModule,
        LegalAndHelpRoutingModule,
        SharedModule,
        NgbModule
    ],
    declarations: [
        FeesComponent,
        FaqComponent,
        LegalComponent
    ]
})
export class LegalAndHelpModule { }
