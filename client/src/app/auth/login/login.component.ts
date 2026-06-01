import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AccountService } from '../../_services/account.service';
import { LoginRequest } from '../../_models/login-request';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private readonly accountService = inject(AccountService);
  private readonly router = inject(Router);

  // ===============================
  // Login form data
  // Users sign in with email and password.
  // ===============================

  loginModel = {
    email: '',
    password: ''
  };

  errorMessage = '';
  isSubmitting = false;

  // ===============================
  // Password visibility state
  // This controls whether the password input shows text or dots.
  // ===============================

  showPassword = false;

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  // ===============================
  // Login action
  // Sends the email and password to the ASP.NET API.
  // If successful, the user is taken to the homepage.
  // ===============================

  login(): void {
    this.errorMessage = '';

    const loginRequest: LoginRequest = {
      email: this.loginModel.email.trim().toLowerCase(),
      password: this.loginModel.password
    };

    if (!loginRequest.email || !loginRequest.password) {
      this.errorMessage = 'Please enter your email address and password.';
      return;
    }

    this.isSubmitting = true;

    this.accountService.login(loginRequest).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigateByUrl('/home');
      },
      error: error => {
        this.isSubmitting = false;
        this.errorMessage = this.accountService.getErrorMessage(
          error,
          'Login failed. Please check your details and try again.'
        );
      }
    });
  }
}