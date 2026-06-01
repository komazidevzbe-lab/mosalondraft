import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { AccountService } from '../../_services/account.service';
import { ForgotPasswordRequest } from '../../_models/forgot-password-request';
import { VerifyResetCodeRequest } from '../../_models/verify-reset-code-request';
import { ResetPasswordRequest } from '../../_models/reset-password-request';

type ForgotPasswordStep = 'identify-user' | 'verify-code' | 'reset-password';

@Component({
  selector: 'app-forgotpassword',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './forgotpassword.component.html',
  styleUrl: './forgotpassword.component.css'
})
export class ForgotpasswordComponent {
  private readonly accountService = inject(AccountService);

  // ===============================
  // Forgot password flow state
  // Backend verification is now connected.
  // ===============================

  currentStep: ForgotPasswordStep = 'identify-user';

  forgotPasswordModel = {
    emailOrPhone: '',
    verificationCode: '',
    newPassword: '',
    confirmPassword: ''
  };

  errorMessage = '';
  successMessage = '';
  isSubmitting = false;

  showNewPassword = false;
  showConfirmPassword = false;

  // ===============================
  // Password visibility controls
  // ===============================

  toggleNewPasswordVisibility(): void {
    this.showNewPassword = !this.showNewPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  // ===============================
  // Step 1: Request verification code
  // Calls backend to create a reset code.
  // ===============================

  requestVerificationCode(): void {
    this.clearMessages();

    const emailOrPhone = this.forgotPasswordModel.emailOrPhone.trim();

    if (!emailOrPhone) {
      this.errorMessage = 'Please enter your registered email address or phone number.';
      return;
    }

    if (!this.isValidEmail(emailOrPhone) && !this.isValidPhoneNumber(emailOrPhone)) {
      this.errorMessage = 'Please enter a valid email address or phone number.';
      return;
    }

    const request: ForgotPasswordRequest = {
      emailOrPhone
    };

    this.isSubmitting = true;

    this.accountService.forgotPassword(request).subscribe({
      next: response => {
        this.isSubmitting = false;

        this.successMessage = response.developmentResetCode
          ? `${response.message} Development code: ${response.developmentResetCode}`
          : response.message;

        this.currentStep = 'verify-code';
      },
      error: error => {
        this.isSubmitting = false;
        this.errorMessage = this.accountService.getErrorMessage(
          error,
          'Could not create password reset code.'
        );
      }
    });
  }

  // ===============================
  // Step 2: Verify code
  // Calls backend to verify reset code.
  // ===============================

  verifyCode(): void {
    this.clearMessages();

    const emailOrPhone = this.forgotPasswordModel.emailOrPhone.trim();
    const verificationCode = this.forgotPasswordModel.verificationCode.trim();

    if (!verificationCode) {
      this.errorMessage = 'Please enter the verification code.';
      return;
    }

    if (!/^\d{6}$/.test(verificationCode)) {
      this.errorMessage = 'The verification code must be 6 digits.';
      return;
    }

    const request: VerifyResetCodeRequest = {
      emailOrPhone,
      verificationCode
    };

    this.isSubmitting = true;

    this.accountService.verifyResetCode(request).subscribe({
      next: response => {
        this.isSubmitting = false;
        this.successMessage = response.message;
        this.currentStep = 'reset-password';
      },
      error: error => {
        this.isSubmitting = false;
        this.errorMessage = this.accountService.getErrorMessage(
          error,
          'The verification code is incorrect or expired.'
        );
      }
    });
  }

  // ===============================
  // Step 3: Reset password
  // Calls backend to reset password.
  // ===============================

  resetPassword(): void {
    this.clearMessages();

    const emailOrPhone = this.forgotPasswordModel.emailOrPhone.trim();
    const verificationCode = this.forgotPasswordModel.verificationCode.trim();

    if (!this.isStrongPassword(this.forgotPasswordModel.newPassword)) {
      this.errorMessage = 'Password must be at least 8 characters and include uppercase, lowercase, number, and special character.';
      return;
    }

    if (this.forgotPasswordModel.newPassword !== this.forgotPasswordModel.confirmPassword) {
      this.errorMessage = 'Passwords do not match.';
      return;
    }

    const request: ResetPasswordRequest = {
      emailOrPhone,
      verificationCode,
      newPassword: this.forgotPasswordModel.newPassword,
      confirmPassword: this.forgotPasswordModel.confirmPassword
    };

    this.isSubmitting = true;

    this.accountService.resetPassword(request).subscribe({
      next: response => {
        this.isSubmitting = false;
        this.successMessage = response.message;

        this.forgotPasswordModel.verificationCode = '';
        this.forgotPasswordModel.newPassword = '';
        this.forgotPasswordModel.confirmPassword = '';
      },
      error: error => {
        this.isSubmitting = false;
        this.errorMessage = this.accountService.getErrorMessage(
          error,
          'Password could not be reset.'
        );
      }
    });
  }

  goBackToIdentifyStep(): void {
    this.clearMessages();
    this.currentStep = 'identify-user';
    this.forgotPasswordModel.verificationCode = '';
    this.forgotPasswordModel.newPassword = '';
    this.forgotPasswordModel.confirmPassword = '';
  }

  private clearMessages(): void {
    this.errorMessage = '';
    this.successMessage = '';
  }

  private isValidEmail(value: string): boolean {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
  }

  private isValidPhoneNumber(value: string): boolean {
    return /^(\+27|0)[6-8][0-9]{8}$/.test(value.replace(/\s/g, ''));
  }

  private isStrongPassword(value: string): boolean {
    return /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$/.test(value);
  }
}