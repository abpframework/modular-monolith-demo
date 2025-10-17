import {inject, Injectable} from '@angular/core';
import {RestService} from "@abp/ng.core";
import {CreateOrder, OrderList} from "../models/order.model";
import {ToasterService} from "@abp/ng.theme.shared";
import {tap} from "rxjs";

@Injectable({
  providedIn: 'root'
})

export class OrderService {
  private restService = inject(RestService);
  private toasterService = inject(ToasterService);
  apiName = 'OrderService';

  getOrders = () =>
    this.restService.request<any, OrderList>({
        method: 'GET',
        url: '/api/ordering/public/orders',
        params: {  },
      },
      { apiName: this.apiName});

  createOrder = (orderInfo: CreateOrder) =>
    this.restService.request<any, CreateOrder>({
        method: 'POST',
        url: '/api/ordering/public/orders/create',
        params: {  },
        body: orderInfo
      },
      { apiName: this.apiName}).pipe(tap(res => this.toasterService.success("Order created successfully")));

  cancelOrder = (orderId: string) =>
    this.restService.request<any, any>({
        method: 'POST',
        url: `/api/ordering/public/orders/cancel?id=${orderId}`,
        params: {  },
      },
      { apiName: this.apiName}).pipe(tap(res => this.toasterService.success("Order cancelled successfully")));
}
