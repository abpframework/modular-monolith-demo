import {Component, inject, OnInit} from "@angular/core";
import {CurrencyPipe} from "@angular/common";
import {AuthService} from "@abp/ng.core";
import {BasketService} from "../../services/basket.service";
import {EmptyBasketComponent} from "./empty-basket.component";
import {NgbDropdown, NgbDropdownMenu, NgbDropdownToggle} from "@ng-bootstrap/ng-bootstrap";
import {BasketItemComponent} from "./basket-item.component";
import {CheckoutActionComponent} from "./checkout-action.component";
import {GetTotalPricePipe} from "../../pipes/get-total-price.pipe";

@Component({
  selector: 'app-basket',
  imports: [
    EmptyBasketComponent,
    NgbDropdown,
    NgbDropdownMenu,
    NgbDropdownToggle,
    BasketItemComponent,
    CheckoutActionComponent,
    GetTotalPricePipe,
    CurrencyPipe
  ],
  template: `
      @if (authService.isAuthenticated) {
        <div ngbDropdown #drop="ngbDropdown"
             (openChange)="basketOpenChanged($event)"
             (mouseenter)='over(drop)'
             (mouseleave)='out(drop)'>

        <button
                class="btn btn-outline-secondary dropdown-toggle text-white border-0" data-toggle="dropdown"
                id="basketDropdown" ngbDropdownToggle>
          <i class="fa fa-cart-shopping"></i>
          @if (basketService.basketCount() > 0) {
            <span id="ShopularityBasketCount"
                  class="position-absolute start-100 translate-middle badge rounded-pill bg-danger"
                  style="min-width:20px;height:20px;line-height:20px;font-size:.75rem;padding:0 6px; top: 4px;">
                {{ basketService.basketCount() }}
            </span>
          }
        </button>
          <div ngbDropdownMenu aria-labelledby="basketDropdown" class="p-2 basket-content">
            <div class="d-flex flex-column">
              <div class="d-flex justify-content-between">
                <b>Basket</b>
                <span>{{basketService.basketCount()}} Product's</span>
              </div>
            </div>
            <hr />
            @if (basketService.basketCount() > 0) {
              @for (item of basketService.basketItems(); track item.product.id) {
                <app-basket-item [product]="item.product" [amount]="item.amount"
                                 (addToBasket)="addProductToBasket(item.product.id)"
                                 (removeAllFromBasket)="removeProductFromBasket(item.product.id, item.amount)"
                                 (removeFromBasket)="removeProductFromBasket(item.product.id, 1)"/>
                <hr />
              }
              <app-checkout-action [totalPrice]="basketService.basketItems() | getTotalPrice | currency" />
            } @else {
              <app-empty-basket/>
            }
          </div>
        </div>
      } @else {
        <a class="btn text-white" href="/authorize">
          <i class="fa fa-cart-shopping"></i>
        </a>
      }

  `,
  styles: [`
    .basket-content {
      width: 300px;
      max-height: 1000px;
    }
  `]
})
export class BasketComponent implements OnInit {
  readonly authService = inject(AuthService);
  readonly basketService = inject(BasketService);

  ngOnInit() {
    if (this.authService.isAuthenticated) {
      this.getBasketCount();
    }
  }

  getBasketCount() {
    this.basketService.fetchBasketCount().subscribe();
  }

  basketOpenChanged(isOpen: boolean) {
    if (isOpen) {
      this.basketService.fetchBasket().subscribe();
    }
  }

  addProductToBasket(productId: string) {
    this.basketService.addToBasket(productId, 1)
      .subscribe(res => {
        this.basketService.fetchBasketCount().subscribe();
        this.basketService.fetchBasket().subscribe();
      })
  }

  removeProductFromBasket(productId: string, amount: number) {
    this.basketService.removeFromBasket(productId, 1)
      .subscribe(res => {
        this.basketService.fetchBasketCount().subscribe();
        this.basketService.fetchBasket().subscribe();
      })
  }
  over(drop:NgbDropdown){
    drop.open()
  }
  out(drop:NgbDropdown){
    drop.close()
  }
}
