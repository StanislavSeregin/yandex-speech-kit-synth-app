import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatDialogModule } from '@angular/material/dialog';

import { VoicesComponent } from './voices.component';
import { VoiceFilteringComponent } from './voice-filtering/voice-filtering.component';
import { VoiceListComponent } from './voice-list/voice-list.component';
import { VoicePagingComponent } from './voice-paging/voice-paging.component';
import { VoiceEditorComponent } from './voice-editor/voice-editor.component';
import { VoiceRemoveComponent } from './voice-remove/voice-remove.component';


import { VoicesService } from './voices.service';

import { VoicesRoutes } from './voices.routing';

@NgModule({
  declarations: [
    VoicesComponent,
    VoiceFilteringComponent,
    VoiceListComponent,
    VoicePagingComponent,
    VoiceEditorComponent,
    VoiceRemoveComponent
  ],
  imports: [
    VoicesRoutes,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatPaginatorModule,
    MatIconModule,
    MatButtonModule,
    MatTableModule,
    MatSortModule,
    MatDialogModule
  ],
  providers: [
    VoicesService
  ]
})
export class VoicesModule { }