import { UserCreation } from './user-creation';

describe('UserCreation', () => {
  it('should create an instance', () => {
    expect(new UserCreation()).toBeTruthy();
  });

  it('should hold registration details', () => {
    const user = new UserCreation();
    user.firstName = 'John';
    user.lastName = 'Doe';
    user.email = 'john@test.com';
    user.password = 'secret';

    expect(user.firstName).toBe('John');
    expect(user.email).toBe('john@test.com');
  });
});
