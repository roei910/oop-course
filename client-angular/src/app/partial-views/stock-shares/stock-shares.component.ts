import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ConfirmationService } from 'primeng/api';
import { Share } from 'src/models/shares/share';
import { SharePurchase } from 'src/models/shares/share-purchase';
import { ShareSale } from 'src/models/shares/share-sale';
import { WatchingStock } from 'src/models/stocks/watching-stock';
import { AuthenticationService } from 'src/services/authentication.service';
import { SharesService } from 'src/services/shares.service';
import { ToastService } from 'src/services/toast.service';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-stock-shares',
  templateUrl: './stock-shares.component.html',
  styleUrls: ['./stock-shares.component.css']
})
export class StockSharesComponent {
  userEmail!: string | null;
  symbol!: string;
  listName!: string;
  watchingStock : WatchingStock | undefined;
  visibleAddShareDialog : boolean = false;
  purchaseDate: Date | undefined;
  numberOfShares: number | undefined;
  stockPrice: number | undefined;
  shares: Share[] = [];

  constructor(private activatedRoute: ActivatedRoute,
    private userService: UserService,
    private authenticationService: AuthenticationService,
    private shareService: SharesService,
    private toastService: ToastService,
    private confirmationService: ConfirmationService
  ){ }

  ngOnInit() {
    this.activatedRoute.queryParams.subscribe(params => {
      this.symbol = params['stockSymbol'];
      this.listName = params['listName'];
    });

    this.userEmail = this.authenticationService.getUserEmail()!;

    this.userService.getUser().subscribe(user =>{
      this.watchingStock = user.watchingStocksByListName[this.listName][this.symbol];
      this.updateSharesList();
    });
  }

  addShare() {
    this.visibleAddShareDialog = false;

    if(this.purchaseDate == undefined)
      this.purchaseDate = new Date();

    if(this.numberOfShares == undefined || this.stockPrice == undefined){
      return;
    }

    let sharePurchase: SharePurchase = {
      stockSymbol: this.symbol,
      amount: this.numberOfShares,
      purchasingPrice: this.stockPrice,
      userEmail: this.userEmail!,
      listName: this.listName!,
      purchaseDate: this.purchaseDate
    };

    this.shareService.addUserShare(sharePurchase)
      .subscribe(res => {
        if(res)
        {
          this.watchingStock!.purchaseGuidToShares[res.id!] = res
          this.updateSharesList();
        }
        else
          this.toastService.addErrorMessage("couldnt add share, something went wrong");
      });
  }

  removeShare(purchaseId: string, stockSymbol: string){
    this.confirmationService.confirm({
      message: 'You are deleting a share, are you sure?',
      header: 'Share Remove Confirmation',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: "none",
      rejectIcon: "none",
      rejectButtonStyleClass: "p-button-text",
      accept: () => {
        let shareSale: ShareSale = {
          listName: this.listName,
          sharePurchaseGuid: purchaseId,
          stockSymbol: stockSymbol,
          userEmail: this.userEmail!
        };
    
        this.shareService.removeUserShare(shareSale)
        .subscribe(res => {
          if(res)
          {
            delete(this.watchingStock?.purchaseGuidToShares[purchaseId]);
            this.updateSharesList();
          }
          else
            this.toastService.addErrorMessage("couldnt remove share, something went wrong");
        });
      }
    });
  }

  updateSharesList(){
    this.shares = Object.keys(this.watchingStock!.purchaseGuidToShares).map(purchaseId => this.watchingStock!.purchaseGuidToShares[purchaseId]);
  }
}