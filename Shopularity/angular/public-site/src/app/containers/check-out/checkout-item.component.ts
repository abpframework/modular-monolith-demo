import {Component, inject, input, OnInit, PLATFORM_ID, signal} from '@angular/core';
import {CurrencyPipe, isPlatformBrowser} from "@angular/common";
import {Product} from "../../models";
import {ProductService} from "../../services";

@Component({
  selector: 'app-checkout-item',
  template: `
    <div class="d-flex pt-4 pb-2 px-2 justify-content-between">
      <div class="d-flex column-gap-2 align-items-top">
        <div class="checkout-item-image-container">
          <img [src]="imageUrl()" [alt]="product().name" loading="lazy" width="75" height="75"/>
        </div>
        <div class="d-flex flex-column gap-2">
          <span>{{ product().name }}</span>
          <span>Amount: {{ amount() }}</span>
        </div>
      </div>
      <div class="d-flex align-items-top">
        <b>{{ product()?.price | currency }}</b>
      </div>
    </div>
  `,
  imports: [
    CurrencyPipe
  ],
  styles: [
    `
      .checkout-item-image-container {
        width: 100px;
        height: 100px;
      }
    `
  ]
})

export class CheckoutItemComponent implements OnInit{
  product = input<Product>();
  amount = input<number>();
  imageUrl = signal<string>('');
  readonly productService = inject(ProductService);
  private platformId = inject(PLATFORM_ID);


  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.productService.getProductImage(this.product().id).subscribe(imageUrl => {
        this.imageUrl.set(imageUrl);
      });
    }
  }
}
