import {Component, DestroyRef, inject, OnInit} from '@angular/core';
import {CurrencyPipe, DatePipe} from "@angular/common";
import {takeUntilDestroyed} from "@angular/core/rxjs-interop";

import {OrderService} from "../../services";
import {Order} from "../../models/order.model";
import {PageComponent} from "@abp/ng.components/page";
import {CardComponent} from "@abp/ng.theme.shared";
import {LocalizationPipe} from "@abp/ng.core";

@Component({
  selector: 'app-order-history',
  imports: [
    PageComponent,
    CardComponent,
    CurrencyPipe,
    LocalizationPipe,
    DatePipe
  ],
  templateUrl: './order-history.component.html'
})
export class OrderHistoryComponent implements OnInit {
  readonly orderService = inject(OrderService);
  private readonly destroyRef = inject(DestroyRef);
  orders: Order[] = [];

  ngOnInit() {
    this.getOrders();
  }

  cancelOrder(order: Order) {
    this.orderService.cancelOrder(order.id).pipe(takeUntilDestroyed(this.destroyRef)).subscribe(res => {
      this.getOrders();
    });
  }

  getOrders() {
    this.orderService.getOrders().pipe(takeUntilDestroyed(this.destroyRef)).subscribe(res => {
      this.orders = res?.items || [];
    });
  }
}
