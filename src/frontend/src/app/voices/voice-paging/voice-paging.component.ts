import { Component, ViewChild, OnDestroy, AfterViewInit, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';
import { VoicesService } from './../voices.service';

@Component({
  selector: 'voice-paging',
  templateUrl: './voice-paging.component.html',
})
export class VoicePagingComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(MatPaginator) paginator?: MatPaginator;

  public total = 0;
  public isLoading = false;

  private subscriptions: Array<Subscription> = [];

  constructor(
    private voicesService: VoicesService
  ) { }

  ngOnInit(): void {
    this.subscriptions = [
      this.voicesService.total$.subscribe(total => {
        this.total = total;
      }),
      this.voicesService.isLoading$.subscribe(isLoading => {
        this.isLoading = isLoading;
      })
    ];
  }

  ngAfterViewInit(): void {
    if (this.paginator?.page) {
      this.subscriptions.push(
        this.paginator.page.subscribe(page => {
          this.voicesService.changePaging({
            skip: page.pageIndex * page.pageSize,
            take: page.pageSize
          });
        })
      );
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
