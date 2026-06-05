import { StockListDetails } from './stock-list-details';

describe('StockListDetails', () => {
  it('should create an instance', () => {
    expect(new StockListDetails()).toBeTruthy();
  });

  it('should hold email and list name', () => {
    const details = new StockListDetails();
    details.userEmail = 'test@test.com';
    details.listName = 'My List';

    expect(details.userEmail).toBe('test@test.com');
    expect(details.listName).toBe('My List');
  });
});
