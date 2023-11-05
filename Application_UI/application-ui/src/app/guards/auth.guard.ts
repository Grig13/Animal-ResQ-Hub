import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { AuthenticationService } from 'src/services/authentication/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {
  constructor(private auth: AuthenticationService, private router: Router, private toast: ToastrService ) {}

 canActivate(): Observable<boolean>  {
    if(this.auth.isLoggedIn()){
      return of(true);
    }else{
      this.toast.error("You could not be logged in!", "Please Sign In first!", {
        timeOut: 3000,
        progressBar : true,
        progressAnimation: 'decreasing'
      });
      this.router.navigate(['login']);
      return of(false);
    }
  }
}
