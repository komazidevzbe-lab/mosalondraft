import { Component, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';

import { NavbarComponent } from '../../shared/navbar/navbar.component';
import { FooterComponent } from '../../shared/footer/footer.component';

@Component({
  selector: 'app-public-layout',
  standalone: true,
  imports: [
    RouterOutlet,
    NavbarComponent,
    FooterComponent
  ],
  templateUrl: './public-layout.component.html',
  styleUrl: './public-layout.component.css'
})
export class PublicLayoutComponent {
  private router = inject(Router);

  // ===============================
  // Home page background control
  // The home background is only applied when the current route is /home.
  // ===============================

  get isHomePage(): boolean {
    return this.router.url.startsWith('/home');
  }

  // ===============================
  // Services page background control
  // The services background is only applied when the current route is /services.
  // ===============================

  get isServicesPage(): boolean {
    return this.router.url.startsWith('/services');
  }
}