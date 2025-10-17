import {Component, DestroyRef, inject, OnInit} from '@angular/core';
import {CurrencyPipe} from "@angular/common";
import {PageComponent} from "@abp/ng.components/page";
import {CardComponent} from "@abp/ng.theme.shared";
import {ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators} from "@angular/forms";
import {takeUntilDestroyed} from "@angular/core/rxjs-interop";
import {Router, RouterModule} from "@angular/router";

import {CheckoutItemComponent} from "./checkout-item.component";
import {GetTotalPricePipe} from "../../pipes/get-total-price.pipe";
import {BasketService, OrderService} from "../../services";

@Component({
  selector: 'app-check-out',
  templateUrl: 'check-out.component.html',
  imports: [
    CurrencyPipe,
    PageComponent,
    CardComponent,
    CheckoutItemComponent,
    GetTotalPricePipe,
    ReactiveFormsModule,
    RouterModule
  ],
  styles: []
})
export class CheckOutComponent implements OnInit {
  readonly basketService = inject(BasketService);
  readonly orderService = inject(OrderService);
  readonly destroyRef = inject(DestroyRef);
  private router = inject(Router);
  formBuilder = inject(UntypedFormBuilder);
  paymentForm: UntypedFormGroup;

  constructor() {
    this.paymentForm = this.formBuilder.group({
      creditCardNumber: ['', Validators.required],
      address: ['', Validators.required]
    })
  }

  ngOnInit() {
    this.basketService.fetchBasket().pipe(takeUntilDestroyed(this.destroyRef)).subscribe();
  }

  createOrder() {
    this.paymentForm.markAllAsTouched();
    if (this.paymentForm.valid) {
      const postData = {
        products: this.basketService.basketItems()?.map(item => {
          return {productId: item.product.id, amount: item.amount}
        }),
        shippingAddress: this.address.value
      }
      this.orderService.createOrder(postData).subscribe(res => {
        this.basketService.fetchBasket().pipe(takeUntilDestroyed(this.destroyRef)).subscribe()
        this.router.navigateByUrl('/order-history');
      })
    }
  }

  get creditCardNumber() {
    return this.paymentForm.get('creditCardNumber');
  }

  get address() {
    return this.paymentForm.get('address');
  }
}
