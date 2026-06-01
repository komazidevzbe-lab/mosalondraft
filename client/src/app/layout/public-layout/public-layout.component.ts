import { Component, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';

import { NavbarComponent } from '../../shared/navbar/navbar.component';
import { FooterComponent } from '../../shared/footer/footer.component';
import { SignoutComponent } from '../../auth/signout/signout.component';
import { SignoutModalService } from '../../_services/signout-modal.service';

@Component({
  selector: 'app-public-layout',
  standalone: true,
  imports: [
    RouterOutlet,
    NavbarComponent,
    FooterComponent,
    SignoutComponent
  ],
  templateUrl: './public-layout.component.html',
  styleUrl: './public-layout.component.css'
})
export class PublicLayoutComponent {
  private router = inject(Router);

  readonly signoutModalService = inject(SignoutModalService);

  // ===============================
  // Home page background control
  // The home background is only applied when the current route is /home.
  // ===============================

  get isHomePage(): boolean {
    return this.router.url.startsWith('/home');
  }

  // ===============================
  // Booking flow background control
  // The booking flow pages use the services background.
  // ===============================

  get isBookingFlowPage(): boolean {
    return (
      this.router.url.startsWith('/booking') ||
      this.router.url.startsWith('/review-booking') ||
      this.router.url.startsWith('/pay-deposit') ||
      this.router.url.startsWith('/confirmation')
    );
  }

  // ===============================
  // Gallery page background control
  // The gallery background is applied only on the /gallery route.
  // ===============================

  get isGalleryPage(): boolean {
    return this.router.url.startsWith('/gallery');
  }

  // ===============================
  // Contact page background control
  // The contact background is applied only on the /contact route.
  // ===============================

  get isContactPage(): boolean {
    return this.router.url.startsWith('/contact');
  }
}