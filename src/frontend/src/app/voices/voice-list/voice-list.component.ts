import { Component, ViewChild, OnDestroy, AfterViewInit, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { MatSort, Sort } from '@angular/material/sort';
import { VoicesService, Ordering } from './../voices.service';
import { SpeechModel, OrderDirections, OrderFields } from './../../services/speech.service';

@Component({
  selector: 'voice-list',
  templateUrl: './voice-list.component.html',
})
export class VoiceListComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(MatSort) sort?: MatSort;

  public displayedColumns: Array<string> = ['text', 'transcription', 'createdAt', 'updatedAt', 'actions'];

  public data: Array<SpeechModel> = [];
  public isLoading = false;

  private subscriptions: Array<Subscription> = [];

  constructor(
    private voicesService: VoicesService
  ) { }

  ngOnInit(): void {
    this.subscriptions = [
      this.voicesService.voiceModelList$.subscribe(voiceModelList => {
        this.data = voiceModelList;
      }),
      this.voicesService.isLoading$.subscribe(isLoading => {
        this.isLoading = isLoading;
      })
    ];
  }

  ngAfterViewInit(): void {
    if (this.sort?.sortChange) {
      this.subscriptions.push(
        this.sort.sortChange.subscribe(sort => {
          const ordering = this.mapOrdering(sort);
          this.voicesService.changeOrdering(ordering);
        })
      );
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  private mapOrdering(
    sort: Sort
  ): Ordering {
    let orderDirection: OrderDirections | null;
    switch (sort.direction) {
      case 'asc':
        orderDirection = OrderDirections.ascending;
        break;

      case 'desc':
        orderDirection = OrderDirections.descending;
        break;

      default:
        orderDirection = null;
        break;
    }

    let orderFields: OrderFields | null;
    switch (sort.active) {
      case 'text':
        orderFields = OrderFields.text
        break;

      case 'transcription':
        orderFields = OrderFields.transcription
        break;

      default:
        orderFields = null;
        break;
    }

    return orderDirection !== null && orderFields !== null
      ? {
        orderQueries: [{
          orderDirection: orderDirection,
          queryingField: orderFields
        }]
      }
      : {
        orderQueries: []
      };
  }

  public openEditor(
    voice: SpeechModel
  ): void {
    this.voicesService.openEditor(voice);
  }

  public openRemoveDialog(
    voice: SpeechModel
  ): void {
    this.voicesService.openRemoveDialog(voice);
  }

  public async playVoice(
    voice: SpeechModel
  ): Promise<void> {
    if (voice.fileId) {
      const voiceUrl = this.voicesService.getSpeechUrl(voice.fileId);
      const audio = new Audio(voiceUrl);
      audio.play();
    }
  }
}
