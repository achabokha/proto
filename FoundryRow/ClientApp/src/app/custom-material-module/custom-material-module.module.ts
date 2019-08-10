import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import {
  MatToolbarModule,
  MatSidenavModule,
  MatIconModule,
  MatListModule, MatGridListModule, MatCardModule, MatMenuModule, MatRadioModule, MatDialogModule, MatFormFieldModule, MatInputModule
} from '@angular/material';

@NgModule({
  imports: [MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatGridListModule,
    MatCardModule,
    MatMenuModule,
    MatRadioModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule
  ],
  exports: [MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatGridListModule,
    MatCardModule,
    MatMenuModule,
    MatRadioModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule],
})
export class CustomMaterialModuleModule { }
