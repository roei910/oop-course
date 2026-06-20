import { ObjectIdResponse } from './object-id-response';

describe('ObjectIdResponse', () => {
  it('should create an instance', () => {
    expect(new ObjectIdResponse()).toBeTruthy();
  });

  it('should hold the Id', () => {
    const response = new ObjectIdResponse();
    response.Id = 'abc123';

    expect(response.Id).toBe('abc123');
  });
});
