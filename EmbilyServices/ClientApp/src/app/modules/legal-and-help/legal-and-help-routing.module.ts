import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";
import { FaqComponent } from './faq/faq.component';
import { FeesComponent } from './fees/fees.component';
import { LegalComponent } from './legal/legal.component';


const routes: Routes = [
    {
        path: "",
        children: [
            { path: 'faqs', component: FaqComponent },
            { path: 'fees', component: FeesComponent },
            { path: 'section/:section', component: LegalComponent },

        ]
    }
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forChild(routes)]
})
export class LegalAndHelpRoutingModule {
}
