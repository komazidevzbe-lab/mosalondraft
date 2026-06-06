import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { BookingReview } from '../../_models/booking';
import { AccountService } from '../../_services/account.service';
import { BookingService } from '../../_services/booking.service';
import { BookingStateService } from '../../_services/booking-state.service';
import { PaymentService } from '../../_services/payment.service';

interface ReviewService {
  id: number;
  salonServiceId: number;
  title: string;
  selectedType: string;
  selectedLength: string;
  imageUrl: string;
  altText: string;
  durationMinutes: number;
  duration: string;
  price: number;
  date: string;
  time: string;
  endTime: string;
}

interface ReviewClient {
  fullName: string;
  phoneNumber: string;
  emailAddress: string;
  preferredContactMethod: string;
}

@Component({
  selector: 'app-reviewbooking',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './reviewbooking.component.html',
  styleUrl: './reviewbooking.component.css'
})
export class ReviewbookingComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly accountService = inject(AccountService);
  private readonly bookingStateService = inject(BookingStateService);
  private readonly bookingService = inject(BookingService);
  private readonly paymentService = inject(PaymentService);

  services: ReviewService[] = [];
  bookingMode: 'combined' | 'separate' = 'combined';

  client: ReviewClient = {
    fullName: '',
    phoneNumber: '',
    emailAddress: '',
    preferredContactMethod: ''
  };

  bookingId: number | null = null;
  bookingReference = '';
  bookingStatus = '';
  paymentStatus = '';

  depositAmount = 0;
  totalPrice = 0;
  balanceRemaining = 0;
  totalDurationMinutes = 0;

  readonly paymentMethod = 'PayFast';

  isLoadingReview = false;
  isInitiatingPayment = false;
  reviewErrorMessage = '';
  paymentErrorMessage = '';

  ngOnInit(): void {
    this.loadReviewBookingDetails();
  }

  get hasReviewData(): boolean {
    return this.services.length > 0;
  }

  get formattedBookingMode(): string {
    return this.bookingMode === 'combined' ? 'Combined Session' : 'Separate Sessions';
  }

  get formattedDuration(): string {
    const hours = Math.floor(this.totalDurationMinutes / 60);
    const minutes = this.totalDurationMinutes % 60;

    if (hours === 0) {
      return `${minutes} min`;
    }

    if (minutes === 0) {
      return `${hours}h`;
    }

    return `${hours}h ${minutes}m`;
  }

  get bookingRouteMode(): string {
    return this.bookingMode;
  }

  get combinedSessionDate(): string {
    return this.services[0]?.date || 'No date selected';
  }

  get combinedSessionStartTime(): string {
    return this.services[0]?.time || 'No time selected';
  }

  get combinedSessionEndTime(): string {
    if (!this.services[0]?.time) {
      return 'No end time available';
    }

    return this.calculateEndTime(this.services[0].time, this.totalDurationMinutes);
  }

  initiatePayFastPayment(): void {
    if (!this.bookingId || this.isInitiatingPayment) {
      return;
    }

    this.isInitiatingPayment = true;
    this.paymentErrorMessage = '';

    this.paymentService.initiatePayFastPayment({ bookingId: this.bookingId }).subscribe({
      next: response => {
        this.isInitiatingPayment = false;
        this.submitPayFastForm(response.paymentUrl, response.formFields);
      },
      error: error => {
        console.error('Failed to initiate PayFast payment:', error);
        this.paymentErrorMessage = this.accountService.getErrorMessage(
          error,
          'Payment could not be started. Please try again.'
        );
        this.isInitiatingPayment = false;
      }
    });
  }

  private loadReviewBookingDetails(): void {
    const routeBookingId = Number(this.route.snapshot.queryParamMap.get('bookingId'));
    const savedBookingId = this.bookingStateService.getState()?.pendingBookingId;
    const bookingId = routeBookingId || savedBookingId;

    if (!bookingId) {
      this.reviewErrorMessage = 'No booking ID was found. Please go back and create your booking again.';
      this.services = [];
      return;
    }

    this.bookingId = bookingId;
    this.isLoadingReview = true;
    this.reviewErrorMessage = '';

    this.bookingService.getBookingReview(bookingId).subscribe({
      next: bookingReview => {
        this.applyBookingReview(bookingReview);
        this.isLoadingReview = false;
      },
      error: error => {
        console.error('Failed to load booking review:', error);
        this.reviewErrorMessage = this.accountService.getErrorMessage(
          error,
          'Booking review could not be loaded.'
        );
        this.services = [];
        this.isLoadingReview = false;
      }
    });
  }

  private applyBookingReview(bookingReview: BookingReview): void {
    this.bookingId = bookingReview.bookingId;
    this.bookingReference = bookingReview.bookingReference;
    this.bookingMode = bookingReview.bookingMode;
    this.bookingStatus = bookingReview.bookingStatus;
    this.paymentStatus = bookingReview.paymentStatus;

    this.client = {
      fullName: bookingReview.clientFullName,
      phoneNumber: bookingReview.clientPhoneNumber,
      emailAddress: bookingReview.clientEmailAddress,
      preferredContactMethod: this.formatPreferredContactMethod(
        bookingReview.preferredContactMethod
      )
    };

    this.depositAmount = bookingReview.depositAmount;
    this.totalPrice = bookingReview.totalAmount;
    this.balanceRemaining = bookingReview.balanceAmount;
    this.totalDurationMinutes = bookingReview.totalDurationMinutes;

    this.services = bookingReview.items.map(item => ({
      id: item.id,
      salonServiceId: item.salonServiceId,
      title: item.serviceName,
      selectedType: item.serviceTypeName,
      selectedLength: item.lengthName ?? '',
      imageUrl: item.referenceImageUrl || 'assets/home/nailcardcollage.svg',
      altText: `${item.serviceName} booking image`,
      durationMinutes: item.durationMinutes,
      duration: this.formatDuration(item.durationMinutes),
      price: item.finalPrice,
      date: this.formatSelectedDate(item.appointmentDate),
      time: this.formatTime(item.startTime),
      endTime: this.formatTime(item.endTime)
    }));
  }

  private submitPayFastForm(paymentUrl: string, formFields: Record<string, string>): void {
    const form = document.createElement('form');

    form.method = 'POST';
    form.action = paymentUrl;

    Object.entries(formFields).forEach(([key, value]) => {
      const input = document.createElement('input');

      input.type = 'hidden';
      input.name = key;
      input.value = value;

      form.appendChild(input);
    });

    document.body.appendChild(form);
    form.submit();
    document.body.removeChild(form);
  }

  private formatDuration(durationMinutes: number): string {
    if (durationMinutes <= 0) {
      return '0 min';
    }

    return `${durationMinutes} min`;
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

  private calculateEndTime(startTime: string, durationMinutes: number): string {
    const [hours, minutes] = startTime.split(':').map(Number);

    const startDate = new Date();

    startDate.setHours(hours, minutes, 0, 0);
    startDate.setMinutes(startDate.getMinutes() + durationMinutes);

    const endHours = startDate.getHours().toString().padStart(2, '0');
    const endMinutes = startDate.getMinutes().toString().padStart(2, '0');

    return `${endHours}:${endMinutes}`;
  }

  private formatPreferredContactMethod(method: string): string {
    if (method === 'whatsapp') {
      return 'WhatsApp';
    }

    if (method === 'phone') {
      return 'Phone Call';
    }

    if (method === 'email') {
      return 'Email';
    }

    return '';
  }

  private formatTime(time: string): string {
    if (!time) {
      return '';
    }

    return time.slice(0, 5);
  }
}