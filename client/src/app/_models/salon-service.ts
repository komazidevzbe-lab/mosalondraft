export interface SalonServiceTypeOption {
  id: number;
  name: string;
  displayOrder: number;
}

export interface SalonServiceLengthOption {
  id: number;
  name: string;
  priceAddOn: number;
  displayOrder: number;
}

export interface SalonService {
  id: number;
  slug: string;
  title: string;
  description: string;
  imageUrl: string;
  altText: string;
  durationMinutes: number;
  basePrice: number;
  requiresLength: boolean;
  displayOrder: number;
  serviceTypes: SalonServiceTypeOption[];
  lengthOptions: SalonServiceLengthOption[];
}