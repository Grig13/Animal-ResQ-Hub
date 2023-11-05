import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Route, Router } from '@angular/router';
import { Observable, catchError, throwError } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TokenApiModel } from 'src/app/models/token-api.model';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private apiUrl = 'https://localhost:7125'; 
  private userPayLoad: any;

  constructor(private http: HttpClient, private router: Router) {
    this.userPayLoad = this.decodedToken();
  }

  signUp(userObj: any) {
    return this.http.post<any>(`${this.apiUrl}/register`,userObj);
  }

  login(userObj: any) {
    return this.http.post<any>(`${this.apiUrl}/authenticate`,userObj);
  }

  getUsers() {
    return this.http.get<any>(`${this.apiUrl}/all-users`);
  }

  storeToken(tokenValue: string){
    localStorage.setItem('token', tokenValue);
  }

  storeRefreshToken(tokenValue: string){
    localStorage.setItem('refreshToken', tokenValue);
  }

  getToken(){
    return localStorage.getItem('token')
  }

  getRefreshToken(){
    return localStorage.getItem('refreshToken')
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  signOut(){
    localStorage.clear();
    this.router.navigate(['login']);
  }

  decodedToken(){
    const jwtHelper = new JwtHelperService();
    const token = this.getToken()!;
    return jwtHelper.decodeToken(token);
  }

  getFullNameFromToken(){
    if(this.userPayLoad)
      return this.userPayLoad.unique_name;
  }

  getRoleFromToken(){
    if(this.userPayLoad)
      return this.userPayLoad.role;
  }

  renewToken(tokenApi: TokenApiModel){
    return this.http.post<any>(`${this.apiUrl}/refresh`, tokenApi)
    .pipe(
      catchError((err: HttpErrorResponse) => {
          console.error("Error details:", err);
          return throwError(err);
      })
  );
  }
}
