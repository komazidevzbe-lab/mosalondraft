import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

import { environment } from '../../environments/environment';
import { SalonService } from '../_models/salon-service';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class SalonServiceService {
  private readonly http = inject(HttpClient);
  private readonly accountService = inject(AccountService);
  private readonly baseUrl = environment.apiUrl;

  getActiveServices() {
    return this.http.get<SalonService[]>(this.baseUrl + 'salonservices', {
      headers: this.accountService.getAuthHeaders()
    });
  }

  getServiceById(serviceId: number) {
    return this.http.get<SalonService>(`${this.baseUrl}salonservices/${serviceId}`, {
      headers: this.accountService.getAuthHeaders()
    });
  }

  getServiceBySlug(slug: string) {
    return this.http.get<SalonService>(`${this.baseUrl}salonservices/slug/${slug}`, {
      headers: this.accountService.getAuthHeaders()
    });
  }
}