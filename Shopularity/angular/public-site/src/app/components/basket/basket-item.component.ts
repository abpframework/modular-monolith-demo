import {Component, inject, input, OnInit, output, signal} from "@angular/core";
import {Product} from "../../models";
import {ProductService} from "../../services";

@Component({
  selector: 'app-basket-item',
  template: `
    <div class="d-flex flex-1 position-relative">
      <button class="btn btn-outline-secondary btn-sm position-absolute top-0 left-0" (click)="removeAllFromBasket.emit()">
        <i class="fa fa-solid fa-trash"></i>
      </button>
      <div class="product-image-container basket-image">
        <img
          class="card-img-top"
          [src]="productImage()"
          [alt]="product().name"
          loading="lazy"
        />
      </div>
      <div class="d-flex flex-column gap-1">
        <span>{{product().name}}</span>
        <b><span>$</span>{{product().price}}</b>
        <div>
          <button class="amount-minus btn btn-outline-secondary btn-sm" [disabled]="amount() <= 1" (click)="removeFromBasket.emit()">
            <i class="fa-solid fa-minus"></i>
          </button>
          {{amount()}}
          <button class="amount-minus btn btn-outline-secondary btn-sm" (click)="addToBasket.emit()">
            <i class="fa-solid fa-plus"></i>
          </button>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .basket-image {
        width: 120px;
        height: 120px;
        overflow: hidden;
        border-radius: 4px;
        margin-right: 10px;
      }
    `
  ]
})
export class BasketItemComponent implements OnInit {
  private productService = inject(ProductService);
  productImage = signal<string>('');
  product = input<Product>();
  amount = input<number>();
  addToBasket = output();
  removeFromBasket = output();
  removeAllFromBasket = output();

  ngOnInit() {
    this.productService.getProductImage(this.product().id).subscribe(imageUrl => {
      this.productImage.set(imageUrl);
    });
  }
}
