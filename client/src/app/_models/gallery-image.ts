export type GalleryCategory =
  | 'all'
  | 'manicure'
  | 'pedicure'
  | 'makeup'
  | 'lashes'
  | 'favorites';

export type GalleryDatabaseCategory = Exclude<GalleryCategory, 'all' | 'favorites'>;

export interface GalleryImage {
  id: number;
  title: string;
  description: string;
  imageUrl: string;
  altText: string;
  category: GalleryDatabaseCategory;
  displayOrder: number;
  isFavorite: boolean;
}