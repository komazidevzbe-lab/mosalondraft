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
  private accountService = inject(AccountService);
  private router = inject(Router);

  // ===============================
  // Login form data
  // Users sign in with email and password.
  // ===============================

  loginModel = {
    email: '',
    password: '',
    rememberMe: false
  };

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
    const loginRequest: LoginRequest = {
      email: this.loginModel.email.trim().toLowerCase(),
      password: this.loginModel.password
    };

    this.accountService.login(loginRequest).subscribe({
      next: () => {
        this.router.navigateByUrl('/home');
      },
      error: error => {
        console.error('Login failed:', error);
      }
    });
  }
}