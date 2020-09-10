import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { first, map, take } from 'rxjs/operators';
import { AuthenticationService, UserService } from '../_services';
// import { GridOptions } from 'ag-grid-community';

import { Observable, BehaviorSubject, of } from 'rxjs';
import { UserDetail } from '../_models/credit-model';
import { CompanyService } from '../_services/company.service';


@Component({
  selector: 'app-add-users',
  templateUrl: './add-users.component.html',
  styleUrls: ['./add-users.component.scss']
})
export class AddUsersComponent implements OnInit {

  userForm: FormGroup;
  loading = false;
  submitted = false;
 // private gridOptions: GridOptions;
  public rowData:any[];
  private columnDefs: any[];
  public userDetails: UserDetail[];
  usersList: any;
  userID = 0;
  error = '';
  companyID = 0;
  data: object = { };
  private userDetailSubject = new BehaviorSubject<object>(this.data);
  totalTicketCount: BehaviorSubject<number> = new BehaviorSubject<number>(10);
  ticketCount: number = 9;
  public currentCount = 0;
  subject: BehaviorSubject<any[]> = new BehaviorSubject<any>([]);
  array$: Observable<any> = this.subject.asObservable();

  subjectUser: BehaviorSubject<any[]> = new BehaviorSubject<any>([]);
  arrayUser$: Observable<any> = this.subjectUser.asObservable();
  companyDetails: any;
  selectedCompany = 0;
  isEdited: boolean = true;
 //addUser: UserDetail;
  //currentData = this.userDetailSubject.asObservable();
  isPasswordReadonly: boolean = false;

  public incrementCounter() {
    this.currentCount++;
  }

  changeDectector(data: object) {
    this.userDetailSubject.next(data)
  }

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private companyService: CompanyService,

    //private alertService: AlertService
  ) {
     //redirect to home if already logged in
    //if (this.authenticationService.currentUserValue) {
    //  this.router.navigate(['/']);
    //}
    if (this.authenticationService != null && this.authenticationService.currentUserValue != null) {
      this.companyID = this.authenticationService.currentUserValue.companyId;
      this.userID = this.authenticationService.currentUserValue.id;
    }
    //this.userDetailSubject$ = new BehaviorSubject<UserDetail>(this.addUser);
   // this.addUser = this.addUser.asObservable();

    this.totalTicketCount.subscribe(totalTicketCount => {
      this.ticketCount = totalTicketCount
    });
 
  }

  ngOnInit() {
    this.userForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      surName: ['', Validators.required],
      userName: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      company: ['', [Validators.required]],
      id: new FormControl(false) //['', [Validators.required]]
    });

    
    this.loading = true;
    this.loadCompany();
    if (this.companyID != null) {
      this.userService.getUserByCompany(this.companyID)
        .subscribe(
          data => {
            if (data) {
              this.userDetails = data;
              this.usersList = data;
              this.changeDectector(data);
              this.addUserArray(data);
            }
            this.loading = false;
          },
          error => {
            //this.alertService.error(error);
            this.loading = false;
          },
          () => { this.loading = false; }
      );
    }

  }

  // convenience getter for easy access to form fields
  get f() { return this.userForm.controls; }

  onSubmit() {
    this.submitted = true;

    // reset alerts on submit
    //this.alertService.clear();

    // stop here if form is invalid
    if (this.userForm.invalid) {
      return;
    }

    this.addUsers();
  }


  addUserArray(item) {
    this.arrayUser$.pipe(take(1)).subscribe(val => {
      this.subjectUser.next(item);
    })
  }

addElementToObservableArray(item) {
  this.array$.pipe(take(1)).subscribe(val => {
    console.log(val)
    const newArr = [...val, item];
    this.subject.next(newArr);
  })
}

  loadCompany() {

    this.companyService.getCompanyForSelect()
      .subscribe(
        data => {
          this.companyDetails = data;
          this.loading = false;
        });
  }

  addUsers() {
    let addUserDetail: UserDetail;

    addUserDetail = {
      CompanyId: this.selectedCompany,
      FirstName: this.f.firstName.value,
      SurName: this.f.surName.value,
      UserName: this.f.userName.value,
      Password: this.f.password.value,
      Id: this.f.id.value,
      UserId: this.userID

    }

    this.userService.addUserwithObserve(addUserDetail)
      //.pipe(first())
      .subscribe(
        data => {
          if (data) {
           // this.addUserArray(data);
           // this.userDetails.push(data);
           this.usersList.push(data)
           // this.userDetails = this.userDetails.slice();
            //this.userDetailSubject.next(data);
            //this.changeDectector(data);
            this.loadCompany();
            this.resetFields();
          }
        },
        err => console.error(err),
        () => console.log('Added user')
      //  error => {
      //    this.error = error.message;
      //    this.loading = false;
      //}
    );
  }

  onSelectedRow(user: any) {
      if (user != null) {
      this.isEdited = true;
      let formControls = this.userForm;
      formControls.setValue({
        firstName: user.firstName,
        surName: user.surName,
        userName: user.userName,
        password: user.passwordHash,
        company: user.companyId,
        id: user.id
      });

      this.isPasswordReadonly = true;
      //formControls.controls["password"].disabled = true;
    }
  }

  resetFields() {
    this.isPasswordReadonly = false;
    this.userForm.reset();
  }

}
