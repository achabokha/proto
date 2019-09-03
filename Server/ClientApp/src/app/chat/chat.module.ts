import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChatRoutingModule } from './chat-routing.module';
import { ContainerComponent } from './container/container.component';
import { MaterialModule } from '../material-modules';
import { HttpClientModule } from '@angular/common/http';
import { NgChatModule } from 'ng-chat';


@NgModule({
  declarations: [ContainerComponent],
  imports: [
    CommonModule,
    ChatRoutingModule,
    HttpClientModule,
    NgChatModule,
    MaterialModule
  ]
})
export class ChatModule { }
