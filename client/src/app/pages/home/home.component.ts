import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

interface HomeCategory {
  title: string;
  description: string;
  imageUrl: string;
  altText: string;
  iconUrl: string;
  iconAltText: string;
}

interface ClientReview {
  clientName: string;
  location: string;
  reviewText: string;
  imageUrl: string;
  altText: string;
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit, OnDestroy {
  // ===============================
  // Hero makeup slideshow
  // This controls only the first large makeup card in the hero section.
  // ===============================

  makeupHeroImages: string[] = [
    'assets/gallery/makeup/1.svg',
    'assets/gallery/makeup/2.svg',
    'assets/gallery/makeup/3.svg',
    'assets/gallery/makeup/4.svg',
    'assets/gallery/makeup/5.svg',
    'assets/gallery/makeup/6.svg',
    'assets/gallery/makeup/7.svg',
    'assets/gallery/makeup/8.svg',
    'assets/gallery/makeup/9.svg',
    'assets/gallery/makeup/10.svg',
    'assets/gallery/makeup/11.svg',
    'assets/gallery/makeup/12.svg',
    'assets/gallery/makeup/13.svg',
    'assets/gallery/makeup/14.svg',
    'assets/gallery/makeup/15.svg',
    'assets/gallery/makeup/16.svg',
    'assets/gallery/makeup/17.svg',
    'assets/gallery/makeup/18.svg',
    'assets/gallery/makeup/19.svg',
    'assets/gallery/makeup/20.svg',
    'assets/gallery/makeup/21.svg',
    'assets/gallery/makeup/22.svg',
    'assets/gallery/makeup/23.svg',
    'assets/gallery/makeup/24.svg',
    'assets/gallery/makeup/25.svg',
    'assets/gallery/makeup/26.svg'
  ];

  currentMakeupHeroImageIndex = 0;
  nextMakeupHeroImageIndex = 1;
  isMakeupSlideTransitioning = false;
  isMakeupSlideResetting = false;

  private makeupSlideshowIntervalId?: number;
  private makeupSlideshowTimeoutId?: number;
  private makeupSlideshowResetTimeoutId?: number;

  get currentMakeupHeroImage(): string {
    return this.makeupHeroImages[this.currentMakeupHeroImageIndex];
  }

  get nextMakeupHeroImage(): string {
    return this.makeupHeroImages[this.nextMakeupHeroImageIndex];
  }

  // ===============================
  // Hero lashes slideshow
  // This controls only the top small lashes card in the hero section.
  // ===============================

  lashesHeroImages: string[] = [
    'assets/gallery/lashes/1.svg',
    'assets/gallery/lashes/2.svg',
    'assets/gallery/lashes/3.svg',
    'assets/gallery/lashes/4.svg',
    'assets/gallery/lashes/5.svg',
    'assets/gallery/lashes/6.svg',
    'assets/gallery/lashes/7.svg',
    'assets/gallery/lashes/8.svg',
    'assets/gallery/lashes/9.svg',
    'assets/gallery/lashes/10.svg',
    'assets/gallery/lashes/11.svg',
    'assets/gallery/lashes/12.svg',
    'assets/gallery/lashes/13.svg',
    'assets/gallery/lashes/14.svg',
    'assets/gallery/lashes/15.svg',
    'assets/gallery/lashes/16.svg',
    'assets/gallery/lashes/17.svg',
    'assets/gallery/lashes/18.svg',
    'assets/gallery/lashes/19.svg',
    'assets/gallery/lashes/20.svg'
  ];

  currentLashesHeroImageIndex = 0;
  nextLashesHeroImageIndex = 1;
  isLashesSlideTransitioning = false;
  isLashesSlideResetting = false;

  private lashesSlideshowIntervalId?: number;
  private lashesSlideshowTimeoutId?: number;
  private lashesSlideshowResetTimeoutId?: number;

  get currentLashesHeroImage(): string {
    return this.lashesHeroImages[this.currentLashesHeroImageIndex];
  }

  get nextLashesHeroImage(): string {
    return this.lashesHeroImages[this.nextLashesHeroImageIndex];
  }

  // ===============================
  // Hero pedicure slideshow
  // This controls only the bottom small pedicure card in the hero section.
  // ===============================

  pedicureHeroImages: string[] = [
    'assets/gallery/pedicure/1.svg',
    'assets/gallery/pedicure/2.svg',
    'assets/gallery/pedicure/3.svg',
    'assets/gallery/pedicure/4.svg',
    'assets/gallery/pedicure/5.svg',
    'assets/gallery/pedicure/6.svg',
    'assets/gallery/pedicure/7.svg',
    'assets/gallery/pedicure/8.svg',
    'assets/gallery/pedicure/9.svg',
    'assets/gallery/pedicure/10.svg',
    'assets/gallery/pedicure/11.svg',
    'assets/gallery/pedicure/12.svg',
    'assets/gallery/pedicure/13.svg',
    'assets/gallery/pedicure/14.svg',
    'assets/gallery/pedicure/15.svg',
    'assets/gallery/pedicure/16.svg',
    'assets/gallery/pedicure/17.svg',
    'assets/gallery/pedicure/18.svg',
    'assets/gallery/pedicure/19.svg',
    'assets/gallery/pedicure/20.svg',
    'assets/gallery/pedicure/21.svg',
    'assets/gallery/pedicure/22.svg',
    'assets/gallery/pedicure/23.svg',
    'assets/gallery/pedicure/24.svg'
  ];

  currentPedicureHeroImageIndex = 0;
  nextPedicureHeroImageIndex = 1;
  isPedicureSlideTransitioning = false;
  isPedicureSlideResetting = false;

  private pedicureSlideshowIntervalId?: number;
  private pedicureSlideshowTimeoutId?: number;
  private pedicureSlideshowResetTimeoutId?: number;

  get currentPedicureHeroImage(): string {
    return this.pedicureHeroImages[this.currentPedicureHeroImageIndex];
  }

  get nextPedicureHeroImage(): string {
    return this.pedicureHeroImages[this.nextPedicureHeroImageIndex];
  }

  // ===============================
  // Hero manicure slideshow
  // This controls only the last large manicure card in the hero section.
  // ===============================

  manicureHeroImages: string[] = [
    'assets/gallery/manicure/1.svg',
    'assets/gallery/manicure/2.svg',
    'assets/gallery/manicure/3.svg',
    'assets/gallery/manicure/4.svg',
    'assets/gallery/manicure/5.svg',
    'assets/gallery/manicure/6.svg',
    'assets/gallery/manicure/7.svg',
    'assets/gallery/manicure/8.svg',
    'assets/gallery/manicure/9.svg',
    'assets/gallery/manicure/10.svg',
    'assets/gallery/manicure/11.svg',
    'assets/gallery/manicure/12.svg',
    'assets/gallery/manicure/13.svg',
    'assets/gallery/manicure/14.svg',
    'assets/gallery/manicure/15.svg',
    'assets/gallery/manicure/16.svg',
    'assets/gallery/manicure/17.svg',
    'assets/gallery/manicure/18.svg',
    'assets/gallery/manicure/19.svg',
    'assets/gallery/manicure/20.svg',
    'assets/gallery/manicure/21.svg',
    'assets/gallery/manicure/22.svg',
    'assets/gallery/manicure/23.svg',
    'assets/gallery/manicure/24.svg',
    'assets/gallery/manicure/25.svg',
    'assets/gallery/manicure/26.svg',
    'assets/gallery/manicure/27.svg',
    'assets/gallery/manicure/28.svg',
    'assets/gallery/manicure/29.svg',
    'assets/gallery/manicure/30.svg'
  ];

  currentManicureHeroImageIndex = 0;
  nextManicureHeroImageIndex = 1;
  isManicureSlideTransitioning = false;
  isManicureSlideResetting = false;

  private manicureSlideshowIntervalId?: number;
  private manicureSlideshowTimeoutId?: number;
  private manicureSlideshowResetTimeoutId?: number;

  get currentManicureHeroImage(): string {
    return this.manicureHeroImages[this.currentManicureHeroImageIndex];
  }

  get nextManicureHeroImage(): string {
    return this.manicureHeroImages[this.nextManicureHeroImageIndex];
  }

  ngOnInit(): void {
    this.startMakeupHeroSlideshow();
    this.startLashesHeroSlideshow();
    this.startPedicureHeroSlideshow();
    this.startManicureHeroSlideshow();
  }

  ngOnDestroy(): void {
    this.stopMakeupHeroSlideshow();
    this.stopLashesHeroSlideshow();
    this.stopPedicureHeroSlideshow();
    this.stopManicureHeroSlideshow();
  }

  private startMakeupHeroSlideshow(): void {
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

  // ===============================
  // Category cards
  // These cards are displayed in the Explore Services section.
  // ===============================

  categories: HomeCategory[] = [
    {
      title: 'MANICURE',
      description: 'Polished to perfection. Every detail.',
      imageUrl: 'assets/home/nailcardcollage.svg',
      altText: 'Manicure nail design',
      iconUrl: 'assets/home/nailpolishicon.svg',
      iconAltText: 'Nail polish icon'
    },
    {
      title: 'MAKEUP',
      description: 'Flawless looks for every occasion.',
      imageUrl: 'assets/home/makeupcardcollage.svg',
      altText: 'Makeup service',
      iconUrl: 'assets/home/makeupbrushicon.svg',
      iconAltText: 'Makeup brush icon'
    },
    {
      title: 'PEDICURE',
      description: 'Relaxing care for beautiful feet.',
      imageUrl: 'assets/home/pedicurecardcollage.svg',
      altText: 'Pedicure service',
      iconUrl: 'assets/home/footprinticon.svg',
      iconAltText: 'Bare foot print icon'
    },
    {
      title: 'BROWS & LASHES',
      description: 'Frame your beauty. Enhance your look.',
      imageUrl: 'assets/home/lashescardcollage.svg',
      altText: 'Brows and lashes service',
      iconUrl: 'assets/home/eyelashesicon.svg',
      iconAltText: 'Eyelashes icon'
    }
  ];

  clientReviews: ClientReview[] = [
    {
      clientName: 'Lerato M.',
      location: 'Cape Town, GP',
      reviewText: 'Absolutely love my nails! The attention to detail and overall experience was beyond amazing.',
      imageUrl: 'assets/gallery/makeup/3.svg',
      altText: 'Client review profile'
    },
    {
      clientName: 'Asanda R.',
      location: 'Joburg, SL',
      reviewText: 'The best pedicure I have ever had. So relaxing and my feet have never looked better!',
      imageUrl: 'assets/gallery/makeup/8.svg',
      altText: 'Client review profile'
    },
    {
      clientName: 'Nontutuzelo L.',
      location: 'Bloemfontein, CBD',
      reviewText: 'My brows and lashes have never looked this good. I feel so confident every single day.',
      imageUrl: 'assets/gallery/makeup/12.svg',
      altText: 'Client review profile'
    }
  ];
}