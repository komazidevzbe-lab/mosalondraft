import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';

import {
  BookingStateService,
  SavedServiceSelection
} from '../../_services/booking-state.service';

interface ReviewService {
  id: string;
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
  private readonly bookingStateService = inject(BookingStateService);

  services: ReviewService[] = [];

  bookingMode: 'combined' | 'separate' = 'combined';

  client: ReviewClient = {
    fullName: '',
    phoneNumber: '',
    emailAddress: '',
    preferredContactMethod: ''
  };

  readonly depositAmount = 250;
  readonly paymentMethod = 'PayFast';

  ngOnInit(): void {
    this.loadReviewBookingDetails();
  }

  get hasReviewData(): boolean {
    return this.services.length > 0;
  }

  get formattedBookingMode(): string {
    return this.bookingMode === 'combined' ? 'Combined Session' : 'Separate Sessions';
  }

  get totalPrice(): number {
    return this.services.reduce((total, service) => total + service.price, 0);
  }

  get balanceRemaining(): number {
    return Math.max(this.totalPrice - this.depositAmount, 0);
  }

  get totalDurationMinutes(): number {
    return this.services.reduce((total, service) => total + service.durationMinutes, 0);
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

  private loadReviewBookingDetails(): void {
    const savedState = this.bookingStateService.getState();

    if (!savedState || savedState.selectedServices.length === 0) {
      this.services = [];
      return;
    }

    if (savedState.appointmentDetails) {
      this.bookingMode = savedState.appointmentDetails.bookingMode;
    } else if (savedState.bookingPreference === 'combined' || savedState.bookingPreference === 'separate') {
      this.bookingMode = savedState.bookingPreference;
    }

    this.services = savedState.selectedServices.map(service => {
      return this.mapSavedServiceToReviewService(service);
    });

    if (savedState.clientDetails) {
      this.client = {
        fullName: savedState.clientDetails.fullName,
        phoneNumber: savedState.clientDetails.phoneNumber,
        emailAddress: savedState.clientDetails.emailAddress,
        preferredContactMethod: this.formatPreferredContactMethod(
          savedState.clientDetails.preferredContactMethod
        )
      };
    }
  }

  private mapSavedServiceToReviewService(service: SavedServiceSelection): ReviewService {
    const savedState = this.bookingStateService.getState();
    const appointmentDetails = savedState?.appointmentDetails;

    const savedServiceAppointment = appointmentDetails?.services.find(
      savedService => savedService.serviceId === service.id
    );

    const appointmentDate = this.bookingMode === 'combined'
      ? appointmentDetails?.selectedCombinedDate
      : savedServiceAppointment?.selectedDate;

    const appointmentTime = this.bookingMode === 'combined'
      ? appointmentDetails?.selectedCombinedTime
      : savedServiceAppointment?.selectedTime;

    const durationMinutes = this.parseDurationMinutes(service.duration);
    const time = appointmentTime || '';

    return {
      id: service.id,
      title: service.title,
      selectedType: service.selectedType,
      selectedLength: service.selectedLength,
      imageUrl: service.imageUrl,
      altText: service.altText,
      durationMinutes,
      duration: this.formatDuration(durationMinutes),
      price: service.price,
      date: this.formatSelectedDate(appointmentDate || ''),
      time,
      endTime: time ? this.calculateEndTime(time, durationMinutes) : ''
    };
  }

  private parseDurationMinutes(duration: string): number {
    const durationNumber = Number(duration.replace(/\D/g, ''));

    if (Number.isNaN(durationNumber) || durationNumber <= 0) {
      return 0;
    }

    return durationNumber;
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
}