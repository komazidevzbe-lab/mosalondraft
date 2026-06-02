import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import {
  ClientReviewResponse,
  ContactMessage,
  CreateContactMessageRequest
} from '../_models/contact';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private readonly http = inject(HttpClient);

  // ===============================
  // API connection
  // All Contact page requests go through this service.
  // ===============================

  private readonly baseUrl = 'https://localhost:5001/api/';

  // ===============================
  // Contact messages
  // Saves "Send Us A Message" form submissions in the backend.
  // ===============================

  submitContactMessage(request: CreateContactMessageRequest): Observable<ContactMessage> {
    return this.http.post<ContactMessage>(`${this.baseUrl}contact/messages`, request);
  }

  getMyMessages(): Observable<ContactMessage[]> {
    return this.http.get<ContactMessage[]>(`${this.baseUrl}contact/messages/my-messages`);
  }

  // ===============================
  // Client reviews
  // Saves feedback reviews into the shared ClientReviews table.
  // ===============================

  submitReview(formData: FormData): Observable<ClientReviewResponse> {
    return this.http.post<ClientReviewResponse>(`${this.baseUrl}contact/reviews`, formData);
  }
}