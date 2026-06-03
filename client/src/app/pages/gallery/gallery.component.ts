import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';

import { GalleryCategory, GalleryImage } from '../../_models/gallery-image';
import { GalleryService } from '../../_services/gallery.service';

interface GalleryFilter {
  id: GalleryCategory;
  label: string;
  iconClass?: string;
  iconUrl?: string;
  iconAltText?: string;
}

interface GalleryItem extends GalleryImage {
  isLarge?: boolean;
  isTall?: boolean;
}

interface GalleryGridPosition {
  column: number;
  rowOffset: number;
  rowSpan: number;
}

@Component({
  selector: 'app-gallery',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './gallery.component.html',
  styleUrl: './gallery.component.css'
})
export class GalleryComponent implements OnInit {
  private readonly galleryService = inject(GalleryService);

  // ===============================
  // Gallery filters
  // These filters control which beauty inspiration cards are shown.
  // ===============================

  selectedCategory: GalleryCategory = 'all';

  filters: GalleryFilter[] = [
    {
      id: 'all',
      label: 'All',
      iconClass: 'fa-solid fa-table-cells-large'
    },
    {
      id: 'manicure',
      label: 'Manicure',
      iconUrl: 'assets/home/nailpolishicon.svg',
      iconAltText: 'Nail polish icon'
    },
    {
      id: 'pedicure',
      label: 'Pedicure',
      iconUrl: 'assets/home/footprinticon.svg',
      iconAltText: 'Bare foot print icon'
    },
    {
      id: 'makeup',
      label: 'Makeup',
      iconUrl: 'assets/home/makeupbrushicon.svg',
      iconAltText: 'Makeup brush icon'
    },
    {
      id: 'lashes',
      label: 'EyeLashes',
      iconUrl: 'assets/home/eyelashesicon.svg',
      iconAltText: 'Eyelashes icon'
    },
    {
      id: 'favorites',
      label: 'Favorites',
      iconClass: 'fa-regular fa-heart'
    }
  ];

  // ===============================
  // Load more settings
  // Pattern 1 loads 8 cards.
  // Pattern 2 loads 7 cards.
  // The gallery alternates between these two pattern sizes.
  // ===============================

  readonly galleryBatchSizes = [8, 7];
  visibleBatchCount = 1;

  // ===============================
  // Gallery cards
  // These are loaded from the backend database.
  // Favorite state is now also database-backed per logged-in user.
  // ===============================

  galleryItems: GalleryItem[] = [];
  isLoadingGallery = false;
  galleryErrorMessage = '';

  ngOnInit(): void {
    this.loadGalleryItems();
  }

  // ===============================
  // Backend data loading
  // Gets active gallery images from the API.
  // The API also returns which images the logged-in user favorited.
  // ===============================

  loadGalleryItems(): void {
    this.isLoadingGallery = true;
    this.galleryErrorMessage = '';

    this.galleryService.getGalleryImages().subscribe({
      next: galleryImages => {
        this.galleryItems = galleryImages.map(image => ({
          ...image,
          isFavorite: image.isFavorite ?? false
        }));

        this.isLoadingGallery = false;
      },
      error: error => {
        console.error('Failed to load gallery images:', error);

        this.galleryErrorMessage = 'Gallery images could not be loaded right now. Please try again later.';
        this.galleryItems = [];
        this.isLoadingGallery = false;
      }
    });
  }

  // ===============================
  // Gallery display helpers
  // ===============================

  get visibleItemLimit(): number {
    let itemLimit = 0;

    for (let batchIndex = 0; batchIndex < this.visibleBatchCount; batchIndex++) {
      itemLimit += this.galleryBatchSizes[batchIndex % this.galleryBatchSizes.length];
    }

    return itemLimit;
  }

  get filteredGalleryItems(): GalleryItem[] {
    if (this.selectedCategory === 'all') {
      return this.galleryItems;
    }

    if (this.selectedCategory === 'favorites') {
      return this.galleryItems.filter(item => item.isFavorite);
    }

    return this.galleryItems.filter(item => item.category === this.selectedCategory);
  }

  get visibleGalleryItems(): GalleryItem[] {
    return this.filteredGalleryItems.slice(0, this.visibleItemLimit);
  }

  get hasMoreGalleryItems(): boolean {
    return this.visibleGalleryItems.length < this.filteredGalleryItems.length;
  }

  selectCategory(category: GalleryCategory): void {
    this.selectedCategory = category;
    this.visibleBatchCount = 1;
  }

  loadMoreItems(): void {
    this.visibleBatchCount++;
  }

  toggleFavorite(item: GalleryItem): void {
    const originalFavoriteState = item.isFavorite;

    item.isFavorite = !item.isFavorite;
    this.galleryErrorMessage = '';

    const favoriteRequest = item.isFavorite
      ? this.galleryService.addGalleryImageToFavorites(item.id)
      : this.galleryService.removeGalleryImageFromFavorites(item.id);

    favoriteRequest.subscribe({
      next: updatedGalleryImage => {
        this.updateGalleryItemFavoriteState(
          updatedGalleryImage.id,
          updatedGalleryImage.isFavorite
        );
      },
      error: error => {
        console.error('Failed to update gallery favorite:', error);

        item.isFavorite = originalFavoriteState;
        this.galleryErrorMessage = 'Favorite could not be updated right now. Please try again.';
      }
    });
  }

  getGalleryCardGridStyle(index: number): { [key: string]: string } {
    const position = this.getGalleryGridPosition(index);
    const rowStart = position.batchIndex * 2 + position.rowOffset;

    return {
      'grid-column': `${position.column}`,
      'grid-row': `${rowStart} / span ${position.rowSpan}`
    };
  }

  isGalleryCardTall(index: number): boolean {
    return this.getGalleryGridPosition(index).rowSpan === 2;
  }

  trackByGalleryItemId(index: number, item: GalleryItem): number {
    return item.id;
  }

  private updateGalleryItemFavoriteState(
    galleryImageId: number,
    isFavorite: boolean
  ): void {
    this.galleryItems = this.galleryItems.map(item => {
      if (item.id !== galleryImageId) {
        return item;
      }

      return {
        ...item,
        isFavorite
      };
    });
  }

  private getGalleryGridPosition(index: number): GalleryGridPosition & { batchIndex: number } {
    let remainingIndex = index;
    let batchIndex = 0;

    while (remainingIndex >= this.galleryBatchSizes[batchIndex % this.galleryBatchSizes.length]) {
      remainingIndex -= this.galleryBatchSizes[batchIndex % this.galleryBatchSizes.length];
      batchIndex++;
    }

    const patternOne: GalleryGridPosition[] = [
      { column: 1, rowOffset: 1, rowSpan: 1 },
      { column: 1, rowOffset: 2, rowSpan: 1 },
      { column: 2, rowOffset: 1, rowSpan: 2 },
      { column: 3, rowOffset: 1, rowSpan: 1 },
      { column: 3, rowOffset: 2, rowSpan: 1 },
      { column: 4, rowOffset: 1, rowSpan: 2 },
      { column: 5, rowOffset: 1, rowSpan: 1 },
      { column: 5, rowOffset: 2, rowSpan: 1 }
    ];

    const patternTwo: GalleryGridPosition[] = [
      { column: 1, rowOffset: 1, rowSpan: 2 },
      { column: 2, rowOffset: 1, rowSpan: 1 },
      { column: 2, rowOffset: 2, rowSpan: 1 },
      { column: 3, rowOffset: 1, rowSpan: 2 },
      { column: 4, rowOffset: 1, rowSpan: 1 },
      { column: 4, rowOffset: 2, rowSpan: 1 },
      { column: 5, rowOffset: 1, rowSpan: 2 }
    ];

    const selectedPattern = batchIndex % 2 === 0 ? patternOne : patternTwo;

    return {
      ...selectedPattern[remainingIndex],
      batchIndex
    };
  }
}