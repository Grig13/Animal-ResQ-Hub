import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, switchMap, throwError } from 'rxjs';
import { AuthenticationService } from 'src/services/authentication/authentication.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { TokenApiModel } from '../models/token-api.model';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private auth: AuthenticationService, private toast: ToastrService, private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const myToken = this.auth.getToken();
    if(myToken){
      request = request.clone({
        setHeaders: {Authorization: `Bearer ${myToken}`} 
    });
    }

    return next.handle(request).pipe(
      catchError((err: any)=>{
        if(err instanceof HttpErrorResponse){
          if(err.status === 401){
            return this.handleUnauthorizedError(request,next);
          }
        }
        return throwError(() => new Error("Some other error occured"));
      })
    );
  }

  handleUnauthorizedError(req: HttpRequest<any>, next: HttpHandler) {
    let AccessToken = this.auth.getToken();
    let RefreshToken = this.auth.getRefreshToken();

    if (!AccessToken || !RefreshToken) {
        this.toast.warning("Missing token information. Please login again.", "Warning");
        this.router.navigate(['login']);
        return throwError('Token information missing');
    }

    let tokenApiModel = new TokenApiModel();
    tokenApiModel.AccessToken = AccessToken;
    tokenApiModel.RefreshToken = RefreshToken;

    return this.auth.renewToken(tokenApiModel)
    .pipe(
        switchMap((data: TokenApiModel) => {
            this.auth.storeToken(data.AccessToken);
            this.auth.storeRefreshToken(data.RefreshToken);

            req = req.clone({
                setHeaders: { Authorization: `Bearer ${data.AccessToken}` }
            });

            return next.handle(req);
        }),
        catchError((err) => {
            this.toast.warning("Token renewal failed. Please login again.", "Warning");
            this.router.navigate(['login']);
            return throwError(err);
        })
    );
}

}
