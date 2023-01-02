import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { VoicesService } from './voices.service';

@Component({
  templateUrl: './voices.component.html'
})
export class VoicesComponent implements OnInit, OnDestroy {
  private subscriptions: Array<Subscription> = [];

  constructor(
    public voicesService: VoicesService
  ) { }

  ngOnInit(): void {
    this.subscriptions = [];
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  public add(): void {
    this.voicesService.openEditor({
      text: null,
      transcription: null,
    });
  }
}