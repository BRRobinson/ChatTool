import { Routes } from '@angular/router';
import { Login } from './components/login/login';
import { Home } from './components/home/home';
import { authGuard, unAuthGuard } from './guards/auth.guards';

export const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: Home , canActivate: [unAuthGuard] },
    { path: 'login', component: Login, canActivate: [authGuard]}
];
