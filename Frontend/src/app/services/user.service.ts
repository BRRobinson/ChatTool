import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ReturnResult } from '../models/return-result.model';
import { API_URL } from '../app.config';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient, @Inject(API_URL) private apiUrl: string) {
  }

  getUsers(): Observable<ReturnResult<User[]>> {
    return this.http.get<ReturnResult<User[]>>(`${this.apiUrl}/users`); 
  }
}
