import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ResetPassword } from 'src/app/models/reset-password.model';

@Injectable({
  providedIn: 'root'
})
export class ResetPasswordService {
  private apiUrl = 'https://localhost:7125'; 

  constructor(private http: HttpClient) {}

  sendResetPasswordLink(email: string) {
    return this.http.post<any>(`${this.apiUrl}/send-reset-email/${email}`, {});
  }

  resetPassword(ressetPasswordObj: ResetPassword) {
    return this.http.post<any>(`${this.apiUrl}/reset-password`, ressetPasswordObj);
  }
}
