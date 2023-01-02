import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormControl, FormGroup } from '@angular/forms';
import { environment } from './../../../environments/environment';
import { VoicesService } from './../voices.service';
import { SpeechModel } from './../../services/speech.service';
import { Subscription } from 'rxjs';

type ContentFormData = Pick<SpeechModel, 'text' | 'transcription'>;

@Component({
  selector: 'voice-editor',
  templateUrl: './voice-editor.component.html',
})
export class VoiceEditorComponent implements OnDestroy, OnInit {
  public voice: SpeechModel;
  public form: FormGroup;
  public voiceUrl?: string;
  private subscriptions: Array<Subscription> = [];

  constructor(
    private voicesService: VoicesService,
    private dialogRef: MatDialogRef<VoiceEditorComponent>,
    @Inject(MAT_DIALOG_DATA) private data: SpeechModel
  ) {
    this.voice = Object.assign({}, this.data);
    this.form = new FormGroup({
      text: new FormControl(this.voice.text),
      transcription: new FormControl(this.voice.transcription)
    });
  }

  ngOnInit(): void {
    if (this.voice.fileId) {
      this.voiceUrl = this.voicesService.getSpeechUrl(this.voice.fileId);
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  public async save(): Promise<void> {
    this.form.disable();
    const formData = this.form.value as ContentFormData;
    const voice = Object.assign({}, this.voice, formData);
    await this.voicesService.save(voice);
    this.form.enable();
    this.dialogRef.close();
  }
}
