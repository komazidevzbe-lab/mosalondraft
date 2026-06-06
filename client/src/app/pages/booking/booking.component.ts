import { CommonModule } from '@angular/common';
import { Component, DoCheck, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import {
  BookingMode,
  CreateBooking,
  PreferredContactMethod
} from '../../_models/booking';
import { AccountService } from '../../_services/account.service';
import { BookingService as BookingApiService } from '../../_services/booking.service';
import {
  BookingState,
  BookingStateService,
  SavedServiceSelection
} from '../../_services/booking-state.service';

interface BookingPageService {
  id: string;
  salonServiceId: number;
  title: string;
  selectedType: string;
  selectedServiceTypeId: number | null;
  selectedLength: string;
  selectedLengthOptionId: number | null;
  inspoNote: string;
  inspoPreviewUrl: string;
  favoriteGalleryImageId: number | null;
  imageUrl: string;
  altText: string;
  durationMinutes: number;
  price: number;
  selectedDate: string;
  selectedTime: string;
  unavailableTimeSlots: string[];
}

interface CalendarDay {
  date: Date;
  isoDate: string;
  dayNumber: number;
  isCurrentMonth: boolean;
  isPast: boolean;
  isUnavailable: boolean;
}

interface ClientDetails {
  fullName: string;
  phoneNumber: string;
  emailAddress: string;
  preferredContactMethod: PreferredContactMethod;
}

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './booking.component.html',
  styleUrl: './booking.component.css'
})
export class BookingComponent implements OnInit, DoCheck {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly accountService = inject(AccountService);
  private readonly bookingStateService = inject(BookingStateService);
  private readonly bookingApiService = inject(BookingApiService);

  private lastSavedBookingDetails = '';

  bookingMode: BookingMode = 'combined';
  selectedServices: BookingPageService[] = [];
  calendarDays: CalendarDay[] = [];

  readonly timeSlots = [
    '09:00',
    '10:00',
    '11:00',
    '12:00',
    '13:00',
    '14:00',
    '15:00',
    '16:00',
    '17:00'
  ];

  currentCalendarDate = new Date();
  selectedCombinedDate = '';
  selectedCombinedTime = '13:00';
  unavailableCombinedTimeSlots: string[] = [];

  clientDetails: ClientDetails = {
    fullName: '',
    phoneNumber: '',
    emailAddress: '',
    preferredContactMethod: ''
  };

  isCreatingBooking = false;
  bookingErrorMessage = '';

  ngOnInit(): void {
    this.currentCalendarDate.setDate(1);
    this.currentCalendarDate.setHours(0, 0, 0, 0);

    const selectedMode = this.route.snapshot.queryParamMap.get('mode');

    if (selectedMode === 'combined' || selectedMode === 'separate') {
      this.bookingMode = selectedMode;
    }

    this.loadSelectedServices();
    this.generateCalendarDays();
    this.setDefaultSelectedDates();
    this.autofillClientDetails();
    this.restoreSavedBookingDetails();

    if (selectedMode === 'combined' || selectedMode === 'separate') {
      this.bookingMode = selectedMode;
    }

    this.saveBookingDetails();
  }

  ngDoCheck(): void {
    this.saveBookingDetails();
  }

  get hasSelectedServices(): boolean {
    return this.selectedServices.length > 0;
  }

  get currentMonthLabel(): string {
    return this.currentCalendarDate.toLocaleDateString('en-ZA', {
      month: 'long',
      year: 'numeric'
    });
  }

  get isPreviousMonthDisabled(): boolean {
    const today = this.getToday();

    const previousMonth = new Date(
      this.currentCalendarDate.getFullYear(),
      this.currentCalendarDate.getMonth() - 1,
      1
    );

    const lastDayOfPreviousMonth = new Date(
      previousMonth.getFullYear(),
      previousMonth.getMonth() + 1,
      0
    );

    return lastDayOfPreviousMonth < today;
  }

  get totalAppointmentDuration(): number {
    return this.selectedServices.reduce(
      (total, service) => total + service.durationMinutes,
      0
    );
  }

  get totalAppointmentPrice(): number {
    return this.selectedServices.reduce(
      (total, service) => total + service.price,
      0
    );
  }

  get formattedTotalDuration(): string {
    const hours = Math.floor(this.totalAppointmentDuration / 60);
    const minutes = this.totalAppointmentDuration % 60;

    if (hours === 0) {
      return `${minutes} min`;
    }

    if (minutes === 0) {
      return `${hours}h`;
    }

    return `${hours}h ${minutes}m`;
  }

  get combinedEndTime(): string {
    return this.calculateEndTime(this.selectedCombinedTime, this.totalAppointmentDuration);
  }

  get bookingIntroText(): string {
    if (this.bookingMode === 'combined') {
      return 'Choose one date and time for all your selected services.';
    }

    return 'Choose a date and time for each selected service.';
  }

  get canReviewAndPay(): boolean {
    if (!this.hasSelectedServices) {
      return false;
    }

    if (!this.clientDetails.preferredContactMethod) {
      return false;
    }

    if (this.bookingMode === 'combined') {
      return !!this.selectedCombinedDate && !!this.selectedCombinedTime;
    }

    return this.selectedServices.every(service => service.selectedDate && service.selectedTime);
  }

  setBookingMode(mode: BookingMode): void {
    this.bookingMode = mode;
    this.saveBookingDetails();
  }

  goToPreviousMonth(): void {
    if (this.isPreviousMonthDisabled) {
      return;
    }

    this.currentCalendarDate = new Date(
      this.currentCalendarDate.getFullYear(),
      this.currentCalendarDate.getMonth() - 1,
      1
    );

    this.generateCalendarDays();
  }

  goToNextMonth(): void {
    this.currentCalendarDate = new Date(
      this.currentCalendarDate.getFullYear(),
      this.currentCalendarDate.getMonth() + 1,
      1
    );

    this.generateCalendarDays();
  }

  selectCombinedDate(day: CalendarDay): void {
    if (day.isPast || day.isUnavailable) {
      return;
    }

    this.selectedCombinedDate = day.isoDate;
    this.saveBookingDetails();
  }

  selectCombinedTime(time: string): void {
    if (this.isCombinedTimeUnavailable(time)) {
      return;
    }

    this.selectedCombinedTime = time;
    this.saveBookingDetails();
  }

  selectServiceDate(service: BookingPageService, day: CalendarDay): void {
    if (day.isPast || day.isUnavailable) {
      return;
    }

    service.selectedDate = day.isoDate;
    this.saveBookingDetails();
  }

  selectServiceTime(service: BookingPageService, time: string): void {
    if (this.isServiceTimeUnavailable(service, time)) {
      return;
    }

    service.selectedTime = time;
    this.saveBookingDetails();
  }

  isCombinedTimeUnavailable(time: string): boolean {
    return this.unavailableCombinedTimeSlots.includes(time);
  }

  isServiceTimeUnavailable(service: BookingPageService, time: string): boolean {
    return service.unavailableTimeSlots.includes(time);
  }

  calculateEndTime(startTime: string, durationMinutes: number): string {
    const [hours, minutes] = startTime.split(':').map(Number);

    const startDate = new Date();

    startDate.setHours(hours, minutes, 0, 0);
    startDate.setMinutes(startDate.getMinutes() + durationMinutes);

    const endHours = startDate.getHours().toString().padStart(2, '0');
    const endMinutes = startDate.getMinutes().toString().padStart(2, '0');

    return `${endHours}:${endMinutes}`;
  }

  formatSelectedDate(isoDate: string): string {
    if (!isoDate) {
      return 'No date selected';
    }

    return new Date(`${isoDate}T00:00:00`).toLocaleDateString('en-ZA', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  createPendingBookingAndReview(): void {
    if (!this.canReviewAndPay || this.isCreatingBooking) {
      return;
    }

    const bookingRequest = this.buildCreateBookingRequest();

    if (!bookingRequest) {
      return;
    }

    this.isCreatingBooking = true;
    this.bookingErrorMessage = '';

    this.bookingApiService.createPendingBooking(bookingRequest).subscribe({
      next: bookingReview => {
        this.bookingStateService.savePendingBookingId(bookingReview.bookingId);
        this.isCreatingBooking = false;

        this.router.navigate(['/review-booking'], {
          queryParams: {
            bookingId: bookingReview.bookingId,
            mode: this.bookingMode
          }
        });
      },
      error: error => {
        console.error('Failed to create pending booking:', error);

        this.bookingErrorMessage = this.accountService.getErrorMessage(
          error,
          'Your booking could not be created. Please check your details and try again.'
        );

        this.isCreatingBooking = false;
      }
    });
  }

  private loadSelectedServices(): void {
    const savedState = this.bookingStateService.getState();

    if (!savedState || savedState.selectedServices.length === 0) {
      this.selectedServices = [];
      return;
    }

    this.selectedServices = savedState.selectedServices.map(service => {
      return this.mapSavedServiceToBookingService(service);
    });

    if (savedState.bookingPreference === 'combined' || savedState.bookingPreference === 'separate') {
      this.bookingMode = savedState.bookingPreference;
    }
  }

  private mapSavedServiceToBookingService(service: SavedServiceSelection): BookingPageService {
    const displayImageUrl = service.inspoPreviewUrl || service.imageUrl;

    return {
      id: service.id,
      salonServiceId: service.salonServiceId,
      title: service.title,
      selectedType: service.selectedType,
      selectedServiceTypeId: service.selectedServiceTypeId,
      selectedLength: service.selectedLength,
      selectedLengthOptionId: service.selectedLengthOptionId,
      inspoNote: service.inspoNote,
      inspoPreviewUrl: service.inspoPreviewUrl,
      favoriteGalleryImageId: service.favoriteGalleryImageId,
      imageUrl: displayImageUrl,
      altText: service.inspoPreviewUrl
        ? `${service.title} selected inspiration image`
        : service.altText,
      durationMinutes: service.durationMinutes || this.parseDurationMinutes(service.duration),
      price: service.price,
      selectedDate: '',
      selectedTime: '13:00',
      unavailableTimeSlots: []
    };
  }

  private parseDurationMinutes(duration: string): number {
    const durationNumber = Number(duration.replace(/\D/g, ''));

    if (Number.isNaN(durationNumber) || durationNumber <= 0) {
      return 0;
    }

    return durationNumber;
  }

  private generateCalendarDays(): void {
    const today = this.getToday();
    const year = this.currentCalendarDate.getFullYear();
    const month = this.currentCalendarDate.getMonth();

    const firstDayOfMonth = new Date(year, month, 1);
    const firstDayWeekIndex = (firstDayOfMonth.getDay() + 6) % 7;
    const calendarStartDate = new Date(year, month, 1 - firstDayWeekIndex);

    this.calendarDays = Array.from({ length: 35 }, (_, index) => {
      const date = new Date(calendarStartDate);

      date.setDate(calendarStartDate.getDate() + index);
      date.setHours(0, 0, 0, 0);

      return {
        date,
        isoDate: this.toIsoDate(date),
        dayNumber: date.getDate(),
        isCurrentMonth: date.getMonth() === month,
        isPast: date < today,
        isUnavailable: this.isDateUnavailable(date)
      };
    });
  }

  private setDefaultSelectedDates(): void {
    const firstAvailableDate = this.calendarDays.find(
      day => day.isCurrentMonth && !day.isPast && !day.isUnavailable
    );

    if (!firstAvailableDate) {
      return;
    }

    if (!this.selectedCombinedDate) {
      this.selectedCombinedDate = firstAvailableDate.isoDate;
    }

    this.selectedServices = this.selectedServices.map(service => ({
      ...service,
      selectedDate: service.selectedDate || firstAvailableDate.isoDate
    }));
  }

  private restoreSavedBookingDetails(): void {
    const savedState = this.bookingStateService.getState();

    if (!savedState) {
      return;
    }

    if (savedState.appointmentDetails) {
      this.bookingMode = savedState.appointmentDetails.bookingMode;
      this.selectedCombinedDate =
        savedState.appointmentDetails.selectedCombinedDate || this.selectedCombinedDate;
      this.selectedCombinedTime =
        savedState.appointmentDetails.selectedCombinedTime || this.selectedCombinedTime;

      this.selectedServices = this.selectedServices.map(service => {
        const savedServiceAppointment = savedState.appointmentDetails?.services.find(
          savedService => savedService.serviceId === service.id
        );

        return {
          ...service,
          selectedDate: savedServiceAppointment?.selectedDate || service.selectedDate,
          selectedTime: savedServiceAppointment?.selectedTime || service.selectedTime
        };
      });
    }

    if (savedState.clientDetails) {
      this.clientDetails = {
        ...this.clientDetails,
        ...savedState.clientDetails
      };
    }
  }

  private saveBookingDetails(): void {
    const existingState = this.bookingStateService.getState();

    if (!existingState) {
      return;
    }

    const nextBookingDetails: BookingState = {
      bookingPreference: this.bookingMode,
      selectedServiceDetails: existingState.selectedServiceDetails,
      selectedServices: existingState.selectedServices,
      appointmentDetails: {
        bookingMode: this.bookingMode,
        selectedCombinedDate: this.selectedCombinedDate,
        selectedCombinedTime: this.selectedCombinedTime,
        services: this.selectedServices.map(service => ({
          serviceId: service.id,
          selectedDate: service.selectedDate,
          selectedTime: service.selectedTime
        }))
      },
      clientDetails: {
        fullName: this.clientDetails.fullName,
        phoneNumber: this.clientDetails.phoneNumber,
        emailAddress: this.clientDetails.emailAddress,
        preferredContactMethod: this.clientDetails.preferredContactMethod
      },
      pendingBookingId: existingState.pendingBookingId
    };

    const nextSavedBookingDetails = JSON.stringify(nextBookingDetails);

    if (nextSavedBookingDetails === this.lastSavedBookingDetails) {
      return;
    }

    this.lastSavedBookingDetails = nextSavedBookingDetails;
    this.bookingStateService.saveState(nextBookingDetails);
  }

  private buildCreateBookingRequest(): CreateBooking | null {
    const invalidService = this.selectedServices.find(
      service => !service.selectedServiceTypeId
    );

    if (invalidService) {
      this.bookingErrorMessage = `Please choose a valid service type for ${invalidService.title}.`;
      return null;
    }

    const invalidLengthService = this.selectedServices.find(
      service => service.selectedLength && !service.selectedLengthOptionId
    );

    if (invalidLengthService) {
      this.bookingErrorMessage = `Please choose a valid length for ${invalidLengthService.title}.`;
      return null;
    }

    return {
      bookingMode: this.bookingMode,
      clientFullName: this.clientDetails.fullName,
      clientEmailAddress: this.clientDetails.emailAddress,
      clientPhoneNumber: this.clientDetails.phoneNumber,
      preferredContactMethod: this.clientDetails.preferredContactMethod,
      items: this.selectedServices.map(service => ({
        salonServiceId: service.salonServiceId,
        salonServiceTypeId: service.selectedServiceTypeId as number,
        salonServiceLengthOptionId: service.selectedLengthOptionId,
        appointmentDate:
          this.bookingMode === 'combined'
            ? this.selectedCombinedDate
            : service.selectedDate,
        startTime:
          this.bookingMode === 'combined'
            ? this.selectedCombinedTime
            : service.selectedTime,
        notes: service.inspoNote || null,
        galleryImageId: service.favoriteGalleryImageId,
        uploadedReferenceImageUrl: null
      }))
    };
  }

  private isDateUnavailable(date: Date): boolean {
    return false;
  }

  private getToday(): Date {
    const today = new Date();

    today.setHours(0, 0, 0, 0);

    return today;
  }

  private toIsoDate(date: Date): string {
    const year = date.getFullYear();
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const day = `${date.getDate()}`.padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private autofillClientDetails(): void {
    const user = this.accountService.currentUser();

    if (!user) {
      return;
    }

    this.clientDetails = {
      ...this.clientDetails,
      fullName: user.fullName || `${user.firstName ?? ''} ${user.lastName ?? ''}`.trim(),
      emailAddress: user.email ?? '',
      phoneNumber: user.phoneNumber ?? ''
    };
  }
}