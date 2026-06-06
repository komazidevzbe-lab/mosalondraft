import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { BookingReview } from '../../_models/booking';
import { AccountService } from '../../_services/account.service';
import { BookingService } from '../../_services/booking.service';
import { BookingStateService } from '../../_services/booking-state.service';

interface ConfirmedService {
  title: string;
  selectedType: string;
  date: string;
  time: string;
}

@Component({
  selector: 'app-confirmation',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './confirmation.component.html',
  styleUrl: './confirmation.component.css'
})
export class ConfirmationComponent implements OnInit, OnDestroy {
  private readonly route = inject(ActivatedRoute);
  private readonly accountService = inject(AccountService);
  private readonly bookingService = inject(BookingService);
  private readonly bookingStateService = inject(BookingStateService);

  private readonly maxConfirmationAttempts = 12;
  private readonly retryDelayMs = 2500;
  private confirmationRetryTimer: ReturnType<typeof setTimeout> | null = null;

  booking: BookingReview | null = null;
  bookingId: number | null = null;

  bookingReference = '';
  depositPaid = 0;
  totalAmount = 0;

  services: ConfirmedService[] = [];

  client = {
    fullName: '',
    phoneNumber: '',
    emailAddress: ''
  };

  isLoadingConfirmation = false;
  confirmationErrorMessage = '';

  ngOnInit(): void {
    this.loadConfirmation();
  }

  ngOnDestroy(): void {
    this.clearConfirmationRetryTimer();
  }

  get hasConfirmedBooking(): boolean {
    return this.booking !== null;
  }

  get balanceRemaining(): number {
    return this.totalAmount - this.depositPaid;
  }

  get reviewBookingQueryParams(): { bookingId?: number } {
    if (!this.bookingId) {
      return {};
    }

    return {
      bookingId: this.bookingId
    };
  }

  retryConfirmation(): void {
    if (!this.bookingId || this.isLoadingConfirmation) {
      return;
    }

    this.confirmationErrorMessage = '';
    this.loadConfirmedBookingWithRetry(this.bookingId, 1);
  }

  private loadConfirmation(): void {
    const routeBookingId = Number(this.route.snapshot.queryParamMap.get('bookingId'));
    const savedBookingId = this.bookingStateService.getState()?.pendingBookingId;
    const bookingId = routeBookingId || savedBookingId;

    if (!bookingId) {
      this.confirmationErrorMessage = 'No booking ID was found.';
      return;
    }

    this.bookingId = bookingId;
    this.loadConfirmedBookingWithRetry(bookingId, 1);
  }

  private loadConfirmedBookingWithRetry(bookingId: number, attemptNumber: number): void {
    this.clearConfirmationRetryTimer();

    this.isLoadingConfirmation = true;

    if (attemptNumber === 1) {
      this.confirmationErrorMessage = '';
    } else {
      this.confirmationErrorMessage =
        'Payment received. Waiting for PayFast to confirm your booking...';
    }

    this.bookingService.getConfirmedBooking(bookingId).subscribe({
      next: booking => {
        this.applyConfirmedBooking(booking);
        this.confirmationErrorMessage = '';
        this.isLoadingConfirmation = false;
      },
      error: error => {
        if (attemptNumber < this.maxConfirmationAttempts) {
          this.confirmationRetryTimer = setTimeout(() => {
            this.loadConfirmedBookingWithRetry(bookingId, attemptNumber + 1);
          }, this.retryDelayMs);

          return;
        }

        console.error('Failed to load confirmed booking:', error);

        this.booking = null;
        this.isLoadingConfirmation = false;
        this.confirmationErrorMessage = this.accountService.getErrorMessage(
          error,
          'Your payment was completed, but your booking confirmation is still processing. Please try again in a moment.'
        );
      }
    });
  }

  private applyConfirmedBooking(booking: BookingReview): void {
    this.booking = booking;

    this.bookingReference = booking.bookingReference;
    this.depositPaid = booking.depositAmount;
    this.totalAmount = booking.totalAmount;

    this.client = {
      fullName: booking.clientFullName,
      phoneNumber: booking.clientPhoneNumber,
      emailAddress: booking.clientEmailAddress
    };

    this.services = booking.items.map(item => ({
      title: item.serviceName,
      selectedType: item.lengthName
        ? `${item.serviceTypeName} • ${item.lengthName}`
        : item.serviceTypeName,
      date: this.formatSelectedDate(item.appointmentDate),
      time: this.formatTime(item.startTime)
    }));
  }

  private formatSelectedDate(isoDate: string): string {
    if (!isoDate) {
      return 'No date selected';
    }

    return new Date(`${isoDate}T00:00:00`).toLocaleDateString('en-ZA', {
      day: 'numeric',
      month: 'long',
      year: 'numeric'
    });
  }

  private formatTime(time: string): string {
    if (!time) {
      return 'No time selected';
    }

    return time.slice(0, 5);
  }

  private clearConfirmationRetryTimer(): void {
    if (this.confirmationRetryTimer) {
      clearTimeout(this.confirmationRetryTimer);
      this.confirmationRetryTimer = null;
    }
  }
}