import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

type ContactActionType = 'call' | 'email' | 'whatsapp' | 'copyAddress';

interface ContactInfoCard {
  title: string;
  description: string;
  value: string;
  iconClass: string;
  actionType: ContactActionType;
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

interface ContactFormErrors {
  fullName?: string;
  emailAddress?: string;
  phoneNumber?: string;
  interest?: string;
  message?: string;
}

interface StoredUser {
  firstName?: string;
  lastName?: string;
  name?: string;
  surname?: string;
  fullName?: string;
  email?: string;
  emailAddress?: string;
  phoneNumber?: string;
  phone?: string;
}

interface SentContactMessage {
  id: number;
  fullName: string;
  emailAddress: string;
  phoneNumber: string;
  interest: string;
  message: string;
  submittedAt: Date;
  statuses: string[];
}

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.css'
})
export class ContactComponent implements OnInit {
  // ===============================
  // Contact information settings
  // These values are reused by the contact cards and action links.
  // ===============================

  private readonly salonPhoneNumber = '+27123456789';
  private readonly salonDisplayPhoneNumber = '(+27) 12 345 6789';
  private readonly salonEmailAddress = 'info@monailmakeup.com';
  private readonly salonAddress = 'The Big Hole, S Circular Road, Kimberley, 8300';
  private readonly whatsappDefaultMessage = 'Hi MO Nail and Makeup Artist, I would like to make an enquiry.';

  // ===============================
  // Contact information cards
  // These cards appear at the top of the contact page.
  // ===============================

  contactCards: ContactInfoCard[] = [
    {
      title: 'Call Us',
      description: 'Speak with our team during business hours.',
      value: this.salonDisplayPhoneNumber,
      iconClass: 'fa-solid fa-phone',
      actionType: 'call'
    },
    {
      title: 'Email Us',
      description: 'Send an email and we’ll get back to you.',
      value: this.salonEmailAddress,
      iconClass: 'fa-solid fa-envelope',
      actionType: 'email'
    },
    {
      title: 'WhatsApp',
      description: 'Quick replies for bookings, questions & more.',
      value: this.salonDisplayPhoneNumber,
      iconClass: 'fa-brands fa-whatsapp',
      actionType: 'whatsapp'
    },
    {
      title: 'Visit Us',
      description: 'Come say hi! We’d love to welcome you.',
      value: this.salonAddress,
      iconClass: 'fa-solid fa-location-dot',
      actionType: 'copyAddress'
    }
  ];

  copiedAddressMessage = '';

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

  contactFormErrors: ContactFormErrors = {};
  isContactMessageSent = false;
  sentContactMessages: SentContactMessage[] = [];

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

  ngOnInit(): void {
    this.prefillContactFormFromStoredUser();
  }

  // ===============================
  // Contact card actions
  // ===============================

  handleContactCardAction(card: ContactInfoCard): void {
    if (card.actionType === 'call') {
      window.location.href = `tel:${this.salonPhoneNumber}`;
      return;
    }

    if (card.actionType === 'email') {
      window.location.href = `mailto:${this.salonEmailAddress}`;
      return;
    }

    if (card.actionType === 'whatsapp') {
      const encodedMessage = encodeURIComponent(this.whatsappDefaultMessage);
      window.open(`https://wa.me/${this.salonPhoneNumber.replace('+', '')}?text=${encodedMessage}`, '_blank');
      return;
    }

    this.copyAddressToClipboard();
  }

  private copyAddressToClipboard(): void {
    if (!navigator.clipboard) {
      this.copiedAddressMessage = 'Address copied';
      return;
    }

    navigator.clipboard.writeText(this.salonAddress).then(() => {
      this.copiedAddressMessage = 'Address copied';

      setTimeout(() => {
        this.copiedAddressMessage = '';
      }, 2500);
    });
  }

  // ===============================
  // Contact form validation and submit
  // ===============================

  submitContactForm(): void {
    this.contactFormErrors = this.validateContactForm();

    if (Object.keys(this.contactFormErrors).length > 0) {
      return;
    }

    this.sentContactMessages = [
      ...this.sentContactMessages,
      {
        id: Date.now(),
        fullName: this.contactForm.fullName.trim(),
        emailAddress: this.contactForm.emailAddress.trim(),
        phoneNumber: this.contactForm.phoneNumber.trim(),
        interest: this.contactForm.interest.trim(),
        message: this.contactForm.message.trim(),
        submittedAt: new Date(),
        statuses: ['Sent', 'Seen', 'Response pending']
      }
    ];

    this.isContactMessageSent = true;
    this.resetContactMessageForm();
  }

  sendAnotherMessage(): void {
    this.isContactMessageSent = false;
    this.contactFormErrors = {};
  }

  private validateContactForm(): ContactFormErrors {
    const errors: ContactFormErrors = {};

    if (!this.contactForm.fullName.trim()) {
      errors.fullName = 'Please enter your full name.';
    }

    if (!this.contactForm.emailAddress.trim()) {
      errors.emailAddress = 'Please enter your email address.';
    } else if (!this.isValidEmail(this.contactForm.emailAddress)) {
      errors.emailAddress = 'Please enter a valid email address.';
    }

    if (!this.contactForm.phoneNumber.trim()) {
      errors.phoneNumber = 'Please enter your phone number.';
    } else if (!this.isValidSouthAfricanPhoneNumber(this.contactForm.phoneNumber)) {
      errors.phoneNumber = 'Please enter a valid South African phone number.';
    }

    if (!this.contactForm.interest.trim()) {
      errors.interest = 'Please select what you are interested in.';
    }

    if (!this.contactForm.message.trim()) {
      errors.message = 'Please enter your message.';
    }

    return errors;
  }

  private isValidEmail(emailAddress: string): boolean {
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;
    return emailPattern.test(emailAddress.trim());
  }

  private isValidSouthAfricanPhoneNumber(phoneNumber: string): boolean {
    const cleanedPhoneNumber = phoneNumber.replace(/\s/g, '');
    const phonePattern = /^(\+27|0)[6-8][0-9]{8}$/;

    return phonePattern.test(cleanedPhoneNumber);
  }

  private resetContactMessageForm(): void {
    const existingUserDetails = {
      fullName: this.contactForm.fullName,
      emailAddress: this.contactForm.emailAddress,
      phoneNumber: this.contactForm.phoneNumber
    };

    this.contactForm = {
      ...existingUserDetails,
      interest: '',
      message: ''
    };

    this.contactFormErrors = {};
  }

  // ===============================
  // Logged-in user autofill support
  // This is frontend-ready for now and can be improved once backend user profile data is connected.
  // ===============================

  private prefillContactFormFromStoredUser(): void {
    const storedUser = this.getStoredUser();

    if (!storedUser) {
      return;
    }

    const fullName = this.getStoredUserFullName(storedUser);
    const emailAddress = storedUser.emailAddress ?? storedUser.email ?? '';
    const phoneNumber = storedUser.phoneNumber ?? storedUser.phone ?? '';

    this.contactForm = {
      ...this.contactForm,
      fullName,
      emailAddress,
      phoneNumber
    };
  }

  private getStoredUser(): StoredUser | null {
    const possibleStorageKeys = ['user', 'currentUser', 'loggedInUser'];

    for (const key of possibleStorageKeys) {
      const storedValue = localStorage.getItem(key);

      if (!storedValue) {
        continue;
      }

      try {
        return JSON.parse(storedValue) as StoredUser;
      } catch {
        return null;
      }
    }

    return null;
  }

  private getStoredUserFullName(user: StoredUser): string {
    if (user.fullName) {
      return user.fullName;
    }

    const firstName = user.firstName ?? user.name ?? '';
    const lastName = user.lastName ?? user.surname ?? '';

    return `${firstName} ${lastName}`.trim();
  }

  // ===============================
  // Feedback form
  // Frontend-only for now. Backend/review integration can be connected later.
  // ===============================

  selectFeedbackRating(rating: number): void {
    this.selectedFeedbackRating = rating;
  }

  submitFeedback(): void {
    // Backend/review integration can be connected here later.
  }
}