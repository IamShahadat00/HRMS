import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../../services/auth';
import { NotificationService } from '../../services/notification';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './dashboard.html',
})
export class DashboardComponent {
  constructor(
    private authService: AuthService,
    private router: Router,
    private notify: NotificationService, // Inject notification
  ) {}

  async logout() {
    // 1. Ask for confirmation
    const confirmed = await this.notify.confirmAction(
      'Are you sure?',
      'You will need to login again to access the HRMS.',
    );

    if (confirmed) {
      // 2. Clear token from AuthService
      this.authService.logout();

      // 3. Show a success toast
      this.notify.showToast('Logged out successfully', 'info');

      // 4. Redirect to login
      this.router.navigate(['/login']);
    }
  }
}
