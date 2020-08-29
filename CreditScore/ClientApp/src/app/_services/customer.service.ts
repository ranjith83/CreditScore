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


  invokeCreditScore(userName:string, idNumber: string) {
    return this.http.post<any>(this.url + 'invokeCreditScore', { userName, idNumber });
    //.pipe(map(user => {
    //  // store user details and jwt token in local storage to keep user logged in between page refreshes
    //  localStorage.setItem('currentUser', JSON.stringify(user));
    //  return user;
    //}));
  }

  getUserScore(userId: number) {
    return this.http.get<any>(this.url + "GetUserScore/" + userId)
      .pipe(map(score => {
        return score;
      }));
  }

  getUserCredits(userId: number) {
    return this.http.get<any>(this.url + "getUserCredits/" + userId)
      .pipe(map(credits => {
        return credits;
      }));
  }


  getAllCustomer() {
    return this.http.get<any>(this.url + "getAllCustomer");
  }

  addCustomer(customerDetail: any): Observable<any> {
    return this.http.post<any>(this.url + 'AddCustomer', customerDetail);
  }
  

  getUserReports(userId: number) {
    return this.http.get<any>(this.url + "getUserReports/" + userId)
      .pipe(map(credits => {
        return credits;
      }));
  }

}
