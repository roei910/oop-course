import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Share } from 'src/models/shares/share';
import { SharePurchase } from 'src/models/shares/share-purchase';
import { ShareSale } from 'src/models/shares/share-sale';
import { StockListDetails } from 'src/models/stocks/stock-list-details';

@Injectable({
  providedIn: 'root'
})
export class SharesService {
  private shareEndPointUrl: string = `${environment.server_url}/Share`;

  constructor(private httpClient: HttpClient) { }

  addUserShare(sharePurchase: SharePurchase): Observable<Share | null> {
    let res = this.httpClient
      .post<Share>(this.shareEndPointUrl, sharePurchase, {
        observe: 'response'
      })
      .pipe(map(res => res.status == 200 ? res.body : null));

    return res;
  }

  removeUserShare(shareSale: ShareSale): Observable<boolean> {
    return this.httpClient
      .delete<boolean>(this.shareEndPointUrl, {
        body: shareSale,
        observe: 'response'
      })
      .pipe(map(res => res.status == 200));
  }

  addWatchingStock(email: string, listName: string, stockSymbol: string):
    Observable<boolean> {
    let res = this.httpClient
      .post<boolean>(`${this.shareEndPointUrl}/watching-stock`,
        {
          email,
          listName,
          stockSymbol
        },
        {
          observe: 'response'
        }
      )
      .pipe(map(res => res.status == 200));

    return res
  }

  removeWatchingStock(email: string, listName: string, stockSymbol: string):
    Observable<boolean> {
    let res = this.httpClient
      .delete<boolean>(`${this.shareEndPointUrl}/watching-stock`,
        {
          body: {
            email,
            listName,
            stockSymbol
          },
          observe: 'response'
        }
      )
      .pipe(map(res => res.status == 200));

    return res;
  }

  updateWatchingStockNote(email: string, listName: string, stockSymbol: string, note: string):
    Observable<boolean> {
    let res = this.httpClient
      .patch<boolean>(`${this.shareEndPointUrl}/watching-stock-note`,
        {
          email,
          listName,
          stockSymbol,
          note
        },
        {
          observe: 'response'
        }
      )
      .pipe(map(res => res.status == 200));

    return res
  }

  addUserList(stockListDetails: StockListDetails): Observable<boolean> {
    let res = this.httpClient
      .post<boolean>(`${this.shareEndPointUrl}/list`,
        stockListDetails, {
        observe: 'response'
      }
      )
      .pipe(map(res => res.status == 200));

    return res;
  }

  removeUserList(stockListDetails: StockListDetails): Observable<boolean> {
    let res = this.httpClient
      .delete<boolean>(`${this.shareEndPointUrl}/list`,
        {
          body: stockListDetails,
          observe: 'response'
        }
      )
      .pipe(map(res => res.status == 200));

    return res;
  }
}