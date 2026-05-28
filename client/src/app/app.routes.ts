import { Routes } from '@angular/router';

import { LoginComponent } from './auth/login/login.component';
import { SignupComponent } from './auth/signup/signup.component';
import { PublicLayoutComponent } from './layout/public-layout/public-layout.component';
import { HomeComponent } from './pages/home/home.component';
import { ServicesComponent } from './pages/services/services.component';
import { GalleryComponent } from './pages/gallery/gallery.component';
import { ContactComponent } from './pages/contact/contact.component';
import { BookingComponent } from './pages/booking/booking.component';

export const routes: Routes = [
  // Landing / authentication pages - no navbar
  {
    path: '',
    pathMatch: 'full',
    component: LoginComponent
  },
  {
    path: 'signup',
    component: SignupComponent
  },

  // Main website pages - navbar/footer layout, for later
  {
    path: '',
    component: PublicLayoutComponent,
    children: [
      { path: 'home', component: HomeComponent },
      { path: 'services', component: ServicesComponent },
      { path: 'booking', component: BookingComponent },
      { path: 'gallery', component: GalleryComponent },
      { path: 'contact', component: ContactComponent }
    ]
  },

  {
    path: '**',
    redirectTo: ''
  }
];