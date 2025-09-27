import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LoginRequest } from '../models/login-request.model';
import { ReturnResult } from '../models/return-result.model';
import { API_URL } from '../app.config';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'auth_token';
  private readonly USERNAME_KEY = 'auth_username';

  private _activeUser = new BehaviorSubject<string | null>(
    localStorage.getItem(this.USERNAME_KEY)
  );
  activeUser$ = this._activeUser.asObservable();
  
  constructor(private http: HttpClient, @Inject(API_URL) private apiUrl: string) {
  }

  setActiveUser(username: string) {
    localStorage.setItem(this.USERNAME_KEY, username);
    this._activeUser.next(username);
  }
  
  getActiveUser(username: string) {
    return localStorage.getItem(this.USERNAME_KEY);
  }
  
  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) return false;

    try {
      const decoded: any = jwtDecode(token);
      const expiry = decoded.exp * 1000;
      return Date.now() < expiry;
    } catch {
      return false;
    }
  }

  login(loginRequest: LoginRequest): Observable<ReturnResult<string>> {
    return this.http.post<ReturnResult<string>>(`${this.apiUrl}/auth/login`, loginRequest)
    .pipe(tap(response => {
        if (response.isSuccess) {
          localStorage.setItem(this.TOKEN_KEY, response.value!);
          this.setActiveUser(this.getUsername(response.value!)!);
        }
    })); 
  }

  register(loginRequest: LoginRequest): Observable<ReturnResult<string>> {
    return this.http.post<ReturnResult<string>>(`${this.apiUrl}/auth/register`, loginRequest)
    .pipe(tap(response => {
        if (response.isSuccess) {
          localStorage.setItem(this.TOKEN_KEY, response.value!);
          this.setActiveUser(this.getUsername(response.value!)!);
        }
    })); 
  }

  private getUsername(token: string): string | null {
    if (!token) return null;

    // JWT format: header.payload.signature
    const payload = token.split('.')[1]; // get the payload
    const decoded = atob(payload); // decode Base64
    return JSON.parse(decoded).username;
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USERNAME_KEY);
  }
}
