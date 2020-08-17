import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  url: string;

  constructor(private http: HttpClient) {
    this.url = `${environment.apiUrl}/customer/`;
  }


  uploadFile(uploadData: FormData): Observable<any> {

    return this.http.post(this.url + 'upload', uploadData, { reportProgress: true, observe: 'events' });

  }

  invokeScore(username: string, password: string) {
    return this.http.post<any>(this.url + 'InvokeScore', { username, password });
      //.pipe(map(user => {
      //  // store user details and jwt token in local storage to keep user logged in between page refreshes
      //  localStorage.setItem('currentUser', JSON.stringify(user));
      //  return user;
      //}));
  }
}
