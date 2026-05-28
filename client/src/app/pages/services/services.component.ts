import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

type BookingPreference = 'combined' | 'separate' | null;

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
}

interface SelectedServiceDetail {
  serviceId: string;
  selectedType: string;
  inspoNote: string;
  inspoPreviewUrl: string;
}

@Component({
  selector: 'app-services',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './services.component.html',
  styleUrl: './services.component.css'
})
export class ServicesComponent {
  // ===============================
  // Service selection cards
  // These use the same category images from the home page.
  // ===============================

  services: ServiceOption[] = [
    {
      id: 'manicure',
      title: 'Manicure',
      description: 'Nail shaping, cuticle care, polish and hand finishing.',
      imageUrl: 'assets/home/nailcardcollage.svg',
      altText: 'Manicure nail design',
      duration: '45 min',
      price: 250,
      serviceTypes: ['Gel Manicure', 'Acrylic Set', 'Classic Manicure', 'Nail Art Add-on'],
      selected: false
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
      price: 550,
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
      selected: false
    }
  ];

  // ===============================
  // Selected service details
  // Each selected service gets its own service type, inspo upload and note.
  // ===============================

  selectedServiceDetails: SelectedServiceDetail[] = [];

  // ===============================
  // Booking preference
  // User decides whether selected services are booked together or separately.
  // ===============================

  bookingPreference: BookingPreference = null;

  // ===============================
  // Selection helpers
  // ===============================

  get selectedServices(): ServiceOption[] {
    return this.services.filter(service => service.selected);
  }

  get totalEstimatedPrice(): number {
    return this.selectedServices.reduce((total, service) => total + service.price, 0);
  }

  get totalSelectedServices(): number {
    return this.selectedServices.length;
  }

  get hasSelectedServices(): boolean {
    return this.selectedServices.length > 0;
  }

  get areServiceDetailsComplete(): boolean {
    if (!this.hasSelectedServices) {
      return false;
    }

    return this.selectedServices.every(service => {
      const detail = this.getServiceDetail(service.id);
      return detail.selectedType.trim().length > 0;
    });
  }

  get hasBookingPreference(): boolean {
    return this.bookingPreference !== null;
  }

  toggleService(service: ServiceOption): void {
    service.selected = !service.selected;

    if (service.selected) {
      this.addServiceDetail(service);
      return;
    }

    this.removeServiceDetail(service.id);

    if (this.selectedServices.length <= 1) {
      this.bookingPreference = null;
    }
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
    return this.selectedServiceDetails.find(
      detail => detail.serviceId === serviceId
    ) as SelectedServiceDetail;
  }

  chooseBookingPreference(preference: BookingPreference): void {
    this.bookingPreference = preference;
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
    };

    reader.readAsDataURL(file);
  }
}