import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth';
import { NotificationService } from '../../services/notification';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html' 
})
export class LoginComponent {
  loginData = { userName: '', password: '' };
  errorMessage = '';

  constructor(
    private authService: AuthService, 
    private router: Router,
    private notify: NotificationService
  ) {}

  onLogin() {
    this.authService.login(this.loginData).subscribe({
      next: (res) => {
        this.notify.showToast('Welcome back! Login Successful.');
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.notify.showError('Login Failed', 'Invalid username or password.');
      }
    });
  }
}