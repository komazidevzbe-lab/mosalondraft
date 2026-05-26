import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AccountService } from '../../_services/account.service';
import { RegisterRequest } from '../../_models/register-request';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  private accountService = inject(AccountService);
  private router = inject(Router);

  // ===============================
  // Signup form data
  // Users sign up with first name, last name, email, phone number,
  // password and confirm password.
  // ===============================

  signupModel = {
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    password: '',
    confirmPassword: '',
    agreeToTerms: false
  };

  // ===============================
  // Password visibility state
  // This controls whether the password inputs show text or dots.
  // ===============================

  showPassword = false;
  showConfirmPassword = false;

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  // ===============================
  // Signup action
  // Sends the signup form data to the ASP.NET API.
  // The frontend checks passwords first, then the backend checks again.
  // If successful, the user is taken to the homepage.
  // ===============================

  signup(): void {
    if (this.signupModel.password !== this.signupModel.confirmPassword) {
      console.error('Password and confirm password do not match.');
      return;
    }

    if (!this.signupModel.agreeToTerms) {
      console.error('Terms and conditions must be accepted.');
      return;
    }

    const registerRequest: RegisterRequest = {
      firstName: this.signupModel.firstName.trim(),
      lastName: this.signupModel.lastName.trim(),
      email: this.signupModel.email.trim().toLowerCase(),
      phoneNumber: this.signupModel.phoneNumber.trim(),
      password: this.signupModel.password,
      confirmPassword: this.signupModel.confirmPassword
    };

    this.accountService.register(registerRequest).subscribe({
      next: () => {
        this.router.navigateByUrl('/home');
      },
      error: error => {
        console.error('Signup failed:', error);
      }
    });
  }
}