import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService } from 'primeng/api';
import { StockDetails } from 'src/interfaces/stock-details';
import { Stock } from 'src/models/stocks/stock';
import { WatchingStock } from 'src/models/stocks/watching-stock';
import { AuthenticationService } from 'src/services/authentication.service';
import { SharesService } from 'src/services/shares.service';
import { ToastService } from 'src/services/toast.service';

@Component({
  selector: 'app-portfolio-details',
  templateUrl: './portfolio-details.component.html',
  styleUrls: ['./portfolio-details.component.css']
})
export class PortfolioDetailsComponent {
  @Input('watchingStocks')
  watchingStocks!: { [stockSymbol: string]: WatchingStock };

  @Input('stocksDictionary')
  stocksDictionary!: { [stockSymbol: string]: Stock };

  @Input('listName')
  listName!: string;

  note: string = '';
  symbol: string = '';
  email: string;
  watchingStockLists: StockDetails[] = [];
  visibleDialog: boolean = false;

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    private shareService: SharesService,
    private toastService: ToastService,
    private confirmationService: ConfirmationService
  ) {
    this.email = this.authenticationService.getUserEmail()!;
  }
  
  ngOnChanges(): void {
    this.updateWatchingStocks();
  }

  openStockNoteDialog(currentSymbol: string, currentNote: string) {
    this.note = currentNote;
    this.symbol = currentSymbol;
    this.visibleDialog = true;
  }

  updateStockNote() {
    this.visibleDialog = false;

    this.shareService.updateWatchingStockNote(this.email, this.listName, this.symbol, this.note)
      .subscribe(res => {
        if (res) {
          this.watchingStocks[this.symbol].note = this.note!;
          this.updateWatchingStocks();
        }
        else
          this.toastService.addErrorMessage("error while updating note");
      });
  }

  getKeys(dictionary: any) {
    let keys = Object.keys(dictionary);

    return keys;
  }

  countShares(watchingStock: WatchingStock) {
    let sum = 0;
    let keys = Object.keys(watchingStock.purchaseGuidToShares);

    keys.forEach((purchaseGuid: string) =>
      sum += watchingStock.purchaseGuidToShares[purchaseGuid].amount);

    if(sum == 0)
      return "";

    return sum.toString();
  }

  redirectToSharesScreen(stockSymbol: string) {
    this.confirmationService.confirm({
      message: 'redirecting to shares screen',
      header: 'Share Screen Redirection',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: "none",
      rejectIcon: "none",
      rejectButtonStyleClass: "p-button-text",
      acceptLabel: "Continue",
      rejectLabel: "Cancel",
      accept: () => this.router.navigate([this.router.url, 'shares'],
        { queryParams: { stockSymbol: stockSymbol, listName: this.listName } })
    });
  }

  mapWatchingStock(stockSymbol: string, watchingStock: WatchingStock): StockDetails {
    let stock = this.stocksDictionary[stockSymbol];

    return {
      symbol: stockSymbol,
      name: stock.name,
      lastUpdate: stock.updatedTime,
      note: watchingStock.note,
      prediction: stock.analysis?.targetMeanPrice ?? 0,
      price: stock.price,
      shares: this.countShares(watchingStock)
    };
  }

  updateWatchingStocks(): void {
    if(this.watchingStocks == undefined || Object.keys(this.watchingStocks).length == 0)
      return;
    
    this.watchingStockLists = Object.keys(this.watchingStocks)
      .map(stockSymbol => this.mapWatchingStock(stockSymbol, this.watchingStocks[stockSymbol]));
  }

  removeListStock(stockSymbol: string): void {
    this.confirmationService.confirm({
      message: 'removing stock from portfolio, are you sure?',
      header: 'Stock Removal Confirmation',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: "none",
      rejectIcon: "none",
      rejectButtonStyleClass: "p-button-text",
      accept: () => {
        this.shareService.removeWatchingStock(this.email, this.listName, stockSymbol)
          .subscribe(res => {
            if (res) {
              delete this.watchingStocks[stockSymbol!];
              this.updateWatchingStocks();
            }
            else
              this.toastService.addErrorMessage("something went wrong, couldnt remove stock from list")
          });
      }
    });
  }
}