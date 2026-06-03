import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { map } from 'rxjs';

import { environment } from '../../environments/environment';
import { AuthMessage } from '../_models/auth-message';
import { ForgotPasswordRequest } from '../_models/forgot-password-request';
import { LoginRequest } from '../_models/login-request';
import { RegisterRequest } from '../_models/register-request';
import { ResetPasswordRequest } from '../_models/reset-password-request';
import { User } from '../_models/user';
import { VerifyResetCodeRequest } from '../_models/verify-reset-code-request';

@Injectable({ providedIn: 'root' })
export class AccountService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  currentUser = signal<User | null>(null);

  // Holds the pending auto-logout timer so it can be cleared/reset.
  private logoutTimer: ReturnType<typeof setTimeout> | null = null;

  roles = computed(() => {
    const user = this.currentUser();

    if (!user?.token) {
      return [];
    }

    try {
      const payload = JSON.parse(atob(user.token.split('.')[1]));
      const extractedRoles = payload.role || payload.roles || [];

      return Array.isArray(extractedRoles) ? extractedRoles : [extractedRoles];
    } catch (error) {
      console.error('Failed to decode JWT roles:', error);
      return [];
    }
  });

  getUserRole(): string {
    const roles = this.roles();
    return roles.length > 0 ? roles[0] : '';
  }

  login(model: LoginRequest) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map(user => {
        if (user) {
          this.setCurrentUser(user);
        }

        return user;
      })
    );
  }

  register(model: RegisterRequest) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if (user) {
          this.setCurrentUser(user);
        }

        return user;
      })
    );
  }

  forgotPassword(model: ForgotPasswordRequest) {
    return this.http.post<AuthMessage>(this.baseUrl + 'account/forgot-password', model);
  }

  verifyResetCode(model: VerifyResetCodeRequest) {
    return this.http.post<AuthMessage>(this.baseUrl + 'account/verify-reset-code', model);
  }

  resetPassword(model: ResetPasswordRequest) {
    return this.http.post<AuthMessage>(this.baseUrl + 'account/reset-password', model);
  }

  serverLogout() {
    return this.http.post<AuthMessage>(
      this.baseUrl + 'account/logout',
      {},
      {
        headers: this.getAuthHeaders()
      }
    );
  }

  setCurrentUser(user: User): void {
    const expiryMs = this.getTokenExpiryMs(user?.token);
    const now = Date.now();

    if (!expiryMs || expiryMs <= now) {
      this.clearStorage();
      this.currentUser.set(null);
      this.clearLogoutTimer();
      return;
    }

    sessionStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);

    this.startAutoLogoutTimer(expiryMs - now);
  }

  loadCurrentUserFromStorage(): void {
    const userJson = sessionStorage.getItem('user');

    if (!userJson) {
      this.currentUser.set(null);
      return;
    }

    try {
      const user = JSON.parse(userJson) as User;
      this.setCurrentUser(user);
    } catch (error) {
      console.error('Failed to load user from storage:', error);
      this.logout();
    }
  }

  logout(): void {
    this.clearStorage();
    this.currentUser.set(null);
    this.clearLogoutTimer();
  }

  getErrorMessage(error: any, fallbackMessage: string): string {
    if (typeof error?.error === 'string') {
      return error.error;
    }

    if (error?.error?.message) {
      return error.error.message;
    }

    if (error?.error?.email) {
      return error.error.email;
    }

    if (error?.error?.password) {
      return error.error.password;
    }

    if (error?.error?.phoneNumber) {
      return error.error.phoneNumber;
    }

    if (error?.error?.confirmPassword) {
      return error.error.confirmPassword;
    }

    if (error?.error?.emailOrPhone) {
      return error.error.emailOrPhone;
    }

    if (error?.error?.verificationCode) {
      return error.error.verificationCode;
    }

    if (error?.error?.newPassword) {
      return error.error.newPassword;
    }

    if (error?.error?.errors) {
      const validationErrors = error.error.errors;

      const firstKey = Object.keys(validationErrors)[0];

      if (firstKey && Array.isArray(validationErrors[firstKey])) {
        return validationErrors[firstKey][0];
      }
    }

    if (Array.isArray(error?.error)) {
      return error.error[0]?.description || fallbackMessage;
    }

    return fallbackMessage;
  }

  getAuthHeaders(): HttpHeaders {
    const token = this.currentUser()?.token;

    if (!token) {
      return new HttpHeaders();
    }

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  private getTokenExpiryMs(token?: string | null): number | null {
    if (!token) {
      return null;
    }

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return typeof payload.exp === 'number' ? payload.exp * 1000 : null;
    } catch (error) {
      console.error('Failed to decode JWT expiry:', error);
      return null;
    }
  }

  private startAutoLogoutTimer(timeoutMs: number): void {
    this.clearLogoutTimer();

    if (timeoutMs <= 0) {
      this.logout();
      return;
    }

    this.logoutTimer = setTimeout(() => {
      this.logout();
    }, timeoutMs);
  }

  private clearLogoutTimer(): void {
    if (this.logoutTimer) {
      clearTimeout(this.logoutTimer);
      this.logoutTimer = null;
    }
  }

  private clearStorage(): void {
    sessionStorage.removeItem('user');
  }
}