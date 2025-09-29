import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { API_URL } from '../../app.config';
import { Observable } from 'rxjs';
import { ReturnResult } from '../../models/return-result.model';
import { MessageModel } from '../../models/message.model';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  constructor(private http: HttpClient, @Inject(API_URL) private apiUrl: string) {
  }
  
  getMessagesByChat(chatId: number): Observable<ReturnResult<MessageModel[]>> {
    return this.http.get<ReturnResult<MessageModel[]>>(`${this.apiUrl}/message/chat/${chatId}`);
  }

  getMessageById(id: number): Observable<ReturnResult<MessageModel>> {
    return this.http.get<ReturnResult<MessageModel>>(`${this.apiUrl}/message/${id}`);
  }

  postMessage(message: MessageModel): Observable<ReturnResult<MessageModel>> {
    return this.http.post<ReturnResult<MessageModel>>(`${this.apiUrl}/message`, message);
  }

  updateMessage(message: MessageModel): Observable<ReturnResult<MessageModel>> {
    return this.http.patch<ReturnResult<MessageModel>>(`${this.apiUrl}/message`, message);
  }

  deleteMessage(id: number): Observable<ReturnResult<void>> {
    return this.http.delete<ReturnResult<void>>(`${this.apiUrl}/message/${id}`);
  }
}
