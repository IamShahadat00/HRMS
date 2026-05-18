import { Component, OnInit, ChangeDetectorRef } from '@angular/core'; // 1. Add ChangeDetectorRef
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { EmployeeService } from '../../services/employee';
import { Employee } from '../../models/hrms.models';
import { NotificationService } from '../../services/notification';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './employee-list.html',
})
export class EmployeeListComponent implements OnInit {
  employees: Employee[] = [];
  isLoading = true;

  constructor(
    private employeeService: EmployeeService,
    private notify: NotificationService,
    private cdr: ChangeDetectorRef, // 2. Inject it here
  ) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  onSearch(event: any) {
    this.loadEmployees(event.target.value);
  }

  loadEmployees(search: string = '') {
    this.isLoading = true;

    this.employeeService.getEmployees(search).subscribe({
      next: (data) => {
        console.log('Data Arrived:', data);
        this.employees = data;
        this.isLoading = false;

        // 3. FORCE THE UI TO REFRESH
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Fetch error:', err);
        this.isLoading = false;
        this.notify.showError('Error', 'Failed to load data');
        this.cdr.detectChanges();
      },
    });
  }

  deleteEmployee(id: number) {
    this.notify.confirmAction('Delete Employee?', 'This is permanent').then((res) => {
      if (res) {
        this.employeeService.deleteEmployee(id).subscribe(() => {
          this.notify.showToast('Employee deleted');
          this.loadEmployees();
        });
      }
    });
  }
}
