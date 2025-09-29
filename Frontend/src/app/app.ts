import { Component, inject, signal } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { filter } from 'rxjs';
import { TopBar } from './components/top-bar/top-bar';
import { AuthService } from './services/auth/auth.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TopBar, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  private router = inject(Router);
  private authService = inject(AuthService);

  protected readonly title = signal('ChatTool');
  
  showToolbar = true;

  constructor() {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        // hide toolbar on login
        this.showToolbar = event.url !== '/login';
      });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
