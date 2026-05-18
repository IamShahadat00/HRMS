import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { DashboardComponent } from './components/dashboard/dashboard';
import { EmployeeListComponent } from './components/employee-list/employee-list';
import { EmployeeFormComponent } from './components/employee-form/employee-form';
import { SalaryListComponent } from './components/salary-list/salary-list';
import { SalarySetupComponent } from './components/salary-setup/salary-setup';
import { PayrollComponent } from './components/payroll/payroll';
import { authGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authGuard],
    children: [
      { path: 'employees', component: EmployeeListComponent },
      { path: 'employees/add', component: EmployeeFormComponent },
      { path: 'employees/edit/:id', component: EmployeeFormComponent },
      { path: 'salary', component: SalaryListComponent },
      { path: 'salary/setup/:id', component: SalarySetupComponent },
      { path: 'payroll', component: PayrollComponent },
      { path: '', redirectTo: 'employees', pathMatch: 'full' },
    ],
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' },
];
