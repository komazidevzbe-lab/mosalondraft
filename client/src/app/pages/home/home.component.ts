import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';

import {
  HomeClientReview,
  HomeFeaturedService,
  HomeHeroImage,
  HomePage,
  HomePageContent
} from '../../_models/home-page';
import { HomeService } from '../../_services/home.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit, OnDestroy {
  private readonly homeService = inject(HomeService);

  // ===============================
  // Home page data
  // Loaded from the backend instead of being hardcoded in Angular.
  // ===============================

  homePage?: HomePage;
  isLoadingHomePage = true;
  homePageError = '';

  makeupHeroImages: HomeHeroImage[] = [];
  lashesHeroImages: HomeHeroImage[] = [];
  pedicureHeroImages: HomeHeroImage[] = [];
  manicureHeroImages: HomeHeroImage[] = [];

  categories: HomeFeaturedService[] = [];
  clientReviews: HomeClientReview[] = [];

  // ===============================
  // Client review carousel
  // Shows 3 reviews at a time and slides by one review.
  // ===============================

  readonly visibleReviewCount = 3;
  currentReviewStartIndex = 0;

  get visibleClientReviews(): HomeClientReview[] {
    if (this.clientReviews.length <= this.visibleReviewCount) {
      return this.clientReviews;
    }

    return Array.from({ length: this.visibleReviewCount }, (_, index) => {
      const reviewIndex = (this.currentReviewStartIndex + index) % this.clientReviews.length;
      return this.clientReviews[reviewIndex];
    });
  }

  get canSlideReviews(): boolean {
    return this.clientReviews.length > this.visibleReviewCount;
  }

  nextReviews(): void {
    if (!this.canSlideReviews) {
      return;
    }

    this.currentReviewStartIndex = (this.currentReviewStartIndex + 1) % this.clientReviews.length;
  }

  previousReviews(): void {
    if (!this.canSlideReviews) {
      return;
    }

    this.currentReviewStartIndex =
      (this.currentReviewStartIndex - 1 + this.clientReviews.length) % this.clientReviews.length;
  }

  trackByReviewId(index: number, review: HomeClientReview): number {
    return review.id;
  }

  // ===============================
  // Hero makeup slideshow
  // This controls only the first large makeup card in the hero section.
  // ===============================

  currentMakeupHeroImageIndex = 0;
  nextMakeupHeroImageIndex = 1;
  isMakeupSlideTransitioning = false;
  isMakeupSlideResetting = false;

  private makeupSlideshowIntervalId?: number;
  private makeupSlideshowTimeoutId?: number;
  private makeupSlideshowResetTimeoutId?: number;

  get currentMakeupHeroImage(): HomeHeroImage | undefined {
    return this.makeupHeroImages[this.currentMakeupHeroImageIndex];
  }

  get nextMakeupHeroImage(): HomeHeroImage | undefined {
    return this.makeupHeroImages[this.nextMakeupHeroImageIndex];
  }

  // ===============================
  // Hero lashes slideshow
  // This controls only the top small lashes card in the hero section.
  // ===============================

  currentLashesHeroImageIndex = 0;
  nextLashesHeroImageIndex = 1;
  isLashesSlideTransitioning = false;
  isLashesSlideResetting = false;

  private lashesSlideshowIntervalId?: number;
  private lashesSlideshowTimeoutId?: number;
  private lashesSlideshowResetTimeoutId?: number;

  get currentLashesHeroImage(): HomeHeroImage | undefined {
    return this.lashesHeroImages[this.currentLashesHeroImageIndex];
  }

  get nextLashesHeroImage(): HomeHeroImage | undefined {
    return this.lashesHeroImages[this.nextLashesHeroImageIndex];
  }

  // ===============================
  // Hero pedicure slideshow
  // This controls only the bottom small pedicure card in the hero section.
  // ===============================

  currentPedicureHeroImageIndex = 0;
  nextPedicureHeroImageIndex = 1;
  isPedicureSlideTransitioning = false;
  isPedicureSlideResetting = false;

  private pedicureSlideshowIntervalId?: number;
  private pedicureSlideshowTimeoutId?: number;
  private pedicureSlideshowResetTimeoutId?: number;

  get currentPedicureHeroImage(): HomeHeroImage | undefined {
    return this.pedicureHeroImages[this.currentPedicureHeroImageIndex];
  }

  get nextPedicureHeroImage(): HomeHeroImage | undefined {
    return this.pedicureHeroImages[this.nextPedicureHeroImageIndex];
  }

  // ===============================
  // Hero manicure slideshow
  // This controls only the last large manicure card in the hero section.
  // ===============================

  currentManicureHeroImageIndex = 0;
  nextManicureHeroImageIndex = 1;
  isManicureSlideTransitioning = false;
  isManicureSlideResetting = false;

  private manicureSlideshowIntervalId?: number;
  private manicureSlideshowTimeoutId?: number;
  private manicureSlideshowResetTimeoutId?: number;

  get currentManicureHeroImage(): HomeHeroImage | undefined {
    return this.manicureHeroImages[this.currentManicureHeroImageIndex];
  }

  get nextManicureHeroImage(): HomeHeroImage | undefined {
    return this.manicureHeroImages[this.nextManicureHeroImageIndex];
  }

  ngOnInit(): void {
    this.loadHomePage();
  }

  ngOnDestroy(): void {
    this.stopAllSlideshows();
  }

  // ===============================
  // Load Home page data
  // ===============================

  private loadHomePage(): void {
    this.homeService.getHomePage().subscribe({
      next: homePage => {
        this.homePage = homePage;

        this.makeupHeroImages = homePage.heroImages.makeup;
        this.lashesHeroImages = homePage.heroImages.lashes;
        this.pedicureHeroImages = homePage.heroImages.pedicure;
        this.manicureHeroImages = homePage.heroImages.manicure;

        this.categories = homePage.featuredServices;
        this.clientReviews = homePage.featuredReviews;
        this.currentReviewStartIndex = 0;

        this.resetSlideshowIndexes();
        this.startAllSlideshows();

        this.isLoadingHomePage = false;
      },
      error: () => {
        this.homePageError = 'Home page content could not be loaded. Please try again later.';
        this.isLoadingHomePage = false;
      }
    });
  }

  private resetSlideshowIndexes(): void {
    this.currentMakeupHeroImageIndex = 0;
    this.nextMakeupHeroImageIndex = this.makeupHeroImages.length > 1 ? 1 : 0;

    this.currentLashesHeroImageIndex = 0;
    this.nextLashesHeroImageIndex = this.lashesHeroImages.length > 1 ? 1 : 0;

    this.currentPedicureHeroImageIndex = 0;
    this.nextPedicureHeroImageIndex = this.pedicureHeroImages.length > 1 ? 1 : 0;

    this.currentManicureHeroImageIndex = 0;
    this.nextManicureHeroImageIndex = this.manicureHeroImages.length > 1 ? 1 : 0;
  }

  // ===============================
  // Text helper
  // Keeps highlighted words working with backend content.
  // ===============================

  getTextBeforeHighlight(text: string, highlight?: string | null): string {
    if (!highlight || !text.includes(highlight)) {
      return text;
    }

    return text.split(highlight)[0];
  }

  getTextAfterHighlight(text: string, highlight?: string | null): string {
    if (!highlight || !text.includes(highlight)) {
      return '';
    }

    return text.split(highlight).slice(1).join(highlight);
  }

  getRouterLink(link?: string | null): string {
    return link && link.trim().length > 0 ? link : '/services';
  }

  getReviewStars(rating: number): number[] {
    return Array.from({ length: rating }, (_, index) => index + 1);
  }

  // ===============================
  // Slideshow starters
  // ===============================

  private startAllSlideshows(): void {
    this.startMakeupHeroSlideshow();
    this.startLashesHeroSlideshow();
    this.startPedicureHeroSlideshow();
    this.startManicureHeroSlideshow();
  }

  private startMakeupHeroSlideshow(): void {
    if (this.makeupHeroImages.length <= 1) {
      return;
    }

    this.makeupSlideshowIntervalId = window.setInterval(() => {
      if (this.isMakeupSlideTransitioning) {
        return;
      }

      this.nextMakeupHeroImageIndex =
        (this.currentMakeupHeroImageIndex + 1) % this.makeupHeroImages.length;

      this.isMakeupSlideTransitioning = true;
      this.isMakeupSlideResetting = false;

      this.makeupSlideshowTimeoutId = window.setTimeout(() => {
        this.isMakeupSlideResetting = true;

        this.currentMakeupHeroImageIndex = this.nextMakeupHeroImageIndex;
        this.nextMakeupHeroImageIndex =
          (this.currentMakeupHeroImageIndex + 1) % this.makeupHeroImages.length;

        this.isMakeupSlideTransitioning = false;

        this.makeupSlideshowResetTimeoutId = window.setTimeout(() => {
          this.isMakeupSlideResetting = false;
        }, 80);
      }, 2600);
    }, 7000);
  }

  private startLashesHeroSlideshow(): void {
    if (this.lashesHeroImages.length <= 1) {
      return;
    }

    this.lashesSlideshowIntervalId = window.setInterval(() => {
      if (this.isLashesSlideTransitioning) {
        return;
      }

      this.nextLashesHeroImageIndex =
        (this.currentLashesHeroImageIndex + 1) % this.lashesHeroImages.length;

      this.isLashesSlideTransitioning = true;
      this.isLashesSlideResetting = false;

      this.lashesSlideshowTimeoutId = window.setTimeout(() => {
        this.isLashesSlideResetting = true;

        this.currentLashesHeroImageIndex = this.nextLashesHeroImageIndex;
        this.nextLashesHeroImageIndex =
          (this.currentLashesHeroImageIndex + 1) % this.lashesHeroImages.length;

        this.isLashesSlideTransitioning = false;

        this.lashesSlideshowResetTimeoutId = window.setTimeout(() => {
          this.isLashesSlideResetting = false;
        }, 80);
      }, 2600);
    }, 7000);
  }

  private startPedicureHeroSlideshow(): void {
    if (this.pedicureHeroImages.length <= 1) {
      return;
    }

    this.pedicureSlideshowIntervalId = window.setInterval(() => {
      if (this.isPedicureSlideTransitioning) {
        return;
      }

      this.nextPedicureHeroImageIndex =
        (this.currentPedicureHeroImageIndex + 1) % this.pedicureHeroImages.length;

      this.isPedicureSlideTransitioning = true;
      this.isPedicureSlideResetting = false;

      this.pedicureSlideshowTimeoutId = window.setTimeout(() => {
        this.isPedicureSlideResetting = true;

        this.currentPedicureHeroImageIndex = this.nextPedicureHeroImageIndex;
        this.nextPedicureHeroImageIndex =
          (this.currentPedicureHeroImageIndex + 1) % this.pedicureHeroImages.length;

        this.isPedicureSlideTransitioning = false;

        this.pedicureSlideshowResetTimeoutId = window.setTimeout(() => {
          this.isPedicureSlideResetting = false;
        }, 80);
      }, 2600);
    }, 7000);
  }

  private startManicureHeroSlideshow(): void {
    if (this.manicureHeroImages.length <= 1) {
      return;
    }

    this.manicureSlideshowIntervalId = window.setInterval(() => {
      if (this.isManicureSlideTransitioning) {
        return;
      }

      this.nextManicureHeroImageIndex =
        (this.currentManicureHeroImageIndex + 1) % this.manicureHeroImages.length;

      this.isManicureSlideTransitioning = true;
      this.isManicureSlideResetting = false;

      this.manicureSlideshowTimeoutId = window.setTimeout(() => {
        this.isManicureSlideResetting = true;

        this.currentManicureHeroImageIndex = this.nextManicureHeroImageIndex;
        this.nextManicureHeroImageIndex =
          (this.currentManicureHeroImageIndex + 1) % this.manicureHeroImages.length;

        this.isManicureSlideTransitioning = false;

        this.manicureSlideshowResetTimeoutId = window.setTimeout(() => {
          this.isManicureSlideResetting = false;
        }, 80);
      }, 2600);
    }, 7000);
  }

  // ===============================
  // Slideshow cleanup
  // ===============================

  private stopAllSlideshows(): void {
    this.stopMakeupHeroSlideshow();
    this.stopLashesHeroSlideshow();
    this.stopPedicureHeroSlideshow();
    this.stopManicureHeroSlideshow();
  }

  private stopMakeupHeroSlideshow(): void {
    if (this.makeupSlideshowIntervalId) {
      window.clearInterval(this.makeupSlideshowIntervalId);
    }

    if (this.makeupSlideshowTimeoutId) {
      window.clearTimeout(this.makeupSlideshowTimeoutId);
    }

    if (this.makeupSlideshowResetTimeoutId) {
      window.clearTimeout(this.makeupSlideshowResetTimeoutId);
    }
  }

  private stopLashesHeroSlideshow(): void {
    if (this.lashesSlideshowIntervalId) {
      window.clearInterval(this.lashesSlideshowIntervalId);
    }

    if (this.lashesSlideshowTimeoutId) {
      window.clearTimeout(this.lashesSlideshowTimeoutId);
    }

    if (this.lashesSlideshowResetTimeoutId) {
      window.clearTimeout(this.lashesSlideshowResetTimeoutId);
    }
  }

  private stopPedicureHeroSlideshow(): void {
    if (this.pedicureSlideshowIntervalId) {
      window.clearInterval(this.pedicureSlideshowIntervalId);
    }

    if (this.pedicureSlideshowTimeoutId) {
      window.clearTimeout(this.pedicureSlideshowTimeoutId);
    }

    if (this.pedicureSlideshowResetTimeoutId) {
      window.clearTimeout(this.pedicureSlideshowResetTimeoutId);
    }
  }

  private stopManicureHeroSlideshow(): void {
    if (this.manicureSlideshowIntervalId) {
      window.clearInterval(this.manicureSlideshowIntervalId);
    }

    if (this.manicureSlideshowTimeoutId) {
      window.clearTimeout(this.manicureSlideshowTimeoutId);
    }

    if (this.manicureSlideshowResetTimeoutId) {
      window.clearTimeout(this.manicureSlideshowResetTimeoutId);
    }
  }
}