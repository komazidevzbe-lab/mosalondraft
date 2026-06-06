import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

import { BookingMode, PreferredContactMethod } from '../_models/booking';

export type ServiceLength = 'Short' | 'Medium' | 'Long' | 'Extra Long';

export type BookingPreference = BookingMode | null;

export interface SavedServiceSelection {
  id: string;
  salonServiceId: number;
  title: string;
  description: string;
  imageUrl: string;
  altText: string;
  duration: string;
  durationMinutes: number;
  price: number;
  basePrice: number;
  selectedType: string;
  selectedServiceTypeId: number | null;
  selectedLength: ServiceLength | '';
  selectedLengthOptionId: number | null;
  lengthAddOnPrice: number;
  inspoNote: string;
  inspoPreviewUrl: string;
  favoriteGalleryImageId: number | null;
  requiresLength?: boolean;
}

export interface SavedServiceDetail {
  serviceId: string;
  selectedType: string;
  selectedServiceTypeId: number | null;
  selectedLength: ServiceLength | '';
  selectedLengthOptionId: number | null;
  inspoNote: string;
  inspoPreviewUrl: string;
  favoriteGalleryImageId: number | null;
}

export interface SavedAppointmentService {
  serviceId: string;
  selectedDate: string;
  selectedTime: string;
}

export interface SavedAppointmentDetails {
  bookingMode: BookingMode;
  selectedCombinedDate: string;
  selectedCombinedTime: string;
  services: SavedAppointmentService[];
}

export interface SavedClientDetails {
  fullName: string;
  phoneNumber: string;
  emailAddress: string;
  preferredContactMethod: PreferredContactMethod;
}

export interface BookingState {
  bookingPreference: BookingPreference;
  selectedServiceDetails: SavedServiceDetail[];
  selectedServices: SavedServiceSelection[];
  appointmentDetails?: SavedAppointmentDetails;
  clientDetails?: SavedClientDetails;
  pendingBookingId?: number;
}

@Injectable({
  providedIn: 'root'
})
export class BookingStateService {
  private readonly storageKey = 'moSalonBookingState';

  private readonly resetBookingFlowSubject = new Subject<void>();

  resetBookingFlow$ = this.resetBookingFlowSubject.asObservable();

  saveState(state: BookingState): void {
    sessionStorage.setItem(this.storageKey, JSON.stringify(state));
  }

  getState(): BookingState | null {
    const stateJson = sessionStorage.getItem(this.storageKey);

    if (!stateJson) {
      return null;
    }

    try {
      return JSON.parse(stateJson) as BookingState;
    } catch (error) {
      console.error('Failed to parse booking state:', error);
      this.clearState();
      return null;
    }
  }

  savePendingBookingId(bookingId: number): void {
    const currentState = this.getState();

    if (!currentState) {
      return;
    }

    this.saveState({
      ...currentState,
      pendingBookingId: bookingId
    });
  }

  clearState(): void {
    sessionStorage.removeItem(this.storageKey);
  }

  resetBookingFlow(): void {
    this.clearState();
    this.resetBookingFlowSubject.next();
  }
}