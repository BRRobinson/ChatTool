import { CommonModule } from '@angular/common';
import { Component, inject, Inject, Input, ViewChild, viewChild } from '@angular/core';
import { ChatList } from '../chat-list/chat-list';
import { Chat } from '../chat/chat';
import { ChatModel } from '../../models/chat.model';
import { ChatForm } from '../chat-form/chat-form';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, ChatList, Chat, ChatForm],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home {
  @ViewChild(ChatList) chatList!: ChatList;

  selectedChat: ChatModel | null = null;

  chatForm: boolean = false;
  
  onChatSelected(chat: ChatModel | null) {
    this.selectedChat = chat;
    this.chatForm = false;
  }

  onInsertChat() {
    this.selectedChat = null;
    this.chatForm = true;
  }

  onEditChat(chat: ChatModel) {
    this.selectedChat = chat;
    this.chatForm = true;
  }

  onChatSaved(chat: ChatModel){
    this.selectedChat = chat;
    this.chatForm = false;
    this.chatList.refreshChats();
  }
}
