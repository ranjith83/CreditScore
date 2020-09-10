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
  companyDetails: CompanyDetail[];
  isEdited: boolean;
  hideColumn:boolean = true;
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
      balance: ['', [Validators.required]],
        id: ['', [Validators.required]]
    });

    this.getAllCompany();
  }

  onSubmit() {
    this.submitted = true;
    this.addCompany();

  }

  getAllCompany() {

    this.companyService.getAllCompany()
      .subscribe(
        data => {
          if (data) {
            this.companyDetails = data;
          }
        }
      );
  }


  addCompany() {
    let companyDetail: CompanyDetail;
    let formControls = this.companyFormGroup.controls;
    companyDetail = {
      Name: formControls.name.value,
      Address: formControls.address.value,
      Telephone: formControls.telephone.value,
      Balance: Number(formControls.balance.value),
      Id: formControls.id.value,
    }


    this.companyService.addUpdateCompany(companyDetail)
      //.pipe(first())
      .subscribe(
        data => {
          //return data
          if (data) {
            this.companyDetails.push(data);
            //this.userDetails.pipe(push(data);
            // this.userDetails = this.userDetails.slice();
            //this.userDetailSubject.next(data);
            //this.changeDectector(data);
            this.getAllCompany();
          }
        }
        //error => {
        //  this.error = error.message;
        //  this.loading = false;
        //}
      );
  }

  onSelectedRow(company: any) {
    if (company != null) {
      this.isEdited = true;
      let formControls = this.companyFormGroup;
      formControls.setValue({
        name: company.name,
        address: company.address,
        telephone: company.telephone,
        balance: company.balance,
        id: company.id
      })
    }
  }


}
