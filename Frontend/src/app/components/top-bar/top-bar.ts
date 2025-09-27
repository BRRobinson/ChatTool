import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-top-bar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './top-bar.html',
  styleUrl: './top-bar.scss'
})
export class TopBar {
  private authService = inject(AuthService);

  @Output() logout = new EventEmitter<void>();

  activeUser: string | null = null;

  constructor() {
    this.authService.activeUser$.subscribe(user => this.activeUser = user);
  }
}
