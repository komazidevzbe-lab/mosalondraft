import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

interface HomeCategory {
  title: string;
  imageUrl: string;
  altText: string;
  iconUrl: string;
  iconAltText: string;
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  // ===============================
  // Category cards
  // These cards are displayed in the Browse Categories section.
  // ===============================

  categories: HomeCategory[] = [
    {
      title: 'Manicure',
      imageUrl: 'assets/home/nailcardcollage.svg',
      altText: 'Manicure nail design',
      iconUrl: 'assets/home/nailpolishicon.svg',
      iconAltText: 'Nail polish icon'
    },
    {
      title: 'Makeup',
      imageUrl: 'assets/home/makeupcardcollage.svg',
      altText: 'Makeup service',
      iconUrl: 'assets/home/makeupbrushicon.svg',
      iconAltText: 'Makeup brush icon'
    },
    {
      title: 'Pedicure',
      imageUrl: 'assets/home/pedicurecardcollage.svg',
      altText: 'Pedicure service',
      iconUrl: 'assets/home/footprinticon.svg',
      iconAltText: 'Bare foot print icon'
    },
    {
      title: 'Brows & Lashes',
      imageUrl: 'assets/home/lashescardcollage.svg',
      altText: 'Brows and lashes service',
      iconUrl: 'assets/home/eyelashesicon.svg',
      iconAltText: 'Eyelashes icon'
    }
  ];
}