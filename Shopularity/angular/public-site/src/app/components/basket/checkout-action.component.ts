import {Component, input} from '@angular/core';
import {RouterLink} from "@angular/router";

@Component({
  selector: 'app-checkout-action',
  imports: [
    RouterLink
  ],
  template: `
    <div class="d-flex flex-column row-gap-2">
      <div class="d-flex align-items-center justify-content-between">
        <span>Total Price</span>
        <b>{{ totalPrice() }}</b>
      </div>
      <button class="btn btn-primary w-100" routerLink="/check-out" role="link">
        Proceed to check-out
      </button>
    </div>
  `
})

export class CheckoutActionComponent {
  totalPrice = input<string>('');
}
