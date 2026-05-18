import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { EmployeeService } from '../../services/employee';
import { NotificationService } from '../../services/notification';
import { Employee } from '../../models/hrms.models';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './employee-form.html',
})
export class EmployeeFormComponent implements OnInit {
  employeeForm: FormGroup;
  isEditMode = false;
  employeeId?: number;

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private route: ActivatedRoute,
    private router: Router,
    private notify: NotificationService,
  ) {
    this.employeeForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required]],
      position: ['', [Validators.required]],
      address: ['', [Validators.required]],
      departmentId: [1, [Validators.required]],
      accountNumber: ['', [Validators.required]],
      isActive: [true],
    });
  }

  ngOnInit(): void {
    this.employeeId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.employeeId) {
      this.isEditMode = true;
      this.loadEmployeeData();
    }
  }

  loadEmployeeData() {
    this.employeeService.getEmployeeById(this.employeeId!).subscribe((emp: Employee) => {
      if (emp) {
        this.employeeForm.patchValue(emp);
      }
    });
  }

  onSubmit() {
    if (this.employeeForm.invalid) {
      this.notify.showToast('Please fill all fields correctly', 'error');
      return;
    }

    const data = this.employeeForm.value;
    const action = this.isEditMode
      ? this.employeeService.updateEmployee(this.employeeId!, data)
      : this.employeeService.addEmployee(data);

    action.subscribe({
      next: () => {
        this.notify.showSuccess('Success', 'Employee record saved');
        this.router.navigate(['/dashboard/employees']);
      },
      error: (err) => this.notify.showError('Error', 'Failed to save record'),
    });
  }
}
