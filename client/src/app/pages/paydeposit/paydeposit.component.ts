import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-paydeposit',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './paydeposit.component.html',
  styleUrl: './paydeposit.component.css'
})
export class PaydepositComponent {
  // ===============================
  // Deposit payment data
  // Later this can come from shared booking state or backend data.
  // ===============================

  totalAmount = 830;
  depositAmount = 250;
  paymentMethod = 'PayFast';

  get balanceRemaining(): number {
    return this.totalAmount - this.depositAmount;
  }
}