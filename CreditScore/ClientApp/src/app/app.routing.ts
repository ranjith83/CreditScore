import { ModuleWithProviders }  from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_helpers'
import { CustomerDetailComponent } from './customer-detail/customer-detail.component';
import { BulkUploadComponent } from './bulk-upload/bulk-upload.component';
import { AddCompanyComponent } from './add-company/add-company.component';
import { AddUsersComponent } from './add-users/add-users.component';
import { LoginComponent } from './login';
import { GetCustomerScoreComponent } from './get-customer-score/get-customer-score.component';
import { GetReportsComponent } from './get-reports/get-reports.component';
import { Role } from './_models/role';


const appRoutes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard] },
  // { path: 'account', loadChildren: 'app/account/account.module#AccountModule' },
  { path: 'login', component: LoginComponent },
  { path: 'get-customer-score', component: GetCustomerScoreComponent, canActivate: [AuthGuard] },
  { path: 'customer-detail', component: CustomerDetailComponent, canActivate: [AuthGuard]},
  { path: 'bulk-upload', component: BulkUploadComponent, canActivate: [AuthGuard]},
  { path: 'add-users', component: AddUsersComponent, canActivate: [AuthGuard], data: { roles: [Role.Admin] } },
  { path: 'add-company', component: AddCompanyComponent, canActivate: [AuthGuard], data: { roles: [Role.Admin] } },
  { path: 'get-reports', component: GetReportsComponent, canActivate: [AuthGuard]}

];

//export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
export const appRoutingModule = RouterModule.forRoot(appRoutes);
