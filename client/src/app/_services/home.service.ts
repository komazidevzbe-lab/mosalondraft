import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { HomePage } from '../_models/home-page';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  private readonly http = inject(HttpClient);

  // ===============================
  // API base URL
  // This points to the ASP.NET Core backend running locally.
  // ===============================

  private readonly baseUrl = 'https://localhost:5001/api/';

  getHomePage(): Observable<HomePage> {
    return this.http.get<HomePage>(`${this.baseUrl}home`);
  }
}