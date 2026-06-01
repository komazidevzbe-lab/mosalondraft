import { Routes } from '@angular/router';

import { LoginComponent } from './auth/login/login.component';
import { SignupComponent } from './auth/signup/signup.component';
import { ForgotpasswordComponent } from './auth/forgotpassword/forgotpassword.component';
import { PublicLayoutComponent } from './layout/public-layout/public-layout.component';
import { HomeComponent } from './pages/home/home.component';
import { ServicesComponent } from './pages/services/services.component';
import { GalleryComponent } from './pages/gallery/gallery.component';
import { ContactComponent } from './pages/contact/contact.component';
import { BookingComponent } from './pages/booking/booking.component';
import { ReviewbookingComponent } from './pages/reviewbooking/reviewbooking.component';
import { PaydepositComponent } from './pages/paydeposit/paydeposit.component';
import { ConfirmationComponent } from './pages/confirmation/confirmation.component';
import { authGuard } from './_guards/auth.guard';

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
  {
    path: 'forgot-password',
    component: ForgotpasswordComponent
  },

  // Main website pages - navbar/footer layout
  // Protected by authGuard so users must be logged in first.
  {
    path: '',
    component: PublicLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: 'home', component: HomeComponent },
      { path: 'services', component: ServicesComponent },
      { path: 'booking', component: BookingComponent },
      { path: 'review-booking', component: ReviewbookingComponent },
      { path: 'pay-deposit', component: PaydepositComponent },
      { path: 'confirmation', component: ConfirmationComponent },
      { path: 'gallery', component: GalleryComponent },
      { path: 'contact', component: ContactComponent }
    ]
  },

  {
    path: '**',
    redirectTo: ''
  }
];