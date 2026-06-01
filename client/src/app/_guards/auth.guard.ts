import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { AccountService } from '../_services/account.service';

export const authGuard: CanActivateFn = () => {
  const accountService = inject(AccountService);
  const router = inject(Router);

  // Restore the logged-in user after page refresh.
  if (!accountService.currentUser()) {
    accountService.loadCurrentUserFromStorage();
  }

  // Allow access if the user exists and the token is still valid.
  if (accountService.currentUser()) {
    return true;
  }

  // Redirect unauthenticated users back to the login page.
  return router.createUrlTree(['/']);
};