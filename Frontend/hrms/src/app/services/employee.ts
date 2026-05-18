import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Employee } from '../models/hrms.models';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class EmployeeService {
  private apiUrl = `${environment.apiUrl}/employee`;

  constructor(private http: HttpClient) {}

  // Allows searching via the backend: /api/employee?search=...
  getEmployees(search: string = ''): Observable<Employee[]> {
    return this.http.get<Employee[]>(`${this.apiUrl}?search=${search}`);
  }

  getEmployeeById(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`);
  }

  addEmployee(emp: Employee): Observable<any> {
    return this.http.post(this.apiUrl, emp);
  }

  updateEmployee(id: number, emp: Employee): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, emp);
  }

  deleteEmployee(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}