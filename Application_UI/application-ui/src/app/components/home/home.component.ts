import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/services/authentication/authentication.service';
import { UserStoreService } from 'src/services/user-store/user-store.service';
import {AnimalsService} from "../../../services/animals/animals.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  public users: any = [];
  animalsForm: FormGroup = new FormGroup({});
  public fullName : string = "";
  public role! : string;
  showForm = false;
  constructor(private fb: FormBuilder, private auth: AuthenticationService, private userStore: UserStoreService, private animalsService: AnimalsService){}

  ngOnInit(): void {

    this.animalsForm = this.fb.group({
      animalType: ['', Validators.required],
      animalBreed: ['', Validators.required],
      animalName: ['', Validators.required],
      animalAge: ['', [Validators.required, Validators.min(0)]],
      animalSize: ['', Validators.required],
      animalHealth: [''],
      animalGender: ['', Validators.required],
      goodWith: ['', Validators.required],
      coatTypes: ['', Validators.required],
      shelter: [''],
      specialTrained: [false]
    })

    this.auth.getUsers()
    .subscribe(res =>{
      this.users = res;
    });

    this.userStore.getFullNameFromStore()
      .subscribe( val => {
      let fullNameFromToken = this.auth.getFullNameFromToken();
      this.fullName = val || fullNameFromToken;
      console.log(this.fullName);
      console.log(fullNameFromToken);
    });

    this.userStore.getRoleFromStore()
      .subscribe( val => {
      let roleFromToken = this.auth.getRoleFromToken();
      this.role = val || roleFromToken;
      });

  }

  logOut(){
    this.auth.signOut();
  }

  addAnimals() {
    if (this.animalsForm.valid) {
      const newAnimal = this.animalsForm.value;

      this.animalsService.addAnimal(newAnimal).subscribe(
        (createdAnimal) => {
          console.log('Animal created successfully:', createdAnimal);
        },
        (error) => {
          console.error('Error creating animal:', error);
        }
      );
    } else {
      console.log('Form is invalid. Cannot submit.');
    }
  }
}
