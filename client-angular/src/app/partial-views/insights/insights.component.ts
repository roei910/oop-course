import { Component } from '@angular/core';
import { MarketTrend } from 'src/models/marketTrends/market-trend';
import { StockService } from 'src/services/stock.service';

@Component({
  selector: 'app-insights',
  templateUrl: './insights.component.html',
  styleUrls: ['./insights.component.css']
})
export class InsightsComponent {
  marketTrends: MarketTrend[] = [];
  selectedMarketTrend?: MarketTrend;

  constructor(private stockService: StockService) {
    this.stockService.getMarketsTrends()
      .subscribe(marketTrends => {
        this.marketTrends = marketTrends;
        this.selectedMarketTrend = marketTrends[0];
      });
  }

  generateMarketTrendName(trendName: string): string {
    let words = trendName.split('_').map(word => {
      let updatedWord = word[0].toUpperCase() + word.toLowerCase().slice(1);

      return updatedWord;
    });

    let updatedTrendName = words.join(" ");

    return updatedTrendName;
  }

  openExternalLink(url: string) {
    window.open(url, '_blank');
  }
}