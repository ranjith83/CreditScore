import { Component } from '@angular/core';
import { AuthenticationService } from '../_services';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  isAdmin: boolean = true;
  isLogged: boolean;

  constructor(private authenticationService: AuthenticationService,
               private router: Router) {

  }

  ngOnInit() {
    this.isAdmin = this.authenticationService.isAdmin;
    this.isLogged = this.authenticationService.isLoggedIn;
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    // remove user from local storage to log user out
    this.authenticationService.logout();

    // redirect to home if already logged in
    if (this.authenticationService.currentUserValue) {
      this.router.navigate(['/login']);
    }
  }
}
