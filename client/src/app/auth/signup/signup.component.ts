import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  // ===============================
  // Signup form data
  // This object temporarily stores what the user types into the form.
  // Later, when we build the backend, this data will be sent to the API.
  // ===============================

  signupModel = {
    fullName: '',
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
  // Temporary frontend-only signup action
  // Later this will call the ASP.NET API.
  // ===============================

  signup(): void {
    console.log('Signup submitted:', this.signupModel);
  }
}