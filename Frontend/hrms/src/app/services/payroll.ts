import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PayrollService {
  private apiUrl = `${environment.apiUrl}/payroll`;

  constructor(private http: HttpClient) {}

  generatePayroll(month: number, year: number): Observable<any> {
    // Ensure numbers are sent correctly
    return this.http.post(`${this.apiUrl}/generate`, { 
      month: Number(month), 
      year: Number(year) 
    });
  }

  getEmployeePayrolls(empId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/employee/${empId}`);
  }
}