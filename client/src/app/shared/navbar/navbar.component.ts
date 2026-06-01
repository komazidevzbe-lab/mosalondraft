import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

import { SignoutModalService } from '../../_services/signout-modal.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  private readonly signoutModalService = inject(SignoutModalService);

  openSignoutModal(): void {
    this.signoutModalService.openModal();
  }
}