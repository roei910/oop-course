import { StockAnalysis } from './stock-analysis';

describe('StockAnalysis', () => {
  it('should create an instance', () => {
    expect(new StockAnalysis()).toBeTruthy();
  });

  it('should hold target prices', () => {
    const analysis = new StockAnalysis();
    analysis.targetHighPrice = 200;
    analysis.targetLowPrice = 150;
    analysis.targetMeanPrice = 175;
    analysis.targetMedianPrice = 175;
    analysis.recomendationKey = 'buy';

    expect(analysis.targetHighPrice).toBe(200);
    expect(analysis.targetLowPrice).toBe(150);
    expect(analysis.recomendationKey).toBe('buy');
  });
});
