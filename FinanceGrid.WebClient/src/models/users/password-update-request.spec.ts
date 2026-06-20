import { PasswordUpdateRequest } from './password-update-request';

describe('PasswordUpdateRequest', () => {
  it('should create an instance', () => {
    expect(new PasswordUpdateRequest()).toBeTruthy();
  });

  it('should hold email and password', () => {
    const req = new PasswordUpdateRequest();
    req.email = 'test@test.com';
    req.password = 'newpassword';

    expect(req.email).toBe('test@test.com');
    expect(req.password).toBe('newpassword');
  });
});
