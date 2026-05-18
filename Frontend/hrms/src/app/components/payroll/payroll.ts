import { Component, ChangeDetectorRef } from '@angular/core'; // 1. Added ChangeDetectorRef
import { CommonModule } from '@angular/common';
import { PayrollService } from '../../services/payroll';
import { NotificationService } from '../../services/notification';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-payroll',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './payroll.html',
})
export class PayrollComponent {
  selectedMonth = new Date().getMonth() + 1;
  selectedYear = new Date().getFullYear();
  payrolls: any[] = [];
  isLoading = false;

  constructor(
    private payrollService: PayrollService,
    private notify: NotificationService,
    private cdr: ChangeDetectorRef, // 2. Inject it
  ) {}

  generate() {
    this.isLoading = true;
    this.payrolls = []; // Clear old table data

    this.payrollService.generatePayroll(this.selectedMonth, this.selectedYear).subscribe({
      next: (res: any) => {
        this.payrolls = res.data;
        this.notify.showSuccess('Success', res.message);
        this.isLoading = false;
        this.cdr.detectChanges(); // 3. Force UI refresh
      },
      error: (err) => {
        this.isLoading = false;
        // Extract exact message from Backend BadRequest or InternalError
        const errorMessage = err.error?.message || 'Check server connection.';
        this.notify.showError('Payroll Engine Stopped', errorMessage);
        this.cdr.detectChanges();
      },
    });
  }
}
