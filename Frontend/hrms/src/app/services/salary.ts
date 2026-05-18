import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { Salary } from '../models/hrms.models';

@Injectable({ providedIn: 'root' })
export class SalaryService {
  private apiUrl = `${environment.apiUrl}/salary`;

  constructor(private http: HttpClient) {}

  // ADD THIS METHOD:
  getAllSalaries(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/all`); 
  }

  getSalaryByEmployee(empId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/employee/${empId}`);
  }

  setSalary(salary: any): Observable<any> {
    return this.http.post(this.apiUrl, salary);
  }
}