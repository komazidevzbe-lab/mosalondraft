import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

type GalleryCategory = 'all' | 'manicure' | 'pedicure' | 'makeup' | 'lashes' | 'favorites';

interface GalleryFilter {
  id: GalleryCategory;
  label: string;
  iconClass?: string;
  iconUrl?: string;
  iconAltText?: string;
}

interface GalleryItem {
  id: number;
  title: string;
  description: string;
  imageUrl: string;
  altText: string;
  category: Exclude<GalleryCategory, 'all' | 'favorites'>;
  isFavorite: boolean;
  isLarge?: boolean;
  isTall?: boolean;
}

interface GalleryGridPosition {
  column: number;
  rowOffset: number;
  rowSpan: number;
}

@Component({
  selector: 'app-gallery',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './gallery.component.html',
  styleUrl: './gallery.component.css'
})
export class GalleryComponent {
  // ===============================
  // Gallery filters
  // These filters control which beauty inspiration cards are shown.
  // ===============================

  selectedCategory: GalleryCategory = 'all';

  filters: GalleryFilter[] = [
    {
      id: 'all',
      label: 'All',
      iconClass: 'fa-solid fa-table-cells-large'
    },
    {
      id: 'manicure',
      label: 'Manicure',
      iconUrl: 'assets/home/nailpolishicon.svg',
      iconAltText: 'Nail polish icon'
    },
    {
      id: 'pedicure',
      label: 'Pedicure',
      iconUrl: 'assets/home/footprinticon.svg',
      iconAltText: 'Bare foot print icon'
    },
    {
      id: 'makeup',
      label: 'Makeup',
      iconUrl: 'assets/home/makeupbrushicon.svg',
      iconAltText: 'Makeup brush icon'
    },
    {
      id: 'lashes',
      label: 'EyeLashes',
      iconUrl: 'assets/home/eyelashesicon.svg',
      iconAltText: 'Eyelashes icon'
    },
    {
      id: 'favorites',
      label: 'Favorites',
      iconClass: 'fa-regular fa-heart'
    }
  ];

  // ===============================
  // Load more settings
  // Pattern 1 loads 8 cards.
  // Pattern 2 loads 7 cards.
  // The gallery alternates between these two pattern sizes.
  // ===============================

  readonly galleryBatchSizes = [8, 7];
  visibleBatchCount = 1;

  // ===============================
  // Gallery cards
  // The first 4 cards use the original home collage images once.
  // After that, the gallery uses every image from each gallery folder once.
  // ===============================

  galleryItems: GalleryItem[] = [
    {
      id: 1,
      title: 'Blush Bloom',
      description: 'Manicure',
      imageUrl: 'assets/home/nailcardcollage.svg',
      altText: 'Manicure nail collage inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 2,
      title: 'Soft Glam Glow',
      description: 'Makeup',
      imageUrl: 'assets/home/makeupcardcollage.svg',
      altText: 'Makeup collage inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 3,
      title: 'Pearl Petal Steps',
      description: 'Pedicure',
      imageUrl: 'assets/home/pedicurecardcollage.svg',
      altText: 'Pedicure collage inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 4,
      title: 'Velvet Wink',
      description: 'Lashes',
      imageUrl: 'assets/home/lashescardcollage.svg',
      altText: 'Brows and lashes collage inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 5,
      title: 'Crystal Rose Spell',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/17.svg',
      altText: 'Crystal rose manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 6,
      title: 'Golden Sand Glow',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/20.svg',
      altText: 'Golden sand pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 7,
      title: 'Moonlit Glamour',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/12.svg',
      altText: 'Moonlit glamour makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 8,
      title: 'Fairy Lash Flutter',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/4.svg',
      altText: 'Fairy lash inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 9,
      title: 'Sugar Pearl Toes',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/3.svg',
      altText: 'Sugar pearl pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 10,
      title: 'Pink Diamond Bloom',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/8.svg',
      altText: 'Pink diamond manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 11,
      title: 'Midnight Lash Charm',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/19.svg',
      altText: 'Midnight lash inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 12,
      title: 'Champagne Halo',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/1.svg',
      altText: 'Champagne halo makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 13,
      title: 'Enchanted Petals',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/2.svg',
      altText: 'Enchanted petal manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 14,
      title: 'Blush Sole Dream',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/14.svg',
      altText: 'Blush sole pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 15,
      title: 'Royal Glow Beat',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/18.svg',
      altText: 'Royal glow makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 16,
      title: 'Soft Siren Flutter',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/7.svg',
      altText: 'Soft siren lashes inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 17,
      title: 'Petal Silk Steps',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/9.svg',
      altText: 'Petal silk pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 18,
      title: 'Rose Quartz Tips',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/20.svg',
      altText: 'Rose quartz manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 19,
      title: 'Starlight Face Beat',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/6.svg',
      altText: 'Starlight makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 20,
      title: 'Goddess Lash Veil',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/15.svg',
      altText: 'Goddess lash inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 21,
      title: 'Glitter Garden',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/11.svg',
      altText: 'Glitter garden manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 22,
      title: 'Fresh Pearl Walk',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/1.svg',
      altText: 'Fresh pearl pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 23,
      title: 'Whisper Wing Lashes',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/10.svg',
      altText: 'Whisper wing lashes inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 24,
      title: 'Golden Hour Muse',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/15.svg',
      altText: 'Golden hour makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 25,
      title: 'Sugar Blush Pedi',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/18.svg',
      altText: 'Sugar blush pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 26,
      title: 'Velvet Rose Nails',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/5.svg',
      altText: 'Velvet rose manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 27,
      title: 'Diamond Dust Glow',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/9.svg',
      altText: 'Diamond dust makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 28,
      title: 'Angel Wing Flick',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/2.svg',
      altText: 'Angel wing lashes inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 29,
      title: 'Pink Potion Set',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/14.svg',
      altText: 'Pink potion manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 30,
      title: 'Cream Cloud Toes',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/6.svg',
      altText: 'Cream cloud pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 31,
      title: 'Drama Queen Lashes',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/18.svg',
      altText: 'Drama queen lashes inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 32,
      title: 'Bronze Belle Glow',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/3.svg',
      altText: 'Bronze belle makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 33,
      title: 'Silk Slipper Shine',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/11.svg',
      altText: 'Silk slipper pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 34,
      title: 'Fairytale French',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/1.svg',
      altText: 'Fairytale french manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 35,
      title: 'Glamour Crown',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/20.svg',
      altText: 'Glamour crown makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 36,
      title: 'Butterfly Wink',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/12.svg',
      altText: 'Butterfly wink lashes inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 37,
      title: 'Ruby Rose Tips',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/18.svg',
      altText: 'Ruby rose manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 38,
      title: 'Crystal Pedi Glow',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/4.svg',
      altText: 'Crystal pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 39,
      title: 'Velvet Smoke',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/11.svg',
      altText: 'Velvet smoke makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 40,
      title: 'Luxe Lash Spell',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/6.svg',
      altText: 'Luxe lash inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 41,
      title: 'Rosewater Steps',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/16.svg',
      altText: 'Rosewater pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 42,
      title: 'Golden Petal Nails',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/9.svg',
      altText: 'Golden petal manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 43,
      title: 'Doll Eye Dream',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/20.svg',
      altText: 'Doll eye lashes inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 44,
      title: 'Soft Royal Flush',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/5.svg',
      altText: 'Soft royal makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 45,
      title: 'Blush Crystal Set',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/13.svg',
      altText: 'Blush crystal manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 46,
      title: 'Peach Pearl Pedi',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/8.svg',
      altText: 'Peach pearl pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 47,
      title: 'Muse Magic',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/14.svg',
      altText: 'Muse magic makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 48,
      title: 'Feather Kiss Lashes',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/1.svg',
      altText: 'Feather kiss lashes inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 49,
      title: 'Opal Toe Shine',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/19.svg',
      altText: 'Opal toe pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 50,
      title: 'Royal Rose Set',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/4.svg',
      altText: 'Royal rose manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 51,
      title: 'Starlit Lash Line',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/14.svg',
      altText: 'Starlit lash inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 52,
      title: 'Honey Glow Glam',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/8.svg',
      altText: 'Honey glow makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 53,
      title: 'Diamond Petal Tips',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/16.svg',
      altText: 'Diamond petal manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 54,
      title: 'Barefoot Blush',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/2.svg',
      altText: 'Barefoot blush pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 55,
      title: 'Pearl Skin Glow',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/17.svg',
      altText: 'Pearl skin makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 56,
      title: 'Soft Volume Spell',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/9.svg',
      altText: 'Soft volume lashes inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 57,
      title: 'Rose Gold Steps',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/13.svg',
      altText: 'Rose gold pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 58,
      title: 'Candy Floss Tips',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/7.svg',
      altText: 'Candy floss manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 59,
      title: 'Velvet Flutter',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/17.svg',
      altText: 'Velvet flutter lashes inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 60,
      title: 'Pink Moon Glam',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/2.svg',
      altText: 'Pink moon makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 61,
      title: 'Lavish Bloom Nails',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/19.svg',
      altText: 'Lavish bloom manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 62,
      title: 'Glossy Garden Pedi',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/7.svg',
      altText: 'Glossy garden pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 63,
      title: 'Satin Glow Beat',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/10.svg',
      altText: 'Satin glow makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 64,
      title: 'Cat Eye Flutter',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/5.svg',
      altText: 'Cat eye lashes inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 65,
      title: 'Champagne Pedi Glow',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/17.svg',
      altText: 'Champagne pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 66,
      title: 'Dreamy Pink Set',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/3.svg',
      altText: 'Dreamy pink manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 67,
      title: 'Royal Wing Lashes',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/13.svg',
      altText: 'Royal wing lashes inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 68,
      title: 'Glazed Goddess',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/19.svg',
      altText: 'Glazed goddess makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 69,
      title: 'Sparkle Rose Tips',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/15.svg',
      altText: 'Sparkle rose manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 70,
      title: 'Soft Petal Pedi',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/5.svg',
      altText: 'Soft petal pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 71,
      title: 'Celestial Soft Glam',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/4.svg',
      altText: 'Celestial makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 72,
      title: 'Glam Doll Lashes',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/16.svg',
      altText: 'Glam doll lashes inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 73,
      title: 'Silky Blush Toes',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/10.svg',
      altText: 'Silky blush pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 74,
      title: 'Magic Rose Nails',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/12.svg',
      altText: 'Magic rose manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 75,
      title: 'Flutter Fantasy',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/8.svg',
      altText: 'Flutter fantasy lashes inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 76,
      title: 'Soft Fire Glam',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/16.svg',
      altText: 'Soft fire makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 77,
      title: 'Pink Velvet Tips',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/6.svg',
      altText: 'Pink velvet manicure inspiration',
      category: 'manicure',
      isFavorite: false
    },
    {
      id: 78,
      title: 'Pearl Glow Pedi',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/15.svg',
      altText: 'Pearl glow pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 79,
      title: 'Chocolate Rose Glam',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/7.svg',
      altText: 'Chocolate rose makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 80,
      title: 'Enchanted Lash Lift',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/11.svg',
      altText: 'Enchanted lash lift inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 81,
      title: 'Golden Barefoot Glow',
      description: 'Pedicure',
      imageUrl: 'assets/gallery/pedicure/12.svg',
      altText: 'Golden barefoot pedicure inspiration',
      category: 'pedicure',
      isFavorite: false
    },
    {
      id: 82,
      title: 'Rose Muse Glam',
      description: 'Makeup',
      imageUrl: 'assets/gallery/makeup/13.svg',
      altText: 'Rose muse makeup inspiration',
      category: 'makeup',
      isFavorite: false
    },
    {
      id: 83,
      title: 'Silk Fan Lashes',
      description: 'Lashes',
      imageUrl: 'assets/gallery/lashes/3.svg',
      altText: 'Silk fan lashes inspiration',
      category: 'lashes',
      isFavorite: false
    },
    {
      id: 84,
      title: 'Glossy Fairy Nails',
      description: 'Manicure',
      imageUrl: 'assets/gallery/manicure/10.svg',
      altText: 'Glossy fairy manicure inspiration',
      category: 'manicure',
      isFavorite: false
    }
  ];

  // ===============================
  // Gallery display helpers
  // ===============================

  get visibleItemLimit(): number {
    let itemLimit = 0;

    for (let batchIndex = 0; batchIndex < this.visibleBatchCount; batchIndex++) {
      itemLimit += this.galleryBatchSizes[batchIndex % this.galleryBatchSizes.length];
    }

    return itemLimit;
  }

  get filteredGalleryItems(): GalleryItem[] {
    if (this.selectedCategory === 'all') {
      return this.galleryItems;
    }

    if (this.selectedCategory === 'favorites') {
      return this.galleryItems.filter(item => item.isFavorite);
    }

    return this.galleryItems.filter(item => item.category === this.selectedCategory);
  }

  get visibleGalleryItems(): GalleryItem[] {
    return this.filteredGalleryItems.slice(0, this.visibleItemLimit);
  }

  get hasMoreGalleryItems(): boolean {
    return this.visibleGalleryItems.length < this.filteredGalleryItems.length;
  }

  selectCategory(category: GalleryCategory): void {
    this.selectedCategory = category;
    this.visibleBatchCount = 1;
  }

  loadMoreItems(): void {
    this.visibleBatchCount++;
  }

  toggleFavorite(item: GalleryItem): void {
    item.isFavorite = !item.isFavorite;
  }

  getGalleryCardGridStyle(index: number): { [key: string]: string } {
    const position = this.getGalleryGridPosition(index);
    const rowStart = position.batchIndex * 2 + position.rowOffset;

    return {
      'grid-column': `${position.column}`,
      'grid-row': `${rowStart} / span ${position.rowSpan}`
    };
  }

  isGalleryCardTall(index: number): boolean {
    return this.getGalleryGridPosition(index).rowSpan === 2;
  }

  trackByGalleryItemId(index: number, item: GalleryItem): number {
    return item.id;
  }

  private getGalleryGridPosition(index: number): GalleryGridPosition & { batchIndex: number } {
    let remainingIndex = index;
    let batchIndex = 0;

    while (remainingIndex >= this.galleryBatchSizes[batchIndex % this.galleryBatchSizes.length]) {
      remainingIndex -= this.galleryBatchSizes[batchIndex % this.galleryBatchSizes.length];
      batchIndex++;
    }

    const patternOne: GalleryGridPosition[] = [
      { column: 1, rowOffset: 1, rowSpan: 1 },
      { column: 1, rowOffset: 2, rowSpan: 1 },
      { column: 2, rowOffset: 1, rowSpan: 2 },
      { column: 3, rowOffset: 1, rowSpan: 1 },
      { column: 3, rowOffset: 2, rowSpan: 1 },
      { column: 4, rowOffset: 1, rowSpan: 2 },
      { column: 5, rowOffset: 1, rowSpan: 1 },
      { column: 5, rowOffset: 2, rowSpan: 1 }
    ];

    const patternTwo: GalleryGridPosition[] = [
      { column: 1, rowOffset: 1, rowSpan: 2 },
      { column: 2, rowOffset: 1, rowSpan: 1 },
      { column: 2, rowOffset: 2, rowSpan: 1 },
      { column: 3, rowOffset: 1, rowSpan: 2 },
      { column: 4, rowOffset: 1, rowSpan: 1 },
      { column: 4, rowOffset: 2, rowSpan: 1 },
      { column: 5, rowOffset: 1, rowSpan: 2 }
    ];

    const selectedPattern = batchIndex % 2 === 0 ? patternOne : patternTwo;

    return {
      ...selectedPattern[remainingIndex],
      batchIndex
    };
  }
}