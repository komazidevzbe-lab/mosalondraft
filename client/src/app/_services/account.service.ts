import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { map } from 'rxjs';

import { environment } from '../../environments/environment';
import { LoginRequest } from '../_models/login-request';
import { RegisterRequest } from '../_models/register-request';
import { User } from '../_models/user';

@Injectable({ providedIn: 'root' })
export class AccountService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;

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

  logout(): void {
    this.clearStorage();
    this.currentUser.set(null);
    this.clearLogoutTimer();
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