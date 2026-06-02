export interface HomePage {
  heroContent: HomePageContent;
  aboutContent: HomePageContent;
  heroImages: HomeHeroImages;
  featuredServices: HomeFeaturedService[];
  featuredReviews: HomeClientReview[];
  averageRating: number;
  reviewCount: number;
}

export interface HomePageContent {
  sectionKey: string;
  eyebrowText?: string | null;
  titleLineOne: string;
  titleLineOneHighlight?: string | null;
  titleLineTwo?: string | null;
  titleLineTwoHighlight?: string | null;
  description: string;
  buttonText?: string | null;
  buttonLink?: string | null;
}

export interface HomeHeroImages {
  makeup: HomeHeroImage[];
  lashes: HomeHeroImage[];
  pedicure: HomeHeroImage[];
  manicure: HomeHeroImage[];
}

export interface HomeHeroImage {
  category: string;
  imageUrl: string;
  altText: string;
  displayOrder: number;
}

export interface HomeFeaturedService {
  id: number;
  slug: string;
  name: string;
  description: string;
  imageUrl: string;
  altText: string;
  iconUrl: string;
  iconAltText: string;
  durationMinutes: number;
  basePrice: number;
  displayOrder: number;
}

export interface HomeClientReview {
  id: number;
  clientName: string;
  location: string;
  reviewText: string;
  rating: number;
  imageUrl: string;
  altText: string;
  displayOrder: number;
}