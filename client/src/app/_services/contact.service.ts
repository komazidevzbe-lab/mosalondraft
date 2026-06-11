import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import {
  ClientReviewResponse,
  ContactMessage,
  CreateContactMessageRequest
} from '../_models/contact';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private readonly http = inject(HttpClient);

  // ===============================
  // API connection
  // Uses environment.apiUrl so local development and Azure deployment
  // both use the correct backend URL.
  // ===============================

  private readonly baseUrl = environment.apiUrl;

  // ===============================
  // Contact messages
  // Uses the logged-in user's token for protected message endpoints.
  // ===============================

  submitContactMessage(request: CreateContactMessageRequest): Observable<ContactMessage> {
    return this.http.post<ContactMessage>(
      `${this.baseUrl}contact/messages`,
      request,
      { headers: this.getAuthHeaders() }
    );
  }

  getMyMessages(): Observable<ContactMessage[]> {
    return this.http.get<ContactMessage[]>(
      `${this.baseUrl}contact/messages/my-messages`,
      { headers: this.getAuthHeaders() }
    );
  }

  // ===============================
  // Client reviews
  // Saves feedback reviews into the shared ClientReviews table.
  // ===============================

  submitReview(formData: FormData): Observable<ClientReviewResponse> {
    return this.http.post<ClientReviewResponse>(
      `${this.baseUrl}contact/reviews`,
      formData,
      { headers: this.getAuthHeaders() }
    );
  }

  private getAuthHeaders(): HttpHeaders {
    const user = this.getStoredUser();

    if (!user?.token) {
      return new HttpHeaders();
    }

    return new HttpHeaders({
      Authorization: `Bearer ${user.token}`
    });
  }

  private getStoredUser(): User | null {
    const storedUser = sessionStorage.getItem('user');

    if (!storedUser) {
      return null;
    }

    try {
      return JSON.parse(storedUser) as User;
    } catch {
      return null;
    }
  }
}