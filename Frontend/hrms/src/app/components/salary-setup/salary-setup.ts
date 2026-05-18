import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { SalaryService } from '../../services/salary';
import { NotificationService } from '../../services/notification';

@Component({
  selector: 'app-salary-setup',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './salary-setup.html' // FIX: Removed .component
})
export class SalarySetupComponent implements OnInit {
  salaryForm: FormGroup;
  empId!: number;

  constructor(
    private fb: FormBuilder,
    private salaryService: SalaryService,
    private route: ActivatedRoute,
    private router: Router,
    private notify: NotificationService
  ) {
    this.salaryForm = this.fb.group({
      employeeId: [0],
      baseSalary: [0, [Validators.required, Validators.min(1)]],
      medicalAllowance: [0, [Validators.min(0)]],
      conveyanceAllowance: [0, [Validators.min(0)]],
      otherBonuses: [0, [Validators.min(0)]],
      deductions: [0, [Validators.min(0)]]
    });
  }

  ngOnInit(): void {
    // Get the employee ID from the URL (/dashboard/salary/setup/5)
    this.empId = Number(this.route.snapshot.paramMap.get('id'));
    this.salaryForm.patchValue({ employeeId: this.empId });
    
    // Check if this employee already has a salary so we can "Edit" it
    this.salaryService.getSalaryByEmployee(this.empId).subscribe(data => {
      if (data) {
        this.salaryForm.patchValue(data);
      }
    });
  }

  onSubmit() {
    if (this.salaryForm.invalid) {
      this.notify.showToast('Please enter valid amounts', 'warning');
      return;
    }

    this.salaryService.setSalary(this.salaryForm.value).subscribe({
      next: () => {
        this.notify.showToast('Salary Configuration Saved Successfully');
        this.router.navigate(['/dashboard/salary']); // Go back to the list
      },
      error: (err) => this.notify.showError('Error', 'Failed to save salary settings.')
    });
  }
}