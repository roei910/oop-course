import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Stock } from 'src/models/stocks/stock';
import { StockNotification } from 'src/models/users/stock-notification';
import { AuthenticationService } from 'src/services/authentication.service';
import { SharesService } from 'src/services/shares.service';
import { StockService } from 'src/services/stock.service';
import { ToastService } from 'src/services/toast.service';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-stock-details',
  templateUrl: './stock-details.component.html',
  styleUrls: ['./stock-details.component.css']
})
export class StockDetailsComponent {
  symbol?: string | undefined;
  stock?: Stock;
  data?: any;
  visibleAddNotificationDialog: boolean = false;
  visibleAddStockToListDialog: boolean = false;
  listName: string = '';
  targetPrice: number = 0;

  constructor(private activatedRoute: ActivatedRoute,
    private stockService: StockService,
    private shareService: SharesService,
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private toastService: ToastService,
    private router: Router
  ) {
    const tempData = [10, 15, 20, 25, 30];

    this.data = {
      labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May'],
      datasets: [
        {
          label: 'Stock Price History',
          data: tempData,
          fill: true,
          borderColor: '#FFFFFF'
        }
      ]
    };
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      this.symbol = params['stockSymbol'];
    });

    if (!this.symbol)
      return;

    this.stockService.getStockBySymbol(this.symbol)
      .subscribe(stock => this.stock = stock);
  }

  addStockToList(listName: string) {
    this.visibleAddStockToListDialog = false;
    let email = this.authenticationService.getUserEmail()!;

    this.shareService.addWatchingStock(email, listName, this.symbol!)
      .subscribe(res => {
        if (!res)
          this.toastService.addErrorMessage("couldn't add stock to list");
      });
  }

  addNotification() {
    this.visibleAddNotificationDialog = false;
    let email = this.authenticationService.getUserEmail()!;

    let stockNotification: StockNotification = {
      userEmail: email,
      stockSymbol: this.symbol!,
      targetPrice: this.targetPrice
    }

    this.userService.addStockNotification(stockNotification)
      .subscribe(() => {
        this.toastService.addSuccessMessage("Notification added successfully");
      });
  }

  openAddNotificationDialog() {
    this.confirmUserConnectedOrRedirect();
    this.visibleAddNotificationDialog = true;
  }

  openAddStockToListDialog() {
    this.confirmUserConnectedOrRedirect();
    this.visibleAddStockToListDialog = true;
  }

  confirmUserConnectedOrRedirect(): boolean{
    let email = this.authenticationService.getUserEmail();

    if (email == null)
      this.router.navigate(['/login']);

    return email != null;
  }

  get currentPosition(): number {
    let range = this.stock?.fiftyTwoWeekHigh! - this.stock?.fiftyTwoWeekLow!;

    return ((this.stock?.price! - this.stock?.fiftyTwoWeekLow!) / range) * 100;
  }
}