import { Injectable } from "@angular/core";
import { Message } from "primeng/api/message";

@Injectable({
    providedIn: 'root'
})
export class MessageFactory {
    public createErrorMessage(body?: string): Message {
        let message = {
            severity: 'error',
            summary: 'Error',
            detail: body
        };

        return message;
    }

    public createSuccessMessage(body?: string): Message {
        let message = {
            severity: 'success',
            summary: 'Success',
            detail: body
        };

        return message;
    }

    public createInfoMessage(body?: string): Message {
        let message = {
            severity: 'info',
            summary: 'Info',
            detail: body
        };

        return message;
    }
}