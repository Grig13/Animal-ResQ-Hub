import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ConfirmPasswordValidator } from 'src/app/helpers/confirm-password.validator';
import ValidateForm from 'src/app/helpers/validate-form';
import { ResetPassword } from 'src/app/models/reset-password.model';
import { ResetPasswordService } from 'src/services/reset-password/reset-password.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent {
  resetPasswordForm!: FormGroup;
  emailToReset!: string;
  emailToken!: string;
  resetPasswordObj = new ResetPassword();

  constructor(private fb: FormBuilder, private activatedRoute: ActivatedRoute, private resetPasswordService: ResetPasswordService, private toaster: ToastrService, private router: Router) { }

ngOnInit() : void{
  this.resetPasswordForm = this.fb.group({
    password: [null,Validators.required],
    confirmPassword:[null, Validators.required]
  },{
    validator: ConfirmPasswordValidator("password","confirmPassword")
  });

  this.activatedRoute.queryParams.subscribe (val => {
    this.emailToReset = val['email'];
    let uriToken = val['code'];
    this.emailToken = uriToken.replace(/ /g,'+');
    console.log(this.emailToReset);
    console.log(this.emailToken);
  })
}

reset(){ 
  if(this.resetPasswordForm.valid){
    this.resetPasswordObj.Email = this.emailToReset;
    this.resetPasswordObj.NewPassword = this.resetPasswordForm.value.password;
    this.resetPasswordObj.ConfirmPassword = this.resetPasswordForm.value.confirmPassword;
    this.resetPasswordObj.EmailToken = this.emailToken;
    this.resetPasswordService.resetPassword(this.resetPasswordObj)
    .subscribe({
      next: (res) => {
        this.toaster.success("SUCCESS!", "Password Reset Successfully!", {
          timeOut: 3000,
          progressBar : true,
          progressAnimation: 'increasing'
        });
        this.router.navigate(['/']);
      },
      error: (err) =>{
        this.toaster.error("ERROR!", "Something went wrong!", {
          timeOut: 3000,
          progressBar : true,
          progressAnimation: 'increasing'
        });
      }
    })
  }else{
    ValidateForm.validateAllFormFields(this.resetPasswordForm);
  }
}

}
