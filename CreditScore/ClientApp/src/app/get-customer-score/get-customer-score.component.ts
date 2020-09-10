import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../_services/customer.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../_services';

@Component({
  selector: 'app-get-customer-score',
  templateUrl: './get-customer-score.component.html',
  styleUrls: ['./get-customer-score.component.css']
})
export class GetCustomerScoreComponent implements OnInit {
  getCustomerScoreForm: FormGroup;
  userDetails: any;
  creditInquiries: any;
  error: string;

  constructor(
    private customerService: CustomerService,
    private formBuilder: FormBuilder,
    private authenticationService: AuthenticationService
  ) { }

  ngOnInit() {

    this.getCustomerScoreForm = this.formBuilder.group({
      userName: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      batchID: ['', Validators.required],
      idNumber: ['', [Validators.required, Validators.minLength(6)]]
    });
    this.userDetails = this.authenticationService.currentUserValue;
  }

  onSubmit() {
    this.invokeCreditScore();
  }

  // convenience getter for easy access to form fields
  get f() { return this.getCustomerScoreForm.controls; }

  invokeCreditScore() {
    this.customerService.invokeCreditScore(this.userDetails.username, this.f.idNumber.value).subscribe(
      data => {
        this.creditInquiries = data;
        this.creditInquiries.idNumber = this.f.idNumber.value;
        return data;
      },
      (err) => {
        this.error = err.error.message;
        console.log(err);
      });
  }

}
