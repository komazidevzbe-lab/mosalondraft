export type BookingMode = 'combined' | 'separate';

export type PreferredContactMethod = 'whatsapp' | 'phone' | 'email' | '';

export interface CreateBookingItem {
  salonServiceId: number;
  salonServiceTypeId: number;
  salonServiceLengthOptionId: number | null;
  appointmentDate: string;
  startTime: string;
  notes?: string | null;
  galleryImageId?: number | null;
  uploadedReferenceImageUrl?: string | null;
}

export interface CreateBooking {
  bookingMode: BookingMode;
  clientFullName: string;
  clientEmailAddress: string;
  clientPhoneNumber: string;
  preferredContactMethod: PreferredContactMethod;
  items: CreateBookingItem[];
}

export interface BookingReviewItem {
  id: number;
  salonServiceId: number;
  serviceName: string;
  serviceTypeName: string;
  lengthName?: string | null;
  appointmentDate: string;
  startTime: string;
  endTime: string;
  durationMinutes: number;
  basePrice: number;
  lengthAddOnPrice: number;
  finalPrice: number;
  notes?: string | null;
  referenceImageUrl?: string | null;
}

export interface BookingReview {
  bookingId: number;
  bookingReference: string;
  bookingMode: BookingMode;
  bookingStatus: string;
  paymentStatus: string;
  clientFullName: string;
  clientEmailAddress: string;
  clientPhoneNumber: string;
  preferredContactMethod: PreferredContactMethod;
  totalDurationMinutes: number;
  totalAmount: number;
  depositAmount: number;
  balanceAmount: number;
  items: BookingReviewItem[];
}