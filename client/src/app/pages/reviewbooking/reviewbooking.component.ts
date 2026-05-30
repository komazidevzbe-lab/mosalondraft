import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

interface ReviewService {
  title: string;
  selectedType: string;
  imageUrl: string;
  altText: string;
  duration: string;
  price: number;
  date: string;
  time: string;
}

@Component({
  selector: 'app-reviewbooking',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './reviewbooking.component.html',
  styleUrl: './reviewbooking.component.css'
})
export class ReviewbookingComponent {
  // ===============================
  // Review booking data
  // Later this can come from shared booking state or backend data.
  // ===============================

  services: ReviewService[] = [
    {
      title: 'Manicure',
      selectedType: 'Gel Manicure',
      imageUrl: 'assets/home/nailcardcollage.svg',
      altText: 'Manicure nail design',
      duration: '45 min',
      price: 250,
      date: '9 June 2025',
      time: '13:00'
    },
    {
      title: 'Pedicure',
      selectedType: 'Classic Pedicure',
      imageUrl: 'assets/home/pedicurecardcollage.svg',
      altText: 'Pedicure service',
      duration: '60 min',
      price: 300,
      date: '9 June 2025',
      time: '13:00'
    },
    {
      title: 'Brows & Lashes',
      selectedType: 'Brow Shape & Tint',
      imageUrl: 'assets/home/lashescardcollage.svg',
      altText: 'Brows and lashes service',
      duration: '45 min',
      price: 280,
      date: '9 June 2025',
      time: '13:00'
    }
  ];

  bookingMode = 'Combined Session';

  client = {
    fullName: 'Client Name',
    phoneNumber: '+27 82 123 4567',
    emailAddress: 'client@email.com',
    preferredContactMethod: 'WhatsApp',
    specialRequests: 'Soft, elegant finish. Please keep the look natural and polished.'
  };

  depositAmount = 250;

  get totalPrice(): number {
    return this.services.reduce((total, service) => total + service.price, 0);
  }

  get balanceRemaining(): number {
    return this.totalPrice - this.depositAmount;
  }

  get totalDurationMinutes(): number {
    return 150;
  }

  get formattedDuration(): string {
    return '2h 30m';
  }
}