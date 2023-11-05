import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import ValidateForm from 'src/app/helpers/validate-form';
import { AuthenticationService } from 'src/services/authentication/authentication.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {

  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  signUpForm!: FormGroup;
  constructor( private fb: FormBuilder, private authenticationService: AuthenticationService){
  
  }

  ngOnInit() :void{
    this.signUpForm = this.fb.group({
      firstName: ['',Validators.required],
      lastName: ['', Validators.required],
      userName: ['',Validators.required],
      password: ['', Validators.required],
      email: ['', Validators.required]
    });
  }

  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon="fa-eye-slash";
    this.isText ? this.type = "text" : this.type = "password";
    }

    onSignUp(){
      if(this.signUpForm.valid){
        this.authenticationService.signUp(this.signUpForm.value).subscribe({
          next: (res) => {
            alert(res.Message)
          },
          error: (err) => {
            alert("User could not be added!");
          }
        });
      }else{
        ValidateForm.validateAllFormFields(this.signUpForm);
        alert("Your form is invalid!");
      }
    }

  
}
