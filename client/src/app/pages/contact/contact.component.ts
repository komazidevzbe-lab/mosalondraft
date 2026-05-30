import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

interface ContactInfoCard {
  title: string;
  description: string;
  value: string;
  iconClass: string;
}

interface OpeningHour {
  day: string;
  time: string;
  isClosed?: boolean;
}

interface FeedbackReview {
  clientName: string;
  reviewText: string;
  imageUrl: string;
  rating: number;
}

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.css'
})
export class ContactComponent {
  // ===============================
  // Contact information cards
  // These cards appear at the top of the contact page.
  // ===============================

  contactCards: ContactInfoCard[] = [
    {
      title: 'Call Us',
      description: 'Speak with our team during business hours.',
      value: '(+27) 12 345 6789',
      iconClass: 'fa-solid fa-phone'
    },
    {
      title: 'Email Us',
      description: 'Send an email and we’ll get back to you.',
      value: 'info@monailmakeup.com',
      iconClass: 'fa-solid fa-envelope'
    },
    {
      title: 'WhatsApp',
      description: 'Quick replies for bookings, questions & more.',
      value: '(+27) 12 345 6789',
      iconClass: 'fa-brands fa-whatsapp'
    },
    {
      title: 'Visit Us',
      description: 'Come say hi! We’d love to welcome you.',
      value: 'The Big Hole, S Circular Road, Kimberley, 8300',
      iconClass: 'fa-solid fa-location-dot'
    }
  ];

  // ===============================
  // Contact form
  // Frontend-only for now. Backend/email sending can be connected later.
  // ===============================

  contactForm = {
    fullName: '',
    emailAddress: '',
    phoneNumber: '',
    interest: '',
    message: ''
  };

  interestOptions = [
    'Manicure',
    'Pedicure',
    'Makeup',
    'Brows & Lashes',
    'Booking Question',
    'Collaboration'
  ];

  // ===============================
  // Opening hours
  // ===============================

  openingHours: OpeningHour[] = [
    { day: 'Monday', time: '09:00 - 18:00' },
    { day: 'Tuesday', time: '09:00 - 18:00' },
    { day: 'Wednesday', time: '09:00 - 18:00' },
    { day: 'Thursday', time: '09:00 - 18:00' },
    { day: 'Friday', time: '09:00 - 18:00' },
    { day: 'Saturday', time: '09:00 - 16:00' },
    { day: 'Sunday', time: 'Closed', isClosed: true }
  ];

  // ===============================
  // Feedback section
  // ===============================

  feedbackReviews: FeedbackReview[] = [
    {
      clientName: 'Lerato M.',
      reviewText: 'The best experience every time! The team is so professional and the results are always flawless.',
      imageUrl: 'assets/contact/reviewer.png',
      rating: 5
    }
  ];

  ratingStars = [1, 2, 3, 4, 5];

  selectedFeedbackRating = 0;
  feedbackMessage = '';

  selectFeedbackRating(rating: number): void {
    this.selectedFeedbackRating = rating;
  }

  submitContactForm(): void {
    // Backend/email integration can be connected here later.
  }

  submitFeedback(): void {
    // Backend/review integration can be connected here later.
  }
}