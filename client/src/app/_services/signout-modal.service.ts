import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SignoutModalService {
  private readonly isOpenSignal = signal(false);

  readonly isOpen = this.isOpenSignal.asReadonly();

  openModal(): void {
    this.isOpenSignal.set(true);
  }

  closeModal(): void {
    this.isOpenSignal.set(false);
  }
}