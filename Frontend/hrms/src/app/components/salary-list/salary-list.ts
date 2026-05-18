import { Component, OnInit, ChangeDetectorRef } from '@angular/core'; // 1. Add ChangeDetectorRef
import { CommonModule, CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { SalaryService } from '../../services/salary';

@Component({
  selector: 'app-salary-list',
  standalone: true,
  imports: [CommonModule, RouterLink, CurrencyPipe],
  templateUrl: './salary-list.html',
})
export class SalaryListComponent implements OnInit {
  salaries: any[] = [];
  isLoading = true;

  constructor(
    private salaryService: SalaryService,
    private cdr: ChangeDetectorRef, // 2. Inject it here
  ) {}

  ngOnInit(): void {
    this.loadSalaries();
  }

  loadSalaries() {
    this.isLoading = true;
    this.salaryService.getAllSalaries().subscribe({
      next: (data) => {
        console.log('Salaries Loaded:', data);
        this.salaries = data;
        this.isLoading = false;

        // 3. FORCE THE TABLE TO APPEAR IMMEDIATELY
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error fetching salaries', err);
        this.isLoading = false;
        this.cdr.detectChanges();
      },
    });
  }
}
