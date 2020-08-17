import { ModuleWithProviders }  from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_helpers'
import { CustomerDetailComponent } from './customer-detail/customer-detail.component';
import { BulkUploadComponent } from './bulk-upload/bulk-upload.component';
import { AddCompanyComponent } from './add-company/add-company.component';
import { AddUsersComponent } from './add-users/add-users.component';
import { LoginComponent } from './login';

const appRoutes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard] },
  // { path: 'account', loadChildren: 'app/account/account.module#AccountModule' },
  { path: 'login', component: LoginComponent },
  { path: 'customer-detail', component: CustomerDetailComponent },
  { path: 'bulk-upload', component: BulkUploadComponent },
  { path: 'add-users', component: AddUsersComponent },
  { path: 'add-company', component: AddCompanyComponent }

];

//export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
export const appRoutingModule = RouterModule.forRoot(appRoutes);
