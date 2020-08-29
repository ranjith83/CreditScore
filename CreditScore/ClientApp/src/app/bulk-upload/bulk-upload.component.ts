import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { CustomerService } from '../_services/customer.service'
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-bulk-upload',
  templateUrl: './bulk-upload.component.html',
  styleUrls: ['./bulk-upload.component.css']
})
export class BulkUploadComponent implements OnInit {
  customerForm: FormGroup;
  loading = false;
  submitted = false;
  message: string;

  public progress: number;
  public uploadMessage: string;
  @Output() public onUploadFinished = new EventEmitter();

  constructor(private http: HttpClient,
    private customerService: CustomerService,
    private formBuilder: FormBuilder,
    private router: Router,
  ) { }

  ngOnInit() {

    this.customerForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      surName: ['', Validators.required],
      cell: ['', Validators.required],
      address: ['', Validators.required],
      idNumber: ['', [Validators.required]]
    });
  }

  get f() { return this.customerForm.controls; }

  onSubmit() {
    this.submitted = true;
    if (this.customerForm.invalid) {
      return;
    }

    this.addCustomer();
  }

  addCustomer() {
    let addCustomerDetail: any;

    addCustomerDetail = {
      FirstName: this.f.firstName.value,
      SurName: this.f.surName.value,
      Cell: this.f.cell.value,
      Address: this.f.address.value,
      IdNumber: this.f.idNumber.value
    }

    this.customerService.addCustomer(addCustomerDetail)
      .subscribe(
        data => {
          if (data) {
            this.message = 'Added customer successfully';
          }
        },
        err => this.message = (err),
        () => console.log('Added user')
        );
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }

    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    this.customerService.uploadFile(formData)
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress)
          this.progress = Math.round(100 * event.loaded / event.total);
        else if (event.type === HttpEventType.Response) {
          this.uploadMessage = 'Upload success.';
          this.onUploadFinished.emit(event.body);
        }
      });
  }
}
