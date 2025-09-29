import { CommonModule } from '@angular/common';
import { Component, HostListener, inject, Input, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth/auth.service';
import { ChatModel } from '../../models/chat.model';
import { MessageModel } from '../../models/message.model';
import { MessageService } from '../../services/message/message.service';
import { UserModel } from '../../models/user.model';
import { ToastService } from '../../services/toast/toast.service';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './chat.html',
  styleUrl: './chat.scss'
})
export class Chat {
  private toastService = inject(ToastService);
  private messageService = inject(MessageService);

  @Input() selectedChat: ChatModel | null = null;

  private authService = inject(AuthService);

  messages: MessageModel[] = [];

  activeUser: UserModel | null = null;

  selectedMessage: MessageModel | null = null;

  newMessage = '';
  editContent = '';

  constructor() {
    this.authService.activeUser$.subscribe(user => this.activeUser = user);
  }
  
  
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['selectedChat'] && this.selectedChat?.id) {
      this.loadMessages();
    }
  }

  loadMessages() {
    this.messageService.getMessagesByChat(this.selectedChat?.id!).subscribe(result => {
      if (result.isSuccess) {
        this.messages = result.value!;
      }
    });
  }

  selectMessage(message: MessageModel){
    this.editContent = message.content;
    this.selectedMessage = message;
  }

  sendMessage() {
    if (!this.newMessage.trim()) return;
    
    let message = new MessageModel(this.selectedChat!, this.activeUser!, this.newMessage);

    this.messageService.postMessage(message).subscribe({
      next: res => {
        if (res.isSuccess){
          this.messages.push(res.value!);
        }
      },
      error: err => this.toastService.error(err.message)
    });
    this.newMessage = '';
  }

  saveMessage(message: MessageModel){
    if (!this.editContent.trim()) return;
    
    message.content = this.editContent;

    this.messageService.updateMessage(message).subscribe({
      next: res => {
        if (res.isSuccess){
          const index = this.messages.findIndex(m => m.id === res.value!.id);
          if (index > -1) {
            // Replace the existing message with the updated one
            this.messages[index] = res.value!;
          }
        }
        else 
          this.toastService.error(res.message)
      },
      error: err => this.toastService.error(err.message)
    });
    this.selectedMessage = null;
  }

  deleteMessage(message: MessageModel){
    if (!this.editContent.trim()) return;
    
    this.messageService.deleteMessage(message.id).subscribe({
      next: res => {
        if (res.isSuccess){
          this.messages = this.messages
            .filter(item => item !== message);
        }
        else 
          this.toastService.error(res.message)
      },
      error: err => this.toastService.error(err.message)
    });
    this.selectedMessage = null;
  }

  @HostListener('document:click', ['$event'])
  clickOutside(event: Event) {
    const target = event.target as HTMLElement;
    // If the clicked element is not inside a bubble, close edit
    if (!target.closest('.bubble')) {
      this.selectedMessage = null;
    }
  }
}
