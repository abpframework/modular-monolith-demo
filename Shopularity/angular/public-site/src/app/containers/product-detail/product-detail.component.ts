import {Component, inject, OnInit, signal} from '@angular/core';
import {ActivatedRoute, RouterModule} from "@angular/router";

import { AuthService } from '@abp/ng.core';
import {ProductService} from "../../services";
import {ProductWithCategory} from "../../models";
import {BasketService} from "../../services/basket.service";
import {LoginRequiredModalComponent} from "../../components/login-required-modal";
import {CurrencyPipe} from "@angular/common";

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss'],
  imports: [RouterModule, LoginRequiredModalComponent, CurrencyPipe],
  providers: [
  ]
})
export class ProductDetailComponent implements OnInit {
  private authService = inject(AuthService);
  private productService = inject(ProductService);
  private route = inject(ActivatedRoute);
  private basketService = inject(BasketService);
  productImage = signal<string>('')
  productDetail: ProductWithCategory;
  isLoginRequiredModalVisible = false;

  ngOnInit() {
    this.getProductDetail();
    this.getProductImage();
  }

  login() {
    this.authService.navigateToLogin();
  }

  addToBasket(productId: string) {
    if (!this.authService.isAuthenticated) {
      this.isLoginRequiredModalVisible = true;
      return;
    }
    this.basketService.addToBasket(productId, 1)
      .subscribe(res => {
        this.basketService.fetchBasketCount().subscribe();
      })
  }

  private getProductDetail() {
    const productId = this.route.snapshot.params['productId'];
    this.productService.getProductDetail(productId).subscribe(product => {
      this.productDetail = product;
    })
  }

  private getProductImage() {
    const productId = this.route.snapshot.params['productId'];
    this.productService.getProductImage(productId).subscribe(imageUrl => {
      this.productImage.set(imageUrl);
    })
  }
}
