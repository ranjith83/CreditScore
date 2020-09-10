import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../_services/customer.service';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { AuthenticationService } from '../_services';

@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.css']
})
export class CustomerDetailComponent implements OnInit {
  customerForm: FormGroup;
  creditInquiries: any;
  userDetails: any;

  constructor(
    private customerService: CustomerService,
    private formBuilder: FormBuilder,
    private authenticationService: AuthenticationService,
  ) { }


  ngOnInit() {
    this.customerForm = this.formBuilder.group({
      userName: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      batchID: ['', Validators.required],
      idNumber: ['', [Validators.required, Validators.minLength(6)]]
    });
    this.userDetails = this.authenticationService.currentUserValue;
    this.getCredits();
  }

  // convenience getter for easy access to form fields
  get f() { return this.customerForm.controls; }


  onSubmit() {
    this.getScore();
  }

  getScore() {
    this.customerService.invokeCreditScore(this.f.userName.value, this.f.idNumber.value).subscribe(
      data => {
        this.creditInquiries = data;
        this.creditInquiries.idNumber = this.f.idNumber.value;
        return data;
      },
      (error) => {
        console.log(error);
      });
  }

  getCredits() {
    var userVal = this.authenticationService.currentUserValue;
    if (userVal != null && userVal.id != null)
      this.customerService.getUserCredits(userVal.id).subscribe(
      data => {
        this.creditInquiries = data;
        return data;
      });
  }

  
}
