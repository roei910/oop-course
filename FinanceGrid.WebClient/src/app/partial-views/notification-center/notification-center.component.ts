import { Component } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { Stock } from 'src/models/stocks/stock';
import { StockNotification } from 'src/models/users/stock-notification';
import { AuthenticationService } from 'src/services/authentication.service';
import { StockService } from 'src/services/stock.service';
import { ToastService } from 'src/services/toast.service';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-notification-center',
  templateUrl: './notification-center.component.html',
  styleUrls: ['./notification-center.component.css']
})
export class NotificationCenterComponent {
  stockNotifications: StockNotification[] = [];
  stocks: Stock[] = [];

  constructor(private userService: UserService,
    private stockService: StockService,
    private authenticationService: AuthenticationService,
    private toastService: ToastService,
    private confirmationService: ConfirmationService
  ) { }

  ngOnInit(): void {
    this.userService.getUser()
      .subscribe(user => this.stockNotifications = user.stockNotifications);

    this.stockService.getAllStocks().subscribe(stocks => {
      this.stocks = stocks;
    });
  }

  findStock(stockSymbol: string): Stock {
    let foundStock = this.stocks.find(stock => stock.symbol == stockSymbol)!;

    return foundStock;
  }

  removeNotification(notification: StockNotification) {
    this.confirmationService.confirm({
      message: 'You are deleting a notification, are you sure?',
      header: 'Notification Remove Confirmation',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: "none",
      rejectIcon: "none",
      rejectButtonStyleClass: "p-button-text",
      accept: () => {
        let email = this.authenticationService.getUserEmail()!;

        this.userService.removeStockNotification(email, notification.id!)
          .subscribe(response => {
            if (response) {
              let notificationIndex = this.stockNotifications.findIndex(stock => stock.id == notification.id);
              this.stockNotifications.splice(notificationIndex, 1);
              this.toastService.addSuccessMessage("Removed notification successfully");
            }
            else {
              this.toastService.addErrorMessage("Couldn't remove notification, please try again");
            }
          });
      }
    });
  }
}
