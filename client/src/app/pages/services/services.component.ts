import { CommonModule } from '@angular/common';
import { Component, DestroyRef, DoCheck, OnInit, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { GalleryDatabaseCategory, GalleryImage } from '../../_models/gallery-image';
import { SalonService, SalonServiceLengthOption, SalonServiceTypeOption } from '../../_models/salon-service';
import {
  BookingPreference,
  BookingStateService,
  SavedServiceDetail,
  ServiceLength
} from '../../_services/booking-state.service';
import { GalleryService } from '../../_services/gallery.service';
import { SalonServiceService } from '../../_services/salon-service.service';

interface ServiceOption {
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
  serviceTypes: string[];
  serviceTypeOptions: SalonServiceTypeOption[];
  lengthOptions: SalonServiceLengthOption[];
  selected: boolean;
  requiresLength?: boolean;
}

interface SelectedServiceDetail {
  serviceId: string;
  selectedType: string;
  selectedServiceTypeId: number | null;
  selectedLength: ServiceLength | '';
  selectedLengthOptionId: number | null;
  inspoNote: string;
  inspoPreviewUrl: string;
  favoriteGalleryImageId: number | null;
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
  private readonly galleryService = inject(GalleryService);
  private readonly salonServiceService = inject(SalonServiceService);
  private readonly destroyRef = inject(DestroyRef);

  private lastSavedState = '';

  services: ServiceOption[] = [];

  bookingFlowSteps: BookingFlowStep[] = [
    { stepNumber: 1, title: 'Services Selected', description: 'Choose your services' },
    { stepNumber: 2, title: 'Book Session', description: 'Choose date and time' },
    { stepNumber: 3, title: 'Review & Pay', description: 'Check and pay deposit' },
    { stepNumber: 4, title: 'Confirmation', description: 'View your receipt' }
  ];

  readonly currentBookingFlowStep = 1;

  selectedServiceDetails: SelectedServiceDetail[] = [];
  bookingPreference: BookingPreference = null;

  favoriteGalleryImages: GalleryImage[] = [];
  isLoadingFavoriteImages = false;
  favoriteImagesErrorMessage = '';

  isLoadingServices = false;
  servicesErrorMessage = '';

  activeFavoritePickerServiceId = '';

  ngOnInit(): void {
    this.loadServices();
    this.loadFavoriteGalleryImages();

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

  get activeFavoritePickerService(): ServiceOption | undefined {
    return this.services.find(service => service.id === this.activeFavoritePickerServiceId);
  }

  get activeFavoritePickerCategoryLabel(): string {
    const service = this.activeFavoritePickerService;

    if (!service) {
      return '';
    }

    return service.title;
  }

  get activeFavoriteImages(): GalleryImage[] {
    if (!this.activeFavoritePickerServiceId) {
      return [];
    }

    return this.getFavoriteImagesForService(this.activeFavoritePickerServiceId);
  }

  get hasActiveFavoriteImages(): boolean {
    return this.activeFavoriteImages.length > 0;
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
      selectedServiceTypeId: null,
      selectedLength: '',
      selectedLengthOptionId: null,
      inspoNote: '',
      inspoPreviewUrl: '',
      favoriteGalleryImageId: null
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
        selectedServiceTypeId: null,
        selectedLength: '',
        selectedLengthOptionId: null,
        inspoNote: '',
        inspoPreviewUrl: '',
        favoriteGalleryImageId: null
      };

      this.selectedServiceDetails.push(detail);
    }

    return detail;
  }

  getServiceDisplayImage(service: ServiceOption): string {
    const detail = this.getServiceDetail(service.id);

    return detail.inspoPreviewUrl || service.imageUrl;
  }

  getServiceDisplayAltText(service: ServiceOption): string {
    const detail = this.getServiceDetail(service.id);

    if (detail.inspoPreviewUrl) {
      return `${service.title} selected inspiration image`;
    }

    return service.altText;
  }

  requiresLength(service: ServiceOption): boolean {
    return service.requiresLength === true;
  }

  getLengthOptionsForService(service: ServiceOption): ServiceLength[] {
    return service.lengthOptions.map(lengthOption => lengthOption.name as ServiceLength);
  }

  getLengthAddOnPrice(service: ServiceOption): number {
    const detail = this.getServiceDetail(service.id);

    if (!this.requiresLength(service) || !detail.selectedLength) {
      return 0;
    }

    const selectedLength = service.lengthOptions.find(
      lengthOption => lengthOption.name === detail.selectedLength
    );

    return selectedLength?.priceAddOn ?? 0;
  }

  getServiceEstimatedPrice(service: ServiceOption): number {
    return service.basePrice + this.getLengthAddOnPrice(service);
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

  onServiceTypeChange(service: ServiceOption): void {
    const detail = this.getServiceDetail(service.id);

    const selectedType = service.serviceTypeOptions.find(
      type => type.name === detail.selectedType
    );

    detail.selectedServiceTypeId = selectedType?.id ?? null;

    this.saveBookingState();
  }

  onLengthChange(service: ServiceOption): void {
    const detail = this.getServiceDetail(service.id);

    const selectedLength = service.lengthOptions.find(
      length => length.name === detail.selectedLength
    );

    detail.selectedLengthOptionId = selectedLength?.id ?? null;

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
      detail.favoriteGalleryImageId = null;

      this.saveBookingState();

      input.value = '';
    };

    reader.readAsDataURL(file);
  }

  openFavoritePicker(serviceId: string): void {
    this.activeFavoritePickerServiceId = serviceId;
  }

  closeFavoritePicker(): void {
    this.activeFavoritePickerServiceId = '';
  }

  selectFavoriteInspoImage(galleryImage: GalleryImage): void {
    if (!this.activeFavoritePickerServiceId) {
      return;
    }

    const detail = this.getServiceDetail(this.activeFavoritePickerServiceId);

    detail.inspoPreviewUrl = galleryImage.imageUrl;
    detail.favoriteGalleryImageId = galleryImage.id;

    this.saveBookingState();
    this.closeFavoritePicker();
  }

  clearInspoImage(serviceId: string): void {
    const detail = this.getServiceDetail(serviceId);

    detail.inspoPreviewUrl = '';
    detail.favoriteGalleryImageId = null;

    this.saveBookingState();
  }

  getFavoriteImagesForService(serviceId: string): GalleryImage[] {
    const category = this.getGalleryCategoryForService(serviceId);

    return this.favoriteGalleryImages.filter(image => image.category === category);
  }

  saveBookingState(): void {
    const selectedServices = this.selectedServices.map(service => {
      const detail = this.getServiceDetail(service.id);
      const displayImageUrl = detail.inspoPreviewUrl || service.imageUrl;

      return {
        id: service.id,
        salonServiceId: service.salonServiceId,
        title: service.title,
        description: service.description,
        imageUrl: displayImageUrl,
        altText: detail.inspoPreviewUrl
          ? `${service.title} selected inspiration image`
          : service.altText,
        duration: service.duration,
        durationMinutes: service.durationMinutes,
        price: this.getServiceEstimatedPrice(service),
        basePrice: service.basePrice,
        selectedType: detail.selectedType,
        selectedServiceTypeId: detail.selectedServiceTypeId,
        selectedLength: detail.selectedLength,
        selectedLengthOptionId: detail.selectedLengthOptionId,
        lengthAddOnPrice: this.getLengthAddOnPrice(service),
        inspoNote: detail.inspoNote,
        inspoPreviewUrl: detail.inspoPreviewUrl,
        favoriteGalleryImageId: detail.favoriteGalleryImageId,
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

  private loadServices(): void {
    this.isLoadingServices = true;
    this.servicesErrorMessage = '';

    this.salonServiceService
      .getActiveServices()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: services => {
          this.services = services.map(service => this.mapSalonServiceToServiceOption(service));
          this.restoreSavedBookingState();
          this.isLoadingServices = false;
        },
        error: error => {
          console.error('Failed to load salon services:', error);
          this.services = [];
          this.servicesErrorMessage = 'Services could not be loaded right now.';
          this.isLoadingServices = false;
        }
      });
  }

  private loadFavoriteGalleryImages(): void {
    this.isLoadingFavoriteImages = true;
    this.favoriteImagesErrorMessage = '';

    this.galleryService
      .getFavoriteGalleryImages()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: favoriteImages => {
          this.favoriteGalleryImages = favoriteImages.filter(image => image.isFavorite);
          this.isLoadingFavoriteImages = false;
        },
        error: error => {
          console.error('Failed to load favorite gallery images:', error);
          this.favoriteGalleryImages = [];
          this.favoriteImagesErrorMessage = 'Your favourite gallery images could not be loaded right now.';
          this.isLoadingFavoriteImages = false;
        }
      });
  }

  private restoreSavedBookingState(): void {
    const savedState = this.bookingStateService.getState();

    if (!savedState) {
      return;
    }

    this.bookingPreference = savedState.bookingPreference;
    this.selectedServiceDetails = this.mapSavedServiceDetails(savedState.selectedServiceDetails ?? []);

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

  private getGalleryCategoryForService(serviceId: string): GalleryDatabaseCategory {
    const serviceCategoryMap: Record<string, GalleryDatabaseCategory> = {
      manicure: 'manicure',
      pedicure: 'pedicure',
      makeup: 'makeup',
      lashes: 'lashes'
    };

    return serviceCategoryMap[serviceId] ?? 'manicure';
  }

  private mapSalonServiceToServiceOption(service: SalonService): ServiceOption {
    return {
      id: service.slug,
      salonServiceId: service.id,
      title: service.title,
      description: service.description,
      imageUrl: service.imageUrl,
      altText: service.altText,
      duration: `${service.durationMinutes} min`,
      durationMinutes: service.durationMinutes,
      price: service.basePrice,
      basePrice: service.basePrice,
      serviceTypes: service.serviceTypes.map(type => type.name),
      serviceTypeOptions: service.serviceTypes,
      lengthOptions: service.lengthOptions,
      selected: false,
      requiresLength: service.requiresLength
    };
  }

  private mapSavedServiceDetails(savedDetails: SavedServiceDetail[]): SelectedServiceDetail[] {
    return savedDetails.map(detail => ({
      serviceId: detail.serviceId,
      selectedType: detail.selectedType,
      selectedServiceTypeId: detail.selectedServiceTypeId ?? null,
      selectedLength: detail.selectedLength,
      selectedLengthOptionId: detail.selectedLengthOptionId ?? null,
      inspoNote: detail.inspoNote,
      inspoPreviewUrl: detail.inspoPreviewUrl,
      favoriteGalleryImageId: detail.favoriteGalleryImageId ?? null
    }));
  }
}