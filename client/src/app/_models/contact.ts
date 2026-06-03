export interface CreateContactMessageRequest {
  interest: string;
  message: string;
}

export interface ContactMessage {
  id: number;
  fullName: string;
  emailAddress: string;
  phoneNumber: string;
  interest: string;
  message: string;
  messageStatus: string;
  statuses: string[];
  adminResponse?: string | null;
  respondedAt?: string | null;
  submittedAt: string;
}

export interface ClientReviewResponse {
  id: number;
  clientName: string;
  location: string;
  reviewText: string;
  rating: number;
  imageUrl: string;
  altText: string;
  isApproved: boolean;
  isFeatured: boolean;
  displayOrder: number;
  createdAt: string;
}