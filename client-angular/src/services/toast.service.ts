import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { MessageFactory } from 'src/factories/message-factory';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  constructor(
    private messageService: MessageService,
    private messageFactory: MessageFactory
  ) { }

  addSuccessMessage(body: string): void{
    let message = this.messageFactory.createSuccessMessage(body);
    this.messageService.add(message);
  }

  addErrorMessage(body: string): void{
    let message = this.messageFactory.createErrorMessage(body);
    this.messageService.add(message);
  }

  addInfoMessage(body: string): void{
    let message = this.messageFactory.createInfoMessage(body);
    this.messageService.add(message);
  }
}