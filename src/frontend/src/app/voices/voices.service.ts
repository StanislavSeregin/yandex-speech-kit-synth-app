import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject, debounceTime, map, combineLatest } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { environment } from './../../environments/environment';
import { VoiceEditorComponent } from './voice-editor/voice-editor.component';
import { VoiceRemoveComponent } from './voice-remove/voice-remove.component';

import {
  SpeechService as SpeechHttpService,
  GettingSpeechListRequest,
  OrderFields,
  OrderDirections,
  SpeechModel
} from './../services/speech.service';

export type Paging = Pick<GettingSpeechListRequest, 'skip' | 'take'>;
export type Ordering = Pick<GettingSpeechListRequest, 'orderQueries'>;
export type Filtering = Pick<GettingSpeechListRequest, 'text' | 'transcription'>;

@Injectable()
export class VoicesService {
  private pagingSubject = new BehaviorSubject<Paging>({ skip: 0, take: 10 });
  private filteringSubject = new BehaviorSubject<Filtering>({});
  private orderingSubject = new BehaviorSubject<Ordering>({
    orderQueries: [
      {
        queryingField: OrderFields.text,
        orderDirection: OrderDirections.descending
      }
    ]
  });

  private isLoadingSubject = new BehaviorSubject<boolean>(false);
  private totalSubject = new BehaviorSubject<number>(0);
  private voiceModelListSubject = new BehaviorSubject<Array<SpeechModel>>([]);
  private externalActionSubject = new Subject<SpeechModel>();

  public isLoading$ = this.isLoadingSubject.asObservable();
  public total$ = this.totalSubject.asObservable();
  public voiceModelList$ = this.voiceModelListSubject.asObservable();
  public externalAction$ = this.externalActionSubject.asObservable();

  constructor(
    private voicesHttpService: SpeechHttpService,
    private dialog: MatDialog
  ) {
    combineLatest([
      this.pagingSubject,
      this.orderingSubject,
      this.filteringSubject
    ]).pipe(
      debounceTime(300),
      map(combined => {
        const [paging, ordering, filtering] = combined;
        return this.mapToGettingvoiceRequest(paging, ordering, filtering);
      })
    ).subscribe(async gettingvoiceRequest => {
      this.isLoadingSubject.next(true);
      const response = await this.voicesHttpService.getSpeechList(gettingvoiceRequest);
      this.totalSubject.next(response.total);
      this.voiceModelListSubject.next(response.models);
      this.isLoadingSubject.next(false);
    });
  }

  public changePaging(
    paging: Paging
  ): void {
    this.pagingSubject.next(paging);
  }

  public changeFiltering(
    filtering: Filtering
  ): void {
    this.filteringSubject.next(filtering);
  }

  public changeOrdering(
    ordering: Ordering
  ): void {
    this.orderingSubject.next(ordering);
  }

  public mapToGettingvoiceRequest(
    paging: Paging,
    ordering: Ordering,
    filtering: Filtering
  ): GettingSpeechListRequest {
    return {
      skip: paging.skip,
      take: paging.take,
      text: filtering.text,
      transcription: filtering.transcription,
      orderQueries: ordering.orderQueries
    };
  }

  public openEditor(
    voice: SpeechModel
  ): void {
    this.dialog.open(
      VoiceEditorComponent,
      {
        width: '1200px',
        data: voice
      }
    );
  }

  public getSpeechUrl(
    fileId: string
  ): string {
    return `${environment.baseUrl}api/speech/file?${fileId}`;
  }

  public async save(
    voice: SpeechModel
  ): Promise<SpeechModel> {
    if (!voice.text) {
      return voice;
    }

    voice.transcription = voice.transcription ?? voice.text;
    const response = await this.voicesHttpService.createOrUpdateSpeech({
      text: voice.text,
      transcription: voice.transcription
    });

    this.reloadPage();
    return response.model;
  }

  public openRemoveDialog(
    voice: SpeechModel
  ): void {
    this.dialog.open(
      VoiceRemoveComponent,
      {
        data: voice
      }
    );
  }

  public async remove(
    voice: SpeechModel
  ): Promise<void> {
    if (voice.text) {
      await this.voicesHttpService.removeSpeech({
        text: voice.text
      });

      this.reloadPage();
    }
  }

  private reloadPage(): void {
    const paging = this.pagingSubject.value;
    this.changePaging(paging);
  }
}
