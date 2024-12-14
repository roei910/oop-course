import { Component } from '@angular/core';
import { Stock } from 'src/models/stocks/stock';
import { StockService } from 'src/services/stock.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent {
  stocksList: Stock[] = [];

  constructor(private stockService: StockService
  ) { }

  searchResults(searchTerm: string) {
    this.stockService.findStocksBySearchTerm(searchTerm)
      .subscribe(stocks => this.stocksList = stocks);
  }
}