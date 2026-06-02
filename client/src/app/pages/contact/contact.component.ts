import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { ContactMessage } from '../../_models/contact';
import { ContactService } from '../../_services/contact.service';

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

interface ContactFormErrors {
  fullName?: string;
  emailAddress?: string;
  phoneNumber?: string;
  interest?: string;
  message?: string;
  api?: string;
}

interface FeedbackFormErrors {
  location?: string;
  reviewText?: string;
  rating?: string;
  image?: string;
  api?: string;
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

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.css'
})
export class ContactComponent implements OnInit {
  private readonly contactService = inject(ContactService);

  // ===============================
  // Contact information settings
  // Reused by the contact cards and action links.
  // ===============================

  private readonly salonPhoneNumber = '+27123456789';
  private readonly salonDisplayPhoneNumber = '(+27) 12 345 6789';
  private readonly salonEmailAddress = 'info@monailmakeup.com';
  private readonly salonAddress = 'The Big Hole, S Circular Road, Kimberley, 8300';
  private readonly whatsappDefaultMessage = 'Hi MO Nail and Makeup Artist, I would like to make an enquiry.';

  private readonly allowedReviewImageTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/webp'];
  private readonly maxReviewImageSizeInBytes = 3 * 1024 * 1024;

  // ===============================
  // Logged-in client details
  // The review form uses the signed-in user's name instead of asking again.
  // ===============================

  loggedInClientName = '';

  // ===============================
  // Contact information cards
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
  // Contact message form
  // Sends messages to the backend ContactMessages table.
  // ===============================

  contactForm = {
    fullName: '',
    emailAddress: '',
    phoneNumber: '',
    interest: '',
    message: ''
  };

  contactFormErrors: ContactFormErrors = {};
  isSubmittingContactMessage = false;
  isContactMessageSent = false;
  sentContactMessages: ContactMessage[] = [];

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
  // Feedback review form
  // Saves into the same ClientReviews table used by the Home page.
  // ===============================

  ratingStars = [1, 2, 3, 4, 5];

  feedbackForm = {
    location: '',
    reviewText: ''
  };

  selectedFeedbackRating = 0;
  selectedReviewImageFile?: File;
  selectedReviewImagePreviewUrl = '';

  feedbackFormErrors: FeedbackFormErrors = {};
  isSubmittingFeedback = false;
  isFeedbackSubmitted = false;

  ngOnInit(): void {
    this.prefillFormsFromStoredUser();
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
  // Contact message submit
  // ===============================

  submitContactForm(): void {
    this.contactFormErrors = this.validateContactForm();

    if (Object.keys(this.contactFormErrors).length > 0) {
      return;
    }

    this.isSubmittingContactMessage = true;

    this.contactService.submitContactMessage({
      fullName: this.contactForm.fullName.trim(),
      emailAddress: this.contactForm.emailAddress.trim(),
      phoneNumber: this.contactForm.phoneNumber.trim(),
      interest: this.contactForm.interest.trim(),
      message: this.contactForm.message.trim()
    }).subscribe({
      next: message => {
        this.sentContactMessages = [message, ...this.sentContactMessages];
        this.isContactMessageSent = true;
        this.isSubmittingContactMessage = false;
        this.resetContactMessageForm();
      },
      error: error => {
        this.contactFormErrors.api = this.getApiErrorMessage(
          error,
          'Your message could not be submitted. Please try again.'
        );

        this.isSubmittingContactMessage = false;
      }
    });
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
    } else if (this.contactForm.message.trim().length > 1000) {
      errors.message = 'Message cannot be longer than 1000 characters.';
    }

    return errors;
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
  // Feedback review submit
  // ===============================

  selectFeedbackRating(rating: number): void {
    this.selectedFeedbackRating = rating;
    this.feedbackFormErrors.rating = undefined;
  }

  onReviewImageSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    this.feedbackFormErrors.image = undefined;
    this.selectedReviewImageFile = undefined;
    this.selectedReviewImagePreviewUrl = '';

    if (!file) {
      return;
    }

    const fileIsAllowed = this.allowedReviewImageTypes.includes(file.type);
    const fileIsTooLarge = file.size > this.maxReviewImageSizeInBytes;

    if (!fileIsAllowed) {
      this.feedbackFormErrors.image = 'Please upload a JPG, PNG, or WEBP image.';
      input.value = '';
      return;
    }

    if (fileIsTooLarge) {
      this.feedbackFormErrors.image = 'Please upload an image smaller than 3MB.';
      input.value = '';
      return;
    }

    this.selectedReviewImageFile = file;
    this.loadReviewImagePreview(file);
  }

  submitFeedback(): void {
    this.feedbackFormErrors = this.validateFeedbackForm();

    if (Object.keys(this.feedbackFormErrors).length > 0) {
      return;
    }

    const formData = this.buildReviewFormData();

    this.isSubmittingFeedback = true;

    this.contactService.submitReview(formData).subscribe({
      next: () => {
        this.isFeedbackSubmitted = true;
        this.isSubmittingFeedback = false;
        this.resetFeedbackForm();
      },
      error: error => {
        this.feedbackFormErrors.api = this.getApiErrorMessage(
          error,
          'Your review could not be submitted. Please try again.'
        );

        this.isSubmittingFeedback = false;
      }
    });
  }

  private validateFeedbackForm(): FeedbackFormErrors {
    const errors: FeedbackFormErrors = {};

    if (!this.feedbackForm.location.trim()) {
      errors.location = 'Please enter your location.';
    }

    if (!this.feedbackForm.reviewText.trim()) {
      errors.reviewText = 'Please enter your review.';
    } else if (this.feedbackForm.reviewText.trim().length > 1000) {
      errors.reviewText = 'Review cannot be longer than 1000 characters.';
    }

    if (this.selectedFeedbackRating < 1 || this.selectedFeedbackRating > 5) {
      errors.rating = 'Please select a rating from 1 to 5.';
    }

    return errors;
  }

  private buildReviewFormData(): FormData {
    const formData = new FormData();

    formData.append('clientName', this.loggedInClientName);
    formData.append('location', this.feedbackForm.location.trim());
    formData.append('reviewText', this.feedbackForm.reviewText.trim());
    formData.append('rating', this.selectedFeedbackRating.toString());

    if (this.selectedReviewImageFile) {
      formData.append('image', this.selectedReviewImageFile);
    }

    return formData;
  }

  private resetFeedbackForm(): void {
    this.feedbackForm = {
      ...this.feedbackForm,
      reviewText: ''
    };

    this.selectedFeedbackRating = 0;
    this.selectedReviewImageFile = undefined;
    this.selectedReviewImagePreviewUrl = '';
    this.feedbackFormErrors = {};
  }

  private loadReviewImagePreview(file: File): void {
    const reader = new FileReader();

    reader.onload = () => {
      this.selectedReviewImagePreviewUrl = reader.result as string;
    };

    reader.readAsDataURL(file);
  }

  // ===============================
  // Stored user support
  // Uses the logged-in user's saved details to avoid recollecting them.
  // ===============================

  private prefillFormsFromStoredUser(): void {
    const storedUser = this.getStoredUser();

    if (!storedUser) {
      return;
    }

    const fullName = this.getStoredUserFullName(storedUser);
    const emailAddress = storedUser.emailAddress ?? storedUser.email ?? '';
    const phoneNumber = storedUser.phoneNumber ?? storedUser.phone ?? '';

    this.loggedInClientName = fullName;

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
  // Shared validation helpers
  // ===============================

  private isValidEmail(emailAddress: string): boolean {
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;

    return emailPattern.test(emailAddress.trim());
  }

  private isValidSouthAfricanPhoneNumber(phoneNumber: string): boolean {
    const cleanedPhoneNumber = phoneNumber.replace(/\s/g, '');
    const phonePattern = /^(\+27|0)[6-8][0-9]{8}$/;

    return phonePattern.test(cleanedPhoneNumber);
  }

  private getApiErrorMessage(error: any, fallbackMessage: string): string {
    if (error?.error?.message) {
      return error.error.message;
    }

    if (typeof error?.error === 'string') {
      return error.error;
    }

    return fallbackMessage;
  }
}