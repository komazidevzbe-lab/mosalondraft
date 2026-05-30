import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

interface ConfirmedService {
  title: string;
  selectedType: string;
  date: string;
  time: string;
}

@Component({
  selector: 'app-confirmation',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './confirmation.component.html',
  styleUrl: './confirmation.component.css'
})
export class ConfirmationComponent {
  // ===============================
  // Confirmation data
  // Later this can come from backend payment and booking records.
  // ===============================

  bookingReference = 'MO-2025-0001';
  depositPaid = 250;
  totalAmount = 830;

  services: ConfirmedService[] = [
    {
      title: 'Manicure',
      selectedType: 'Gel Manicure',
      date: '9 June 2025',
      time: '13:00'
    },
    {
      title: 'Pedicure',
      selectedType: 'Classic Pedicure',
      date: '9 June 2025',
      time: '13:00'
    },
    {
      title: 'Brows & Lashes',
      selectedType: 'Brow Shape & Tint',
      date: '9 June 2025',
      time: '13:00'
    }
  ];

  client = {
    fullName: 'Client Name',
    phoneNumber: '+27 82 123 4567',
    emailAddress: 'client@email.com'
  };

  get balanceRemaining(): number {
    return this.totalAmount - this.depositPaid;
  }
}