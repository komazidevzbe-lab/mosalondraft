import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

import { AccountService } from '../../_services/account.service';
import { SignoutModalService } from '../../_services/signout-modal.service';

@Component({
  selector: 'app-signout',
  standalone: true,
  imports: [],
  templateUrl: './signout.component.html',
  styleUrl: './signout.component.css'
})
export class SignoutComponent {
  private readonly router = inject(Router);
  private readonly accountService = inject(AccountService);
  private readonly signoutModalService = inject(SignoutModalService);

  cancelSignout(): void {
    this.signoutModalService.closeModal();
  }

  confirmSignout(): void {
    if (!this.accountService.currentUser()) {
      this.finishClientSignout();
      return;
    }

    this.accountService.serverLogout().subscribe({
      next: () => {
        this.finishClientSignout();
      },
      error: () => {
        this.finishClientSignout();
      }
    });
  }

  private finishClientSignout(): void {
    this.accountService.logout();
    this.signoutModalService.closeModal();
    this.router.navigate(['/']);
  }
}