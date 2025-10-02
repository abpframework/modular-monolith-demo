import {Component, inject, input, OnInit, PLATFORM_ID, signal} from "@angular/core";
import {CurrencyPipe, isPlatformBrowser} from "@angular/common";
import {Router, RouterModule} from "@angular/router";

import {Product} from "../../../models";
import {BasketService} from "../../../services/basket.service";
import {CardComponent} from "@abp/ng.theme.shared";
import {AuthService, LocalizationPipe} from "@abp/ng.core";
import {ProductService} from "../../../services";
import {LoginRequiredModalComponent} from "../../../components/login-required-modal";

@Component({
  selector: 'app-product-list-item',
  templateUrl: './product-list-item.component.html',
  styleUrls: ['product-list-item.component.scss'],
  imports: [RouterModule, CurrencyPipe, CardComponent, LoginRequiredModalComponent, LocalizationPipe],
  providers: [
  ]
})

export class ProductListItemComponent implements OnInit {
  product = input<Product>();
  imageUrl = signal<string>('');
  private router = inject(Router);
  private basketService = inject(BasketService);
  private authService = inject(AuthService);
  private productService = inject(ProductService);
  private platformId = inject(PLATFORM_ID);
  isLoginRequiredModalVisible = false;

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.productService.getProductImage(this.product().id).subscribe(imageUrl => {
        this.imageUrl.set(imageUrl);
      });
    }
  }

  showProductDetail() {
    this.router.navigate([`/product/${this.product().id}`]);
  }

  addToBasket(e, productId) {
    e.preventDefault();
    e.stopPropagation();
    if (!this.authService.isAuthenticated) {
      this.isLoginRequiredModalVisible = true;
      return;
    }
    this.basketService.addToBasket(productId, 1)
      .subscribe(res => {
        this.basketService.fetchBasketCount().subscribe();
      })
  }
}
