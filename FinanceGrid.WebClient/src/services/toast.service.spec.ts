import { TestBed } from '@angular/core/testing';
import { ToastService } from './toast.service';
import { MessageService } from 'primeng/api';
import { MessageFactory } from 'src/factories/message-factory';
import { Message } from 'primeng/api/message';

describe('ToastService', () => {
  let service: ToastService;
  let messageServiceSpy: jasmine.SpyObj<MessageService>;
  let messageFactorySpy: jasmine.SpyObj<MessageFactory>;

  beforeEach(() => {
    messageServiceSpy = jasmine.createSpyObj('MessageService', ['add']);
    messageFactorySpy = jasmine.createSpyObj('MessageFactory', [
      'createSuccessMessage', 'createErrorMessage', 'createInfoMessage'
    ]);

    TestBed.configureTestingModule({
      providers: [
        ToastService,
        { provide: MessageService, useValue: messageServiceSpy },
        { provide: MessageFactory, useValue: messageFactorySpy }
      ]
    });
    service = TestBed.inject(ToastService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('addSuccessMessage', () => {
    it('should create success message and add it', () => {
      const mockMessage: Message = { severity: 'success', summary: 'Success', detail: 'Done!' };
      messageFactorySpy.createSuccessMessage.and.returnValue(mockMessage);

      service.addSuccessMessage('Done!');

      expect(messageFactorySpy.createSuccessMessage).toHaveBeenCalledWith('Done!');
      expect(messageServiceSpy.add).toHaveBeenCalledWith(mockMessage);
    });
  });

  describe('addErrorMessage', () => {
    it('should create error message and add it', () => {
      const mockMessage: Message = { severity: 'error', summary: 'Error', detail: 'Failed!' };
      messageFactorySpy.createErrorMessage.and.returnValue(mockMessage);

      service.addErrorMessage('Failed!');

      expect(messageFactorySpy.createErrorMessage).toHaveBeenCalledWith('Failed!');
      expect(messageServiceSpy.add).toHaveBeenCalledWith(mockMessage);
    });
  });

  describe('addInfoMessage', () => {
    it('should create info message and add it', () => {
      const mockMessage: Message = { severity: 'info', summary: 'Info', detail: 'FYI' };
      messageFactorySpy.createInfoMessage.and.returnValue(mockMessage);

      service.addInfoMessage('FYI');

      expect(messageFactorySpy.createInfoMessage).toHaveBeenCalledWith('FYI');
      expect(messageServiceSpy.add).toHaveBeenCalledWith(mockMessage);
    });
  });
});
