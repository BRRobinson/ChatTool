import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-chat-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './chat-list.html',
  styleUrl: './chat-list.scss'
})
export class ChatList {
  private authService = inject(AuthService);
  private userService = inject(UserService);

  @Output() chatSelected = new EventEmitter<string>();

  activeUser: string | null = null;

  chats: User[] = [];

  constructor() {
    this.authService.activeUser$.subscribe(user => this.activeUser = user);
  }

  ngOnInit() {
    this.userService.getUsers().subscribe(result => {
      if (result.isSuccess) {
        this.chats = result.value!.filter(u => u.username !== this.activeUser);
      }
    });
  }

  selectUser(user: User) {
    this.chatSelected.emit(user.username);
  }
}
