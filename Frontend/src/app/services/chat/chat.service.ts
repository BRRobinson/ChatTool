import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { API_URL } from '../../app.config';
import { ChatModel } from '../../models/chat.model';
import { ReturnResult } from '../../models/return-result.model';
import { Subject, Observable } from 'rxjs';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private hubConnection!: signalR.HubConnection;
  private chatReceivedSubject = new Subject<ChatModel>();
  chatReceived$ = this.chatReceivedSubject.asObservable();
  private chatDeleteSubject = new Subject<number>();
  chatDeleted$ = this.chatDeleteSubject.asObservable();

  constructor(private http: HttpClient, @Inject(API_URL) private apiUrl: string) {
  }
  
  startHubConnection(userId: number) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.apiUrl}/chathub?userid=${userId}`, {
        withCredentials: true
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(err => console.error(err));
    
    this.hubConnection.on('ReceiveChat', (chat: ChatModel) => {
      this.chatReceivedSubject.next(chat);
    });

    this.hubConnection.on('DeleteChat', (chatid: number) => {
      this.chatDeleteSubject.next(chatid);
    });
  }

  sendMessage(chatId: string, chat: ChatModel) {
    this.hubConnection.invoke('SendMessage', chatId, chat)
      .catch(err => console.error(err));
  }
  
  getChats(): Observable<ReturnResult<ChatModel[]>> {
    return this.http.get<ReturnResult<ChatModel[]>>(`${this.apiUrl}/chats`); 
  }

  getChatById(id: number): Observable<ReturnResult<ChatModel>> {
    return this.http.get<ReturnResult<ChatModel>>(`${this.apiUrl}/chats/${id}`);
  }

  insertChat(chat: ChatModel): Observable<ReturnResult<ChatModel>> {
    return this.http.post<ReturnResult<ChatModel>>(`${this.apiUrl}/chats`, chat);
  }

  updateChat(chat: ChatModel): Observable<ReturnResult<ChatModel>> {
    return this.http.patch<ReturnResult<ChatModel>>(`${this.apiUrl}/chats`, chat);
  }

  deleteChat(id: number): Observable<ReturnResult<void>> {
    return this.http.delete<ReturnResult<void>>(`${this.apiUrl}/chats/${id}`);
  }
}
