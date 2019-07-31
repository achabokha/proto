import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";
import { AffiliatesComponent } from './affiliates.component';
import { InvitationsComponent } from './invitations/invitations.component';
import { AffiliateComponent } from './affiliate/affiliate.component';
import { TokensComponent } from './tokens/tokens.component';

const routes: Routes = [
    {
        path: "",
        component: AffiliatesComponent,
        children: [
            { path: "affiliatesUser", component: AffiliateComponent},
            { path: "invitations", component: InvitationsComponent },
            { path: "tokens", component: TokensComponent },
        ]
    }
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forChild(routes)]
})
export class AffiliatesRoutingModule {
}
