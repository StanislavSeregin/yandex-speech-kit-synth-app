import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { environment } from './../../environments/environment';

const speechListUrl = `${environment}api/speech/get-list`;
const createOrUpdateSpeechUrl = `${environment}api/speech/create-or-update`;
const removeSpeechUrl = `${environment}api/speech/remove`;

export type SpeechModel = {
  text?: string | null,
  transcription?: string | null,
  fileId?: string | null
}

export enum OrderFields {
  transcription = 1,
  text = 2
}

export enum OrderDirections {
  ascending = 0,
  descending = 1
}

export type OrderQuery = {
  queryingField: OrderFields,
  orderDirection: OrderDirections
}

export type GettingSpeechListRequest = {
  skip: number,
  take: number,
  text?: string | null,
  transcription?: string | null,
  orderQueries: Array<OrderQuery>
}

export type GettingSpeechListResponse = {
  models: Array<SpeechModel>,
  total: number
}

export type CreateOrUpdateSpeechRequest = {
  text: string,
  transcription: string
}

export type CreateOrUpdateSpeechResponse = {
  model: SpeechModel
}

export type RemoveSpeechRequest = {
  text: string
}

@Injectable({
  providedIn: 'root'
})
export class SpeechService {
  constructor(
    private http: HttpClient
  ) { }

  public async getSpeechList(
    request: GettingSpeechListRequest
  ): Promise<GettingSpeechListResponse> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const gettingSpeechListResponseObservable = this.http.post<GettingSpeechListResponse>(
      speechListUrl,
      request,
      { headers: headers }
    );

    const response = await lastValueFrom(gettingSpeechListResponseObservable);
    return response;
  }

  public async createOrUpdateSpeech(
    request: CreateOrUpdateSpeechRequest
  ): Promise<CreateOrUpdateSpeechResponse> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const createOrUpdateSpeechResponseObservable = this.http.post<CreateOrUpdateSpeechResponse>(
      createOrUpdateSpeechUrl,
      request,
      { headers: headers }
    );

    const response = await lastValueFrom(createOrUpdateSpeechResponseObservable);
    return response;
  }

  public async removeSpeech(
    request: RemoveSpeechRequest
  ): Promise<void> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const removeVoiceResponseObservable = this.http.post(
      removeSpeechUrl,
      request,
      { headers: headers }
    );

    await lastValueFrom(removeVoiceResponseObservable);
  }
}
