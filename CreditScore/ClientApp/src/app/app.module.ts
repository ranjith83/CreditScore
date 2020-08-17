import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';


import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';

//import { AccountModule } from './account/account.module';
import { ConfigService } from './shared/utils/config.service';
import { LoginComponent } from './login/login.component';
import { appRoutingModule } from './app.routing';
import { CustomerDetailComponent } from './customer-detail/customer-detail.component';
import { BulkUploadComponent } from './bulk-upload/bulk-upload.component';
import { AddUsersComponent } from './add-users/add-users.component';
import { AddCompanyComponent } from './add-company/add-company.component';
import { AgGridModule } from "ag-grid-angular";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LoginComponent,
    CustomerDetailComponent,
    BulkUploadComponent,
    AddUsersComponent,
    AddCompanyComponent,

  ],
  imports: [
   // AccountModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    appRoutingModule,
   AgGridModule.withComponents([])
  ],  
  providers: [ConfigService],
  //providers: []ConfigService, {
  //  provide: XHRBackend,
  //  useClass: AuthenticateXHRBackend
  //}],
  bootstrap: [AppComponent]
})
export class AppModule { }
