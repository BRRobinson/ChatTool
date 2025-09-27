import { CommonModule } from '@angular/common';
import { Component, inject, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './chat.html',
  styleUrl: './chat.scss'
})
export class Chat {
  @Input() selectedChat: string | null = null;

  private authService = inject(AuthService);

  messages: { from: string; text: string }[] = [];

  activeUser: string | null = null;

  newMessage = '';
  
  constructor() {
    this.authService.activeUser$.subscribe(user => this.activeUser = user);
  }
  sendMessage() {
    if (!this.newMessage.trim()) return;
    this.messages.push({ from: 'Me', text: this.newMessage });
    this.newMessage = '';
  }
}
