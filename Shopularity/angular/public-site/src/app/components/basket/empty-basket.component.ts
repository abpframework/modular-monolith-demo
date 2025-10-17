import {Component} from '@angular/core';

@Component({
  selector: 'app-empty-basket',
  template: `
    <div id="shopularityBasketEmpty" class="px-3 py-4 text-center text-muted d-flex flex-column gap-2 align-items-center justify-content-center">
      <i class="fa-solid fa-heart-crack fs-4"></i>
      <span>Your basket is empty.</span>
    </div>
  `
})

export class EmptyBasketComponent {}
