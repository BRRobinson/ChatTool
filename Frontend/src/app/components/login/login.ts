import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastService } from '../../services/toast/toast.service';
import { LoginRequestModel } from '../../models/login-request.model';
import { ReturnResult } from '../../models/return-result.model';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

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
  
  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
    });
  }

  login(){
    if (!this.loginForm.valid)
      return;

    this.authService.login(
      new LoginRequestModel(
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
      new LoginRequestModel(
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
