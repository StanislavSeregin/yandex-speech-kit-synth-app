import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { FormControl, FormGroup } from '@angular/forms';
import { VoicesService } from './../voices.service';

@Component({
  selector: 'voice-filtering',
  templateUrl: './voice-filtering.component.html',
})
export class VoiceFilteringComponent implements OnInit, OnDestroy {
  public filterForm = new FormGroup({
    text: new FormControl<string>(''),
    transcription: new FormControl<string>('')
  });

  public isLoading = false;

  private subscriptions: Array<Subscription> = [];

  constructor(
    private voicesService: VoicesService
  ) { }

  ngOnInit(): void {
    this.subscriptions = [
      this.voicesService.isLoading$.subscribe(isLoading => {
        this.isLoading = isLoading;
      }),
      this.filterForm.valueChanges.subscribe(valueChange => {
        const formValue = valueChange as {
          text: string,
          transcription: string
        };

        this.voicesService.changeFiltering({
          text: formValue.text ?? '',
          transcription: formValue.transcription ?? ''
        });
      })
    ];
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}