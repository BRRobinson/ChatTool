// src/app/core/guards/auth.guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { ToastService } from '../services/toast/toast.service';

export const unAuthGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const toastService = inject(ToastService)
  const router = inject(Router);

  if (authService.isAuthenticated()) {
    return true;
  }
  toastService.warning("User is not authenticated, routing to login.");

  return router.createUrlTree(['/login']);
};

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  
  return authService.isAuthenticated() 
    ? router.createUrlTree(['/home'])
    : true;
};
