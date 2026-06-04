import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

export type BookingPreference = 'combined' | 'separate' | null;
export type ServiceLength = 'Short' | 'Medium' | 'Long' | 'Extra Long';

export interface SavedSelectedServiceDetail {
  serviceId: string;
  selectedType: string;
  selectedLength: ServiceLength | '';
  inspoNote: string;
  inspoPreviewUrl: string;
}

export interface SavedServiceSelection {
  id: string;
  title: string;
  description: string;
  imageUrl: string;
  altText: string;
  duration: string;
  price: number;
  selectedType: string;
  selectedLength: ServiceLength | '';
  inspoNote: string;
  inspoPreviewUrl: string;
  requiresLength?: boolean;
}

export interface SavedBookingAppointment {
  bookingMode: 'combined' | 'separate';
  selectedCombinedDate: string;
  selectedCombinedTime: string;
  services: {
    serviceId: string;
    selectedDate: string;
    selectedTime: string;
  }[];
}

export interface SavedClientDetails {
  fullName: string;
  phoneNumber: string;
  emailAddress: string;
  preferredContactMethod: string;
}

export interface SavedBookingState {
  bookingPreference: BookingPreference;
  selectedServiceDetails: SavedSelectedServiceDetail[];
  selectedServices: SavedServiceSelection[];
  appointmentDetails?: SavedBookingAppointment;
  clientDetails?: SavedClientDetails;
}

@Injectable({
  providedIn: 'root'
})
export class BookingStateService {
  private readonly storageKey = 'moBookingState';
  private readonly resetBookingFlowSubject = new Subject<void>();

  readonly resetBookingFlow$ = this.resetBookingFlowSubject.asObservable();

  saveState(state: SavedBookingState): void {
    const existingState = this.getState();

    const mergedState: SavedBookingState = {
      ...existingState,
      ...state,
      selectedServiceDetails: state.selectedServiceDetails,
      selectedServices: state.selectedServices
    };

    localStorage.setItem(this.storageKey, JSON.stringify(mergedState));
  }

  getState(): SavedBookingState | null {
    const savedState = localStorage.getItem(this.storageKey);

    if (!savedState) {
      return null;
    }

    try {
      return JSON.parse(savedState) as SavedBookingState;
    } catch {
      this.clearState();
      return null;
    }
  }

  clearState(): void {
    localStorage.removeItem(this.storageKey);
  }

  resetBookingFlow(): void {
    this.clearState();
    this.resetBookingFlowSubject.next();
  }
}