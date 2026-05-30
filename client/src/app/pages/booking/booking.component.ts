import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';

type BookingMode = 'combined' | 'separate';

interface BookingService {
  id: string;
  title: string;
  selectedType: string;
  imageUrl: string;
  altText: string;
  durationMinutes: number;
  price: number;
  selectedDate: number;
  selectedTime: string;
}

interface ClientDetails {
  fullName: string;
  phoneNumber: string;
  emailAddress: string;
  preferredContactMethod: string;
  specialRequests: string;
}

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './booking.component.html',
  styleUrl: './booking.component.css'
})
export class BookingComponent implements OnInit {
  private route = inject(ActivatedRoute);

  // ===============================
  // Booking mode
  // Combined means one shared appointment time.
  // Separate means each service can have its own time.
  // The mode is received from the services page using the URL query parameter.
  // Example: /booking?mode=separate
  // ===============================

  bookingMode: BookingMode = 'combined';

  // ===============================
  // Selected services
  // Later this can come from shared booking state or backend data.
  // ===============================

  selectedServices: BookingService[] = [
    {
      id: 'manicure',
      title: 'Manicure',
      selectedType: 'Gel Manicure',
      imageUrl: 'assets/home/nailcardcollage.svg',
      altText: 'Manicure nail design',
      durationMinutes: 45,
      price: 250,
      selectedDate: 9,
      selectedTime: '13:00'
    },
    {
      id: 'pedicure',
      title: 'Pedicure',
      selectedType: 'Classic Pedicure',
      imageUrl: 'assets/home/pedicurecardcollage.svg',
      altText: 'Pedicure service',
      durationMinutes: 60,
      price: 300,
      selectedDate: 9,
      selectedTime: '13:00'
    },
    {
      id: 'lashes',
      title: 'Brows & Lashes',
      selectedType: 'Brow Shape & Tint',
      imageUrl: 'assets/home/lashescardcollage.svg',
      altText: 'Brows and lashes service',
      durationMinutes: 45,
      price: 280,
      selectedDate: 9,
      selectedTime: '13:00'
    }
  ];

  // ===============================
  // Calendar and time options
  // These are temporary frontend options until backend availability is added.
  // ===============================

  calendarDays = [
    26, 27, 28, 29, 30, 31, 1,
    2, 3, 4, 5, 6, 7, 8,
    9, 10, 11, 12, 13, 14, 15,
    16, 17, 18, 19, 20, 21, 22,
    23, 24, 25, 26, 27, 28, 29,
    30
  ];

  timeSlots = [
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

  selectedCombinedDate = 9;
  selectedCombinedTime = '13:00';

  clientDetails: ClientDetails = {
    fullName: '',
    phoneNumber: '',
    emailAddress: '',
    preferredContactMethod: '',
    specialRequests: ''
  };

  // ===============================
  // Page setup
  // Reads the chosen session type from the services page.
  // ===============================

  ngOnInit(): void {
    const selectedMode = this.route.snapshot.queryParamMap.get('mode');

    if (selectedMode === 'combined' || selectedMode === 'separate') {
      this.bookingMode = selectedMode;
    }
  }

  // ===============================
  // Booking helpers
  // ===============================

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

  setBookingMode(mode: BookingMode): void {
    this.bookingMode = mode;
  }

  selectCombinedDate(day: number): void {
    this.selectedCombinedDate = day;
  }

  selectCombinedTime(time: string): void {
    this.selectedCombinedTime = time;
  }

  selectServiceDate(service: BookingService, day: number): void {
    service.selectedDate = day;
  }

  selectServiceTime(service: BookingService, time: string): void {
    service.selectedTime = time;
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
}