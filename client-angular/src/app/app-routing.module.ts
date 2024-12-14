import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { connectedUserGuard } from 'src/routeGuards/connected-user.guard';
import { StocksTableComponent } from './partial-views/stocks-table/stocks-table.component';
import { NotificationCenterComponent } from './partial-views/notification-center/notification-center.component';
import { InsightsComponent } from './partial-views/insights/insights.component';
import { NotFoundComponent } from './partial-views/not-found/not-found.component';
import { LoginComponent } from './partial-views/login/login.component';
import { RegisterComponent } from './partial-views/register/register.component';
import { UserInformationComponent } from './partial-views/user-information/user-information.component';
import { SearchComponent } from './partial-views/search/search.component';
import { HomeComponent } from './partial-views/home/home.component';
import { StockSharesComponent } from './partial-views/stock-shares/stock-shares.component';
import { StockDetailsComponent } from './partial-views/stock-details/stock-details.component';
import { UserStocksComponent } from './partial-views/user-stocks/user-stocks.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'stocks',
    children: [
      { path: 'search', component: SearchComponent },
      { path: 'information', component: StockDetailsComponent },
      { path: 'all', component: StocksTableComponent }
    ],
  },
  { path: 'insights', component: InsightsComponent, canActivate: [connectedUserGuard] },
  {
    path: 'user',
    children: [
      { path: 'stocks', component: UserStocksComponent },
      { path: 'stocks', children: [
        { path: 'shares', component: StockSharesComponent}
      ]},
      { path: 'information', component: UserInformationComponent },
      { path: 'notifications', component: NotificationCenterComponent }
    ],
    canActivateChild: [connectedUserGuard]
  },
  { path: '**', component: NotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule],
})
export class AppRoutingModule { }