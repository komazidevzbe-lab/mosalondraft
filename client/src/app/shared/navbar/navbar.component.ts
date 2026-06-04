import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';

import { BookingStateService } from '../../_services/booking-state.service';
import { SignoutModalService } from '../../_services/signout-modal.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  private readonly router = inject(Router);
  private readonly bookingStateService = inject(BookingStateService);
  private readonly signoutModalService = inject(SignoutModalService);

  private readonly servicesFlowRoutes = [
    '/services',
    '/booking',
    '/review-booking',
    '/confirmation'
  ];

  get isServicesFlowActive(): boolean {
    const currentUrl = this.router.url.split('?')[0];

    return this.servicesFlowRoutes.some(route => currentUrl === route);
  }

  startFreshServicesFlow(event: Event): void {
    event.preventDefault();

    this.bookingStateService.resetBookingFlow();
    this.router.navigate(['/services']);
  }

  openSignoutModal(): void {
    this.signoutModalService.openModal();
  }
}