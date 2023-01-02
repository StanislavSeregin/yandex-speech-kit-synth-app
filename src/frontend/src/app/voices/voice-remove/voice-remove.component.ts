import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { VoicesService } from './../voices.service';
import { SpeechModel } from './../../services/speech.service';

@Component({
  selector: 'voice-remove',
  templateUrl: './voice-remove.component.html',
})
export class VoiceRemoveComponent {
  public voice: SpeechModel;
  public isLoading = false;

  constructor(
    private voicesService: VoicesService,
    private dialogRef: MatDialogRef<VoiceRemoveComponent>,
    @Inject(MAT_DIALOG_DATA) private data: SpeechModel
  ) {
    this.voice = Object.assign({}, this.data);
  }

  public close(): void {
    this.dialogRef.close();
  }

  public async remove(): Promise<void> {
    this.isLoading = true;
    await this.voicesService.remove(this.voice);
    this.dialogRef.close();
  }
}
