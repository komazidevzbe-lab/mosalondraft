import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  // ===============================
  // Login form data
  // This object temporarily stores what the user types into the form.
  // Later, when we build the backend, this data will be sent to the API.
  // ===============================

  loginModel = {
    username: '',
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
  // Temporary frontend-only login action
  // Later this will call the ASP.NET API.
  // ===============================

  login(): void {
    console.log('Login submitted:', this.loginModel);
  }
}