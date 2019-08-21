import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { SearchRoutingModule } from "./search-routing.module";
import { SearchComponent } from "./search.component";
import { FlexLayoutModule } from "@angular/flex-layout";
import { MaterialModule } from '../material-modules';

@NgModule({
    declarations: [SearchComponent],
    imports: [CommonModule, SearchRoutingModule, FlexLayoutModule, MaterialModule]
})
export class SearchModule {}
