import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { ContactMessage } from '../../_models/contact';
import { User } from '../../_models/user';
import { AccountService } from '../../_services/account.service';
import { ContactService } from '../../_services/contact.service';

type ContactActionType = 'call' | 'email' | 'whatsapp' | 'copyAddress';
type MessageViewMode = 'form' | 'history';

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
  history?: string;
}

interface FeedbackFormErrors {
  location?: string;
  reviewText?: string;
  rating?: string;
  image?: string;
  api?: string;
}

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.css'
})
export class ContactComponent implements OnInit {
  private readonly accountService = inject(AccountService);
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
  // Messages and reviews use the signed-in user's account details.
  // ===============================

  loggedInUser?: User;
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
  // Contact message form and history
  // The identity fields stay visible but are readonly.
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
  isLoadingMessages = false;
  isContactMessageSent = false;
  messageViewMode: MessageViewMode = 'form';
  sentContactMessages: ContactMessage[] = [];
  expandedResponseMessageIds: number[] = [];

  interestOptions = [
    'Manicure',
    'Pedicure',
    'Makeup',
    'Brows & Lashes',
    'Booking Question',
    'Collaboration'
  ];

  get messageHeadingTitle(): string {
    return this.messageViewMode === 'form' ? 'Send Us A Message' : 'Message History';
  }

  get messageHeadingDescription(): string {
    return this.messageViewMode === 'form'
      ? 'Fill out the form below and we’ll get back to you as soon as possible.'
      : 'Track the messages you have sent to our team.';
  }

  get messageActionIconClass(): string {
    return this.messageViewMode === 'form' ? 'fa-regular fa-bell' : 'fa-solid fa-plus';
  }

  get messageActionLabel(): string {
    return this.messageViewMode === 'form' ? 'View message history' : 'Create new message';
  }

  get responseNotificationCount(): number {
    return this.sentContactMessages.filter(message =>
      message.messageStatus === 'Responded' || !!message.adminResponse
    ).length;
  }

  get hasMessageNotifications(): boolean {
    return this.responseNotificationCount > 0;
  }

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
    this.loadLoggedInUser();
    this.loadMyMessages();
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
  // Message view controls
  // ===============================

  handleMessageActionButtonClick(): void {
    if (this.messageViewMode === 'form') {
      this.showMessageHistory();
      return;
    }

    this.showMessageForm();
  }

  showMessageForm(): void {
    this.messageViewMode = 'form';
    this.isContactMessageSent = false;
    this.contactFormErrors.api = undefined;
  }

  showMessageHistory(): void {
    this.messageViewMode = 'history';
    this.isContactMessageSent = false;
    this.loadMyMessages();
  }

  toggleMessageResponse(messageId: number): void {
    if (this.isMessageResponseExpanded(messageId)) {
      this.expandedResponseMessageIds = this.expandedResponseMessageIds.filter(id => id !== messageId);
      return;
    }

    this.expandedResponseMessageIds = [...this.expandedResponseMessageIds, messageId];
  }

  isMessageResponseExpanded(messageId: number): boolean {
    return this.expandedResponseMessageIds.includes(messageId);
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
      interest: this.contactForm.interest.trim(),
      message: this.contactForm.message.trim()
    }).subscribe({
      next: message => {
        this.sentContactMessages = [message, ...this.sentContactMessages];
        this.isContactMessageSent = true;
        this.messageViewMode = 'history';
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

  private loadMyMessages(): void {
    this.isLoadingMessages = true;
    this.contactFormErrors.history = undefined;

    this.contactService.getMyMessages().subscribe({
      next: messages => {
        this.sentContactMessages = messages;
        this.isLoadingMessages = false;
      },
      error: error => {
        this.contactFormErrors.history = this.getApiErrorMessage(
          error,
          'Your message history could not be loaded.'
        );

        this.isLoadingMessages = false;
      }
    });
  }

  private validateContactForm(): ContactFormErrors {
    const errors: ContactFormErrors = {};

    if (!this.contactForm.fullName.trim()) {
      errors.fullName = 'Your account name could not be loaded.';
    }

    if (!this.contactForm.emailAddress.trim()) {
      errors.emailAddress = 'Your account email could not be loaded.';
    }

    if (!this.contactForm.phoneNumber.trim()) {
      errors.phoneNumber = 'Your account phone number could not be loaded.';
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
    this.contactForm = {
      fullName: this.contactForm.fullName,
      emailAddress: this.contactForm.emailAddress,
      phoneNumber: this.contactForm.phoneNumber,
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
      location: this.feedbackForm.location,
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
  // Logged-in user support
  // ===============================

  private loadLoggedInUser(): void {
    const currentUser = this.accountService.currentUser() ?? this.getStoredUser();

    if (!currentUser) {
      return;
    }

    this.loggedInUser = currentUser;
    this.loggedInClientName = currentUser.fullName || `${currentUser.firstName} ${currentUser.lastName}`.trim();

    this.contactForm = {
      ...this.contactForm,
      fullName: this.loggedInClientName,
      emailAddress: currentUser.email,
      phoneNumber: currentUser.phoneNumber
    };
  }

  private getStoredUser(): User | null {
    const storedUser = sessionStorage.getItem('user');

    if (!storedUser) {
      return null;
    }

    try {
      return JSON.parse(storedUser) as User;
    } catch {
      return null;
    }
  }

  // ===============================
  // Shared helpers
  // ===============================

  getStatusIconClass(status: string): string {
    if (status === 'Seen') {
      return 'fa-eye';
    }

    if (status === 'Response pending') {
      return 'fa-clock';
    }

    if (status === 'Responded') {
      return 'fa-reply';
    }

    return 'fa-check';
  }

  private getApiErrorMessage(error: any, fallbackMessage: string): string {
    if (error?.error?.message) {
      return error.error.message;
    }

    if (typeof error?.error === 'string') {
      return error.error;
    }

    if (error?.error?.errors) {
      const validationErrors = error.error.errors;
      const firstKey = Object.keys(validationErrors)[0];

      if (firstKey && Array.isArray(validationErrors[firstKey])) {
        return validationErrors[firstKey][0];
      }
    }

    return fallbackMessage;
  }
}