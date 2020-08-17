import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { UserRegistration } from '../models/user.registration.interface';
import { ConfigService } from '../utils/config.service';

import {BaseService} from "./base.service";
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../models/user';


//import * as _ from 'lodash';

// Add the RxJS Observable operators we need in this app.


@Injectable()

export class UserService extends BaseService {

  baseUrl: string = '';
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;
  

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  
  // Observable navItem stream
  //authNavStatus$ = this._authNavStatusSource.asObservable();

  private loggedIn = false;

  constructor(private http: HttpClient, private configService: ConfigService) {
    super();
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.loggedIn = !!localStorage.getItem('auth_token');
    // ?? not sure if this the best way to broadcast the status but seems to resolve issue on page refresh where auth status is lost in
    // header component resulting in authed user nav links disappearing despite the fact user is still logged in
   // this._authNavStatusSource.next(this.loggedIn);
    this.baseUrl = configService.getApiURI();
  }

    register(email: string, password: string, firstName: string, lastName: string,location: string): void {
    //let body = JSON.stringify({ email, password, firstName, lastName,location });
    //let headers = new Headers({ 'Content-Type': 'application/json' });
    //let options = new RequestOptions({ headers: headers });

    //return this.http.post(this.baseUrl + "/accounts", body, options)
    //  .map(res => true)
    //  .catch(this.handleError);
    }

  login(username: string, password: string) {
    return this.http.post<any>(`${this.baseUrl}/users/authenticate`, { username, password })
      .pipe(map(user => {
        // store user details and jwt token in local storage to keep user logged in between page refreshes
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        return user;
      }));
  }

  // login(userName, password) {
  //  //let headers = new Headers();
  //  //headers.append('Content-Type', 'application/json');

  //  //return this.http
  //  //  .post(
  //  //  this.baseUrl + '/auth/login',
  //  //  JSON.stringify({ userName, password }),{ headers }
  //  //  )
  //  //  .map(res => res.json())
  //  //  .map(res => {
  //  //    localStorage.setItem('auth_token', res.auth_token);
  //  //    this.loggedIn = true;
  //  //    this._authNavStatusSource.next(true);
  //  //    return true;
  //  //  })
  //  //  .catch(this.handleError);
  //}

  logout() {
    localStorage.removeItem('auth_token');
    this.loggedIn = false;
    //this._authNavStatusSource.next(false);
  }

  isLoggedIn() {
    return this.loggedIn;
  }  
}


