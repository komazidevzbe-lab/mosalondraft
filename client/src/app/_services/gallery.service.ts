import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

import { environment } from '../../environments/environment';
import { GalleryDatabaseCategory, GalleryImage } from '../_models/gallery-image';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class GalleryService {
  private readonly http = inject(HttpClient);
  private readonly accountService = inject(AccountService);
  private readonly baseUrl = environment.apiUrl;

  getGalleryImages(category?: GalleryDatabaseCategory) {
    let params = new HttpParams();

    if (category) {
      params = params.set('category', category);
    }

    return this.http.get<GalleryImage[]>(this.baseUrl + 'gallery', {
      params,
      headers: this.accountService.getAuthHeaders()
    });
  }

  getFavoriteGalleryImages() {
    return this.http.get<GalleryImage[]>(this.baseUrl + 'gallery/favorites', {
      headers: this.accountService.getAuthHeaders()
    });
  }

  addGalleryImageToFavorites(galleryImageId: number) {
    return this.http.post<GalleryImage>(
      `${this.baseUrl}gallery/favorites/${galleryImageId}`,
      {},
      {
        headers: this.accountService.getAuthHeaders()
      }
    );
  }

  removeGalleryImageFromFavorites(galleryImageId: number) {
    return this.http.delete<GalleryImage>(
      `${this.baseUrl}gallery/favorites/${galleryImageId}`,
      {
        headers: this.accountService.getAuthHeaders()
      }
    );
  }
}