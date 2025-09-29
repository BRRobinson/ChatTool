import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { API_URL } from '../../app.config';
import { ChatModel } from '../../models/chat.model';
import { ReturnResult } from '../../models/return-result.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  constructor(private http: HttpClient, @Inject(API_URL) private apiUrl: string) {
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
