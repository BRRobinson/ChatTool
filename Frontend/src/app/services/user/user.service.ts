import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_URL } from '../../app.config';
import { UserModel } from '../../models/user.model';
import { ReturnResult } from '../../models/return-result.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient, @Inject(API_URL) private apiUrl: string) {
  }

  getUsers(): Observable<ReturnResult<UserModel[]>> {
    return this.http.get<ReturnResult<UserModel[]>>(`${this.apiUrl}/users`); 
  }
  
  getUser(username: string): Observable<ReturnResult<UserModel>> {
    return this.http.get<ReturnResult<UserModel>>(`${this.apiUrl}/users/${username}`); 
  }
}
