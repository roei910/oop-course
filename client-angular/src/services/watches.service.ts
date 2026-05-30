import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Share } from 'src/models/shares/share';
import { UserStockWatch } from 'src/models/stocks/user-stock-watch';

export type WatchesByList = { [listName: string]: { [stockSymbol: string]: UserStockWatch } };

@Injectable({
  providedIn: 'root'
})
export class WatchesService {
  private watchesEndPointUrl: string = `${environment.server_url}/UserStockWatch`;
  private watchesSubject = new BehaviorSubject<WatchesByList>({});

  constructor(private httpClient: HttpClient) { }

  loadWatches(email: string): void {
    this.httpClient.get<UserStockWatch[]>(this.watchesEndPointUrl, { params: { email } })
      .pipe(map(watches => this.groupWatchesByList(watches)))
      .subscribe(grouped => this.watchesSubject.next(grouped));
  }

  getWatches(): Observable<WatchesByList> {
    return this.watchesSubject.asObservable();
  }

  getWatchesByList(listName: string): Observable<{ [stockSymbol: string]: UserStockWatch }> {
    return this.watchesSubject.pipe(
      map(grouped => grouped[listName] ?? {})
    );
  }

  addWatchLocally(listName: string, stockSymbol: string, watch: UserStockWatch): void {
    const current = this.watchesSubject.value;
    if (!current[listName])
      current[listName] = {};
    current[listName][stockSymbol] = watch;
    this.watchesSubject.next({ ...current });
  }

  removeWatchLocally(listName: string, stockSymbol: string): void {
    const current = this.watchesSubject.value;
    if (current[listName]) {
      delete current[listName][stockSymbol];
      this.watchesSubject.next({ ...current });
    }
  }

  addShareLocally(listName: string, stockSymbol: string, shareId: string, share: Share): void {
    const current = this.watchesSubject.value;
    if (current[listName] && current[listName][stockSymbol]) {
      current[listName][stockSymbol].purchaseGuidToShares[shareId] = share;
      this.watchesSubject.next({ ...current });
    }
  }

  removeShareLocally(listName: string, stockSymbol: string, purchaseGuid: string): void {
    const current = this.watchesSubject.value;
    if (current[listName] && current[listName][stockSymbol]) {
      delete current[listName][stockSymbol].purchaseGuidToShares[purchaseGuid];
      this.watchesSubject.next({ ...current });
    }
  }

  updateNoteLocally(listName: string, stockSymbol: string, note: string): void {
    const current = this.watchesSubject.value;
    if (current[listName] && current[listName][stockSymbol]) {
      current[listName][stockSymbol].note = note;
      this.watchesSubject.next({ ...current });
    }
  }

  private groupWatchesByList(watches: UserStockWatch[]): WatchesByList {
    const grouped: WatchesByList = {};
    for (const watch of watches) {
      if (!grouped[watch.listName])
        grouped[watch.listName] = {};
      grouped[watch.listName][watch.stockSymbol] = watch;
    }
    return grouped;
  }
}
