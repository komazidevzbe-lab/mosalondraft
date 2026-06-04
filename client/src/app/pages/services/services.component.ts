import { CommonModule } from '@angular/common';
import { Component, DestroyRef, DoCheck, OnInit, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import {
  BookingPreference,
  BookingStateService,
  ServiceLength
} from '../../_services/booking-state.service';

interface ServiceOption {
  id: string;
  title: string;
  description: string;
  imageUrl: string;
  altText: string;
  duration: string;
  price: number;
  serviceTypes: string[];
  selected: boolean;
  requiresLength?: boolean;
}

interface SelectedServiceDetail {
  serviceId: string;
  selectedType: string;
  selectedLength: ServiceLength | '';
  inspoNote: string;
  inspoPreviewUrl: string;
}

interface BookingFlowStep {
  stepNumber: number;
  title: string;
  description: string;
}

@Component({
  selector: 'app-services',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './services.component.html',
  styleUrl: './services.component.css'
})
export class ServicesComponent implements OnInit, DoCheck {
  private readonly bookingStateService = inject(BookingStateService);
  private readonly destroyRef = inject(DestroyRef);
  private lastSavedState = '';

  services: ServiceOption[] = [
    {
      id: 'manicure',
      title: 'Manicure',
      description: 'Nail shaping, cuticle care, polish and hand finishing.',
      imageUrl: 'assets/home/nailcardcollage.svg',
      altText: 'Manicure nail design',
      duration: '45 min',
      price: 400,
      serviceTypes: ['Gel Manicure', 'Acrylic Set', 'Classic Manicure', 'Nail Art Add-on'],
      selected: false,
      requiresLength: true
    },
    {
      id: 'pedicure',
      title: 'Pedicure',
      description: 'Foot soak, nail care, exfoliation and polish finish.',
      imageUrl: 'assets/home/pedicurecardcollage.svg',
      altText: 'Pedicure service',
      duration: '60 min',
      price: 300,
      serviceTypes: ['Classic Pedicure', 'Spa Pedicure', 'Gel Pedicure', 'French Pedicure'],
      selected: false
    },
    {
      id: 'makeup',
      title: 'Makeup',
      description: 'Flawless makeup for any occasion or photoshoot.',
      imageUrl: 'assets/home/makeupcardcollage.svg',
      altText: 'Makeup service',
      duration: '60 min',
      price: 450,
      serviceTypes: ['Soft Glam', 'Full Glam', 'Natural Makeup', 'Event Makeup'],
      selected: false
    },
    {
      id: 'lashes',
      title: 'Brows & Lashes',
      description: 'Brow shaping, tinting and lash extensions.',
      imageUrl: 'assets/home/lashescardcollage.svg',
      altText: 'Brows and lashes service',
      duration: '45 min',
      price: 280,
      serviceTypes: ['Brow Shape & Tint', 'Classic Lash Extensions', 'Hybrid Lashes', 'Volume Lashes'],
      selected: false,
      requiresLength: true
    }
  ];

  bookingFlowSteps: BookingFlowStep[] = [
    {
      stepNumber: 1,
      title: 'Services Selected',
      description: 'Choose your services'
    },
    {
      stepNumber: 2,
      title: 'Book Session',
      description: 'Choose date and time'
    },
    {
      stepNumber: 3,
      title: 'Review & Pay',
      description: 'Check and pay deposit'
    },
    {
      stepNumber: 4,
      title: 'Confirmation',
      description: 'View your receipt'
    }
  ];

  readonly currentBookingFlowStep = 1;

  readonly lengthOptions: ServiceLength[] = ['Short', 'Medium', 'Long', 'Extra Long'];

  readonly lengthPriceAddOns: Record<ServiceLength, number> = {
    Short: 0,
    Medium: 60,
    Long: 120,
    'Extra Long': 180
  };

  selectedServiceDetails: SelectedServiceDetail[] = [];

  bookingPreference: BookingPreference = null;

  ngOnInit(): void {
    this.restoreSavedBookingState();

    this.bookingStateService.resetBookingFlow$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => {
        this.resetServicesPage();
      });
  }

  ngDoCheck(): void {
    this.saveBookingState();
  }

  get selectedServices(): ServiceOption[] {
    return this.services.filter(service => service.selected);
  }

  get totalEstimatedPrice(): number {
    return this.selectedServices.reduce((total, service) => {
      return total + this.getServiceEstimatedPrice(service);
    }, 0);
  }

  get totalSelectedServices(): number {
    return this.selectedServices.length;
  }

  get hasSelectedServices(): boolean {
    return this.selectedServices.length > 0;
  }

  get hasSingleSelectedService(): boolean {
    return this.totalSelectedServices === 1;
  }

  get hasMultipleSelectedServices(): boolean {
    return this.totalSelectedServices > 1;
  }

  get activeBookingPreference(): BookingPreference {
    if (this.hasSingleSelectedService) {
      return 'combined';
    }

    return this.bookingPreference;
  }

  get hasBookingPreference(): boolean {
    if (!this.areServiceDetailsComplete) {
      return false;
    }

    if (this.hasSingleSelectedService) {
      return true;
    }

    return this.bookingPreference !== null;
  }

  get areServiceDetailsComplete(): boolean {
    if (!this.hasSelectedServices) {
      return false;
    }

    return this.selectedServices.every(service => {
      const detail = this.getServiceDetail(service.id);
      const hasSelectedType = detail.selectedType.trim().length > 0;
      const hasSelectedLength = !this.requiresLength(service) || detail.selectedLength.trim().length > 0;

      return hasSelectedType && hasSelectedLength;
    });
  }

  toggleService(service: ServiceOption): void {
    service.selected = !service.selected;

    if (service.selected) {
      this.addServiceDetail(service);
      this.syncBookingPreferenceWithSelectedServices();
      this.saveBookingState();
      return;
    }

    this.removeServiceDetail(service.id);
    this.syncBookingPreferenceWithSelectedServices();
    this.saveBookingState();
  }

  addServiceDetail(service: ServiceOption): void {
    const detailAlreadyExists = this.selectedServiceDetails.some(
      detail => detail.serviceId === service.id
    );

    if (detailAlreadyExists) {
      return;
    }

    this.selectedServiceDetails.push({
      serviceId: service.id,
      selectedType: '',
      selectedLength: '',
      inspoNote: '',
      inspoPreviewUrl: ''
    });
  }

  removeServiceDetail(serviceId: string): void {
    this.selectedServiceDetails = this.selectedServiceDetails.filter(
      detail => detail.serviceId !== serviceId
    );
  }

  getServiceDetail(serviceId: string): SelectedServiceDetail {
    let detail = this.selectedServiceDetails.find(
      selectedDetail => selectedDetail.serviceId === serviceId
    );

    if (!detail) {
      detail = {
        serviceId,
        selectedType: '',
        selectedLength: '',
        inspoNote: '',
        inspoPreviewUrl: ''
      };

      this.selectedServiceDetails.push(detail);
    }

    return detail;
  }

  requiresLength(service: ServiceOption): boolean {
    return service.requiresLength === true;
  }

  getLengthAddOnPrice(service: ServiceOption): number {
    const detail = this.getServiceDetail(service.id);

    if (!this.requiresLength(service) || !detail.selectedLength) {
      return 0;
    }

    return this.lengthPriceAddOns[detail.selectedLength];
  }

  getServiceEstimatedPrice(service: ServiceOption): number {
    return service.price + this.getLengthAddOnPrice(service);
  }

  isBookingFlowStepCompleted(stepNumber: number): boolean {
    return stepNumber < this.currentBookingFlowStep;
  }

  isBookingFlowStepActive(stepNumber: number): boolean {
    return stepNumber === this.currentBookingFlowStep;
  }

  chooseBookingPreference(preference: BookingPreference): void {
    if (this.hasSingleSelectedService && preference === 'separate') {
      return;
    }

    this.bookingPreference = preference;
    this.saveBookingState();
  }

  onInspoImageSelected(event: Event, serviceId: string): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file) {
      return;
    }

    const reader = new FileReader();

    reader.onload = () => {
      const detail = this.getServiceDetail(serviceId);
      detail.inspoPreviewUrl = reader.result as string;
      this.saveBookingState();
    };

    reader.readAsDataURL(file);
  }

  saveBookingState(): void {
    const selectedServices = this.selectedServices.map(service => {
      const detail = this.getServiceDetail(service.id);

      return {
        id: service.id,
        title: service.title,
        description: service.description,
        imageUrl: service.imageUrl,
        altText: service.altText,
        duration: service.duration,
        price: this.getServiceEstimatedPrice(service),
        selectedType: detail.selectedType,
        selectedLength: detail.selectedLength,
        inspoNote: detail.inspoNote,
        inspoPreviewUrl: detail.inspoPreviewUrl,
        requiresLength: service.requiresLength
      };
    });

    const bookingState = {
      bookingPreference: this.activeBookingPreference,
      selectedServiceDetails: this.selectedServiceDetails,
      selectedServices
    };

    const nextSavedState = JSON.stringify(bookingState);

    if (nextSavedState === this.lastSavedState) {
      return;
    }

    this.lastSavedState = nextSavedState;
    this.bookingStateService.saveState(bookingState);
  }

  private restoreSavedBookingState(): void {
    const savedState = this.bookingStateService.getState();

    if (!savedState) {
      return;
    }

    this.bookingPreference = savedState.bookingPreference;
    this.selectedServiceDetails = savedState.selectedServiceDetails ?? [];

    this.services = this.services.map(service => {
      const savedService = savedState.selectedServices.find(
        selectedService => selectedService.id === service.id
      );

      return {
        ...service,
        selected: !!savedService
      };
    });

    this.selectedServices.forEach(service => this.addServiceDetail(service));
  }

  private resetServicesPage(): void {
    this.services = this.services.map(service => ({
      ...service,
      selected: false
    }));

    this.selectedServiceDetails = [];
    this.bookingPreference = null;
    this.lastSavedState = '';

    this.saveBookingState();
  }

  private syncBookingPreferenceWithSelectedServices(): void {
    if (this.hasSingleSelectedService) {
      this.bookingPreference = 'combined';
      return;
    }

    if (!this.hasSelectedServices) {
      this.bookingPreference = null;
    }
  }
}