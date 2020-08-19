import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first, map, take } from 'rxjs/operators';
import { AuthenticationService, UserService } from '../_services';
import { GridOptions } from 'ag-grid-community';

import { Observable, BehaviorSubject, of } from 'rxjs';
import { UserDetail } from '../_models/credit-model';


@Component({
  selector: 'app-add-users',
  templateUrl: './add-users.component.html',
  styleUrls: ['./add-users.component.scss']
})
export class AddUsersComponent implements OnInit {

  userForm: FormGroup;
  loading = false;
  submitted = false;
  private gridOptions: GridOptions;
  public rowData:any[];
  private columnDefs: any[];
  public userDetails: UserDetail[];
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

 //addUser: UserDetail;
  //currentData = this.userDetailSubject.asObservable();

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
  

    //private alertService: AlertService
  ) {
    // redirect to home if already logged in
    //if (this.authenticationService.currentUserValue) {
    //  this.router.navigate(['/']);
    //}

    this.companyID = this.authenticationService.currentUserValue.companyId;
    this.userID = this.authenticationService.currentUserValue.id;
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
      company: ['', [Validators.required]]
    });

    
    this.loading = true;
    
    if (this.companyID != null) {
      this.userService.getUserByCompany(this.companyID)
        .subscribe(
          data => {
            this.userDetails = data;
            this.changeDectector(data);
            this.addUserArray(data);
            this.loading = false;
          }
          //error => {
          //  //this.alertService.error(error);
          //  this.loading = false;
        //}
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

  addUsers() {
    let addUserDetail: UserDetail;

    addUserDetail = {
      CompanyId: this.companyID | this.f.company.value,
      FirstName: this.f.firstName.value,
      SurName: this.f.surName.value,
      UserName: this.f.userName.value,
      Password: this.f.password.value,
      Id: 0,
      UserId: this.userID

    }

    this.userService.addUserwithObserve(addUserDetail)
      //.pipe(first())
      .subscribe(
        data => {
          if (data) {
            this.addUserArray(data);
            this.userDetails.push(data);
            
           // this.userDetails = this.userDetails.slice();
            //this.userDetailSubject.next(data);
            //this.changeDectector(data);
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

}
