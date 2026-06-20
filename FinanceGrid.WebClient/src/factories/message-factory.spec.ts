import { TestBed } from '@angular/core/testing';
import { MessageFactory } from './message-factory';
import { Message } from 'primeng/api/message';

describe('MessageFactory', () => {
  let factory: MessageFactory;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    factory = TestBed.inject(MessageFactory);
  });

  it('should be created', () => {
    expect(factory).toBeTruthy();
  });

  describe('createErrorMessage', () => {
    it('should create an error message with correct severity', () => {
      const result = factory.createErrorMessage('Something went wrong');
      expect(result.severity).toBe('error');
      expect(result.summary).toBe('Error');
      expect(result.detail).toBe('Something went wrong');
    });

    it('should handle undefined body', () => {
      const result = factory.createErrorMessage();
      expect(result.severity).toBe('error');
      expect(result.summary).toBe('Error');
      expect(result.detail).toBeUndefined();
    });
  });

  describe('createSuccessMessage', () => {
    it('should create a success message with correct severity', () => {
      const result = factory.createSuccessMessage('Operation completed');
      expect(result.severity).toBe('success');
      expect(result.summary).toBe('Success');
      expect(result.detail).toBe('Operation completed');
    });
  });

  describe('createInfoMessage', () => {
    it('should create an info message with correct severity', () => {
      const result = factory.createInfoMessage('For your information');
      expect(result.severity).toBe('info');
      expect(result.summary).toBe('Info');
      expect(result.detail).toBe('For your information');
    });
  });
});
