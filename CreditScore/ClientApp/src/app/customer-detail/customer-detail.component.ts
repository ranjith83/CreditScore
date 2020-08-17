import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../_services/customer.service';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.css']
})
export class CustomerDetailComponent implements OnInit {
  customerForm: FormGroup;

  constructor(
    private customerService: CustomerService,
    private formBuilder: FormBuilder
  ) { }


  ngOnInit() {
    this.customerForm = this.formBuilder.group({
      userName: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.customerForm.controls; }


  onSubmit() {

    this.getScore();

  }

  getScore() {
    this.customerService.invokeScore(this.f.userName.value, this.f.password.value).subscribe(
      data => {
        return data;
      });
  }

  

}
