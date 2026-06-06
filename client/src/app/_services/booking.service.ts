import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

import { environment } from '../../environments/environment';
import { BookingReview, CreateBooking } from '../_models/booking';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private readonly http = inject(HttpClient);
  private readonly accountService = inject(AccountService);
  private readonly baseUrl = environment.apiUrl;

  createPendingBooking(model: CreateBooking) {
    return this.http.post<BookingReview>(this.baseUrl + 'bookings', model, {
      headers: this.accountService.getAuthHeaders()
    });
  }

  getBookingReview(bookingId: number) {
    return this.http.get<BookingReview>(`${this.baseUrl}bookings/${bookingId}/review`, {
      headers: this.accountService.getAuthHeaders()
    });
  }

  getConfirmedBooking(bookingId: number) {
    return this.http.get<BookingReview>(`${this.baseUrl}bookings/${bookingId}/confirmation`, {
      headers: this.accountService.getAuthHeaders()
    });
  }
}