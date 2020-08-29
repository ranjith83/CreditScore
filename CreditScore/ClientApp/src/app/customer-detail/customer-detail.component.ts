import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../_services/customer.service';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { AuthenticationService } from '../_services';
import * as XLSX from 'xlsx';

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

  /*name of the excel-file which will be downloaded. */
  fileName = 'ExcelSheet.xlsx';

  exportexcel(): void {
    /* table id is passed over here */
    let element = document.getElementById('credit-table');
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);

    /* generate workbook and add the worksheet */
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

    /* save to file */
    XLSX.writeFile(wb, this.fileName);

  }

}
