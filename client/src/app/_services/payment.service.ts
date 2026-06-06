import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

import { environment } from '../../environments/environment';
import { InitiatePayment, PaymentInitiationResponse } from '../_models/payment';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private readonly http = inject(HttpClient);
  private readonly accountService = inject(AccountService);
  private readonly baseUrl = environment.apiUrl;

  initiatePayFastPayment(model: InitiatePayment) {
    return this.http.post<PaymentInitiationResponse>(
      this.baseUrl + 'payments/payfast/initiate',
      model,
      {
        headers: this.accountService.getAuthHeaders()
      }
    );
  }

  cancelPayFastPayment(bookingId: number) {
    return this.http.post<{ message: string }>(
      `${this.baseUrl}payments/payfast/cancel/${bookingId}`,
      {},
      {
        headers: this.accountService.getAuthHeaders()
      }
    );
  }
}