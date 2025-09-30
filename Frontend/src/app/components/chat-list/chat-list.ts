import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, input, Input, Output } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { ChatService } from '../../services/chat/chat.service';
import { MessageService } from '../../services/message/message.service';
import { ChatModel } from '../../models/chat.model';
import { ToastService } from '../../services/toast/toast.service';
import { UserModel } from '../../models/user.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-chat-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './chat-list.html',
  styleUrl: './chat-list.scss'
})
export class ChatList {
  private authService = inject(AuthService);
  private toastService = inject(ToastService);
  private chatService = inject(ChatService);
  private messageService = inject(MessageService);
  private chatRecievedSubscription!: Subscription;
  private chatDeletesubscription!: Subscription;

  @Output() selectChat = new EventEmitter<ChatModel | null>();
  @Output() insertChat = new EventEmitter<ChatModel>();
  @Output() editChat = new EventEmitter<ChatModel>();

  activeUser: UserModel | null = null;

  chats: ChatModel[] = [];

  selectedChat: ChatModel | null;

  constructor() {
    this.selectedChat = null;
    this.authService.activeUser$.subscribe(user => this.activeUser = user);
  }

  ngOnInit() {
    this.refreshChats();
    
    this.chatService.startHubConnection(this.activeUser!.id);

    this.chatRecievedSubscription = this.chatService.chatReceived$.subscribe(
      (chat: ChatModel) => {
        this.doAddChat(chat);
      }
    );
    this.chatDeletesubscription = this.chatService.chatDeleted$.subscribe(
      (chatId: number) => {
        this.doDeleteChat(chatId);
      }
    );
  }

  refreshChats() {
    this.chatService.getChats().subscribe({
      next: result => {
        if (result.isSuccess) {
          this.chats = result.value!;
        }
      },
      error: err => this.toastService.error(err.message)
    });
  }

  onSelectChat(chat: ChatModel | null) {
    this.selectedChat = chat;
    this.selectChat.emit(chat);
  }

  onInsertChat(): void {
    this.insertChat.emit();
  }

  onEditChat(chat: ChatModel | null): void {
    if (chat){
      this.editChat.emit(chat);
    }
  }

  doAddChat(chat: ChatModel){
    const index = this.chats.findIndex(m => m.id === chat.id);
    if (index !== -1) {
      // chat already exists → update it
      this.chats[index] = chat;
    } else {
      // new chat → push
      this.chats.push(chat);
    }
  }

  onDeleteChat(chat: ChatModel | null): void {
    if (chat){
      if (!confirm('Are you sure you want to delete this chat?')) return;

      this.chatService.deleteChat(chat!.id).subscribe({
        next: res => {
          if (res.isSuccess) {
            this.doDeleteChat(chat.id);
            this.selectChat.emit(null);
          } else {
            this.toastService.error(res.message);
          }
        },
        error: err => this.toastService.error(err.message)
      });
    }
  }

  doDeleteChat(id: number){
    this.chats = this.chats
      .filter(c => c.id !== id);
  }
}
