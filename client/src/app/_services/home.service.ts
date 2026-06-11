import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { HomePage } from '../_models/home-page';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  private readonly http = inject(HttpClient);

  // ===============================
  // API base URL
  // Uses environment.apiUrl so local development uses localhost
  // and production uses the deployed App Service API path.
  // ===============================

  private readonly baseUrl = environment.apiUrl;

  getHomePage(): Observable<HomePage> {
    return this.http.get<HomePage>(`${this.baseUrl}home`);
  }
}