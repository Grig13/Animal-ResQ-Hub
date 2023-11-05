import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/services/authentication/authentication.service';
import { UserStoreService } from 'src/services/user-store/user-store.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  public users: any = [];
  public fullName : string = "";
  public role! : string;
  constructor(private auth: AuthenticationService, private userStore: UserStoreService){}

  ngOnInit(): void {
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
}
