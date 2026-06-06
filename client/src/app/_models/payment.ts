export interface InitiatePayment {
  bookingId: number;
}

export interface PaymentInitiationResponse {
  bookingId: number;
  bookingReference: string;
  paymentProvider: string;
  amount: number;
  paymentUrl: string;
  formFields: Record<string, string>;
}