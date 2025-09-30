import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { API_URL } from '../../app.config';
import { ReturnResult } from '../../models/return-result.model';
import { MessageModel } from '../../models/message.model';
import * as signalR from '@microsoft/signalr';
import { Subject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private hubConnection!: signalR.HubConnection;
  private messageReceivedSubject = new Subject<MessageModel>();
  messageReceived$ = this.messageReceivedSubject.asObservable();
  private messageDeleteSubject = new Subject<number>();
  messageDeleted$ = this.messageDeleteSubject.asObservable();

  constructor(private http: HttpClient, @Inject(API_URL) private apiUrl: string) {
  }
  
  startHubConnection(chatId: number) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.apiUrl}/messagehub?chatid=${chatId}`, {
        withCredentials: true
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(err => console.error(err));
    
    this.hubConnection.on('ReceiveMessage', (message: MessageModel) => {
      this.messageReceivedSubject.next(message);
    });

    this.hubConnection.on('DeleteMessage', (messageid: number) => {
      this.messageDeleteSubject.next(messageid);
    });
  }

  sendMessage(chatId: string, message: MessageModel) {
    this.hubConnection.invoke('SendMessage', chatId, message)
      .catch(err => console.error(err));
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
