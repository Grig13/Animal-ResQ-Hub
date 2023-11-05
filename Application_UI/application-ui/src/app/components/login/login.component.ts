import { group } from '@angular/animations';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import ValidateForm from 'src/app/helpers/validate-form';
import { AuthenticationService } from 'src/services/authentication/authentication.service';
import { ToastrService } from 'ngx-toastr';
import { UserStoreService } from 'src/services/user-store/user-store.service';
import { ResetPasswordService } from 'src/services/reset-password/reset-password.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  public resetPasswordEmail! :string;
  public isValidEmail!: boolean;
  loginForm!: FormGroup;

  constructor(private fb: FormBuilder,
     private authenticationService: AuthenticationService,
     private router: Router,
     private toaster: ToastrService,
     private userStore: UserStoreService,
     private resetService: ResetPasswordService
     ){}

  ngOnInit() :void{
    this.loginForm = this.fb.group({
      username: ['',Validators.required],
      password: ['', Validators.required]
    })
  }

  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon="fa-eye-slash";
    this.isText ? this.type = "text" : this.type = "password";
    }

    onLogin(){
      if(this.loginForm.valid){
        this.authenticationService.login(this.loginForm.value).subscribe({
          next: (res) => {
            this.toaster.success("Welcome!", "You are succesfully logged in!", {
              timeOut: 3000,
              progressBar : true,
              progressAnimation: 'increasing'
            });
            this.loginForm.reset();
            this.authenticationService.storeToken(res.AccessToken);
            this.authenticationService.storeRefreshToken(res.RefreshToken);
            const tokenPayload = this.authenticationService.decodedToken();
            this.userStore.setFullNameForStore(tokenPayload.unique_name);
            this.userStore.setRoleForStore(tokenPayload.role);
            this.router.navigate(['home']);
          },
          error: (err) => {
            this.toaster.error("You could not be logged in!", "Name Or Password Incorrect!", {
              timeOut: 3000,
              progressBar : true,
              progressAnimation: 'increasing'
            });
          }
        });
      }else{
        ValidateForm.validateAllFormFields(this.loginForm);
        alert("Your form is invalid!");
      }
    }

    checkValidEmail(event: string){
      const value = event;
      const pattern= /^[\w-\.]+@([\w-]+\.)+[\w-]{2,3}$/;
      this.isValidEmail = pattern.test(value);
      return this.isValidEmail;
    }

    confirmToSend(){
      if(this.checkValidEmail(this.resetPasswordEmail)){
        console.log(this.resetPasswordEmail);
      
        this.resetService.sendResetPasswordLink(this.resetPasswordEmail)
        .subscribe({
          next: (res) => {
            this.toaster.success("Success!", "Reset Success!", {
              timeOut: 3000,
              progressBar : true,
              progressAnimation: 'increasing'
            });
            this.resetPasswordEmail = "";
            const buttonRef = document.getElementById("closeBtn");
            buttonRef?.click();
          },
          error: (err) => {
            this.toaster.error("Error!", "Somethign went wrong!", {
              timeOut: 5000
            });
          }

        })
      }
    }



}
