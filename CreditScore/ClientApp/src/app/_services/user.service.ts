import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, } from '@angular/common/http';

import { environment } from '../../environments/environment';

import { map } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';
import { UserDetail } from '../_models/credit-model';


@Injectable({ providedIn: 'root' })
export class UserService {

  url = ''
  constructor(private http: HttpClient) {
    this.url = `${environment.apiUrl}/users/`;
  }

   

  getUserByCompany(companyID: number) {
    return this.http.get<any>(`${environment.apiUrl}/users/GetUserByCompany/${companyID}`)
      .pipe(map(user => {
        return user;
      }));
  }



  addUser(userDetail: UserDetail) {
    const userName = userDetail.UserName;
    const firstName = userDetail.FirstName;
    return this.http.post<any>(`${environment.apiUrl}/users/AddUser`, { userName, firstName })
      .pipe(map(user => {
        return user;
      }));
  }

  addUserwithObserve(userDetail: UserDetail): Observable<UserDetail> {
    //let headers = new Headers({ 'Content-Type': 'application/json' });
    ////let options = new RequestOptions({ headers: headers });
    //let options = {
    //  headers: headers
    //}

    const header = new HttpHeaders({ 'Content-Type': 'application/json' });
    //const pass = 'Basic ' + btoa(cuid + ': ');
    //header.set('Authorization', pass);
    const options = ({
      headers: header
    });
    //return this.http.post(this.url + 'AddUser', userDetail, options)
    //  .subscribe(data => {
    //    console.log(data);
    //  });

    return this.http.post<UserDetail>(this.url + 'AddUpdateUser', userDetail, options);
      
  }
}
