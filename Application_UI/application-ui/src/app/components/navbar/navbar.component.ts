import { Component } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  isLoggedIn: boolean = false; // Set to true if the user is logged in, otherwise false
  showDropdown: boolean = false;

  login() {
    // Implement your login logic here
    this.isLoggedIn = true;
  }

  logout() {
    // Implement your logout logic here
    this.isLoggedIn = false;
  }

  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
  }
}
