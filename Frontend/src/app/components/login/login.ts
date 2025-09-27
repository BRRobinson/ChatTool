import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastService } from '../../services/toast.service';
import { LoginRequest } from '../../models/login-request.model';
import { ReturnResult } from '../../models/return-result.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {

  private authService = inject(AuthService);
  private toastService = inject(ToastService);
  private router = inject(Router);

  public loginForm: FormGroup;
  
  constructor() {
    this.loginForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
    });
  }

  login(){
    if (!this.loginForm.valid)
      return;

    this.authService.login(
      new LoginRequest(
        this.loginForm.value.username, 
        this.loginForm.value.password
      )).subscribe({
        next: (result: ReturnResult<string>) => {
          if (!result.isSuccess){
            this.toastService.warning(result.message)
          }
          else{
            this.router.navigate(['/home']);
          }
        },
        error: () => {
          this.toastService.error("Error");
        },
      }
    );
  }

  register(){
    if (!this.loginForm.valid)
      return;
    
    this.authService.register(
      new LoginRequest(
        this.loginForm.value.username, 
        this.loginForm.value.password
      )).subscribe({
        next: (result: ReturnResult<string>) => {
          if (!result.isSuccess){
            this.toastService.warning(result.message)
          }
          else{
            this.router.navigate(['/home']);
          }
        }
      }
    );
  }
}
