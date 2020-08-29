import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { map } from 'rxjs/operators';
import { CompanyDetail } from '../_models/credit-model';



@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  url: string;


  constructor(private http: HttpClient) {
    this.url = `${environment.apiUrl}/company/`;


  }

  addCompany(companyDetail: CompanyDetail): Observable<CompanyDetail> {

    const header = new HttpHeaders({ 'Content-Type': 'application/json' });
    //const pass = 'Basic ' + btoa(cuid + ': ');
    //header.set('Authorization', pass);
    const options = ({
      headers: header
    });
    return this.http.post<any>(this.url + 'addcompany', companyDetail, options);
      
  }


  getAllCompany(): Observable<CompanyDetail[]> {

    return this.http.get<CompanyDetail[]>(this.url + 'getAllCompany');

  }

  getCompanyForSelect(): Observable<any> {
    return this.http.get<any>(this.url + 'GetCompanyForSelect');
  }
}
