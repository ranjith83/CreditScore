import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CompanyService } from '../_services/company.service';
import { CompanyDetail } from '../_models/credit-model';

@Component({
  selector: 'app-add-company',
  templateUrl: './add-company.component.html',
  styleUrls: ['./add-company.component.css']
})
export class AddCompanyComponent implements OnInit {
  companyFormGroup: FormGroup
    submitted: boolean;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private companyService: CompanyService,

  ) {
  }

  ngOnInit() {

    this.companyFormGroup = this.formBuilder.group({
      name: ['', Validators.required],
      address: ['', Validators.required],
      telephone: ['', Validators.required],
      balance: ['', [Validators.required]]
    });

  }

  onSubmit() {
    this.submitted = true;
    this.addCompany();

  }


  addCompany() {
    let addCompanyDetail: CompanyDetail;
    let formControls = this.companyFormGroup.controls;
    addCompanyDetail = {
      Name: formControls.name.value,
      Address: formControls.address.value,
      Telephone: formControls.telephone.value,
      Balance: Number(formControls.balance.value),
      Id: 0,
    }


    this.companyService.addCompany(addCompanyDetail)
      //.pipe(first())
      .subscribe(
        data => {
          //return data
          if (data) {
            //this.userDetails.pipe(push(data);
            // this.userDetails = this.userDetails.slice();
            //this.userDetailSubject.next(data);
            //this.changeDectector(data);
          }
        }
        //error => {
        //  this.error = error.message;
        //  this.loading = false;
        //}
      );
  }


}
