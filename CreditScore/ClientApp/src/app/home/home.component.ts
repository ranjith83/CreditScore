import { Component } from '@angular/core';
import { AuthenticationService } from '../_services';
import { CustomerService } from '../_services/customer.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  public userName;
  userScore: string;
  customerDetails : any;

  constructor(
    private authenticationService: AuthenticationService,
    private customerService: CustomerService
  ) {
    // redirect to home if already logged in
    if (this.authenticationService.currentUserValue) {
      this.userName = this.authenticationService.currentUserValue.username
    }
  }
  //public loginuserName

  ngOnInit() {

    this.getUserScore();
    //this.getAllCustomers();
  }

  getUserScore() {
    var userID = this.authenticationService.currentUserValue.id;
    if (userID != null) {
      this.customerService.getUserScore(userID).subscribe(
        data => {
          this.userScore = data;
        });
    }
  }

  getAllCustomers() {
    this.customerService.getAllCustomer().subscribe(
        data => {
        this.customerDetails = data;
        });
    }
}



