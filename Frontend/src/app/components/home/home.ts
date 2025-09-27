import { CommonModule } from '@angular/common';
import { Component, inject, Inject, Input } from '@angular/core';
import { ChatList } from '../chat-list/chat-list';
import { Chat } from '../chat/chat';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, ChatList, Chat],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home {
  
  selectedChat: string | null = null;
  
  onChatSelected(chat: string) {
    this.selectedChat = chat;
  }
}
