import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ChartModule } from 'primeng/chart';
import { DropdownModule } from 'primeng/dropdown';
import { MenuModule } from 'primeng/menu';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { ListboxModule } from 'primeng/listbox';
import { PanelModule } from 'primeng/panel';
import { MenubarModule } from 'primeng/menubar';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { CalendarModule } from 'primeng/calendar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AsyncPipe } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { NotificationCenterComponent } from './partial-views/notification-center/notification-center.component';
import { InsightsComponent } from './partial-views/insights/insights.component';
import { interceptConnection } from 'src/interceptors/connection.interceptor';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { interceptLoader } from 'src/interceptors/loading.interceptor';
import { LoadingComponent } from './partial-views/loading/loading.component';
import { NotFoundComponent } from './partial-views/not-found/not-found.component';
import { UserStocksComponent } from './partial-views/user-stocks/user-stocks.component';
import { LoginComponent } from './partial-views/login/login.component';
import { RegisterComponent } from './partial-views/register/register.component';
import { UserInformationComponent } from './partial-views/user-information/user-information.component';
import { SearchComponent } from './partial-views/search/search.component';
import { HomeComponent } from './partial-views/home/home.component';
import { StockSharesComponent } from './partial-views/stock-shares/stock-shares.component';
import { StockDetailsComponent } from './partial-views/stock-details/stock-details.component';
import { PortfolioDetailsComponent } from './partial-views/portfolio-details/portfolio-details.component';
import { StocksTableComponent } from './partial-views/stocks-table/stocks-table.component';
import { InputTextModule } from 'primeng/inputtext';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    UserStocksComponent,
    NotFoundComponent,
    LoginComponent,
    RegisterComponent,
    SearchComponent,
    StockDetailsComponent,
    StockSharesComponent,
    NotificationCenterComponent,
    InsightsComponent,
    LoadingComponent,
    UserStocksComponent,
    LoginComponent,
    RegisterComponent,
    UserInformationComponent,
    SearchComponent,
    StockDetailsComponent,
    PortfolioDetailsComponent,
    StocksTableComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ChartModule,
    FormsModule,
    AsyncPipe,
    DropdownModule,
    BrowserAnimationsModule,
    MenubarModule,
    MenuModule,
    ToastModule,
    TableModule,
    ListboxModule,
    PanelModule,
    ButtonModule,
    DialogModule,
    CalendarModule,
    InputTextModule,
    ConfirmDialogModule
  ],
  providers: [
    provideHttpClient(
      withInterceptors([
        interceptConnection,
        interceptLoader
      ])
    ),
    MessageService,
    ConfirmationService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }