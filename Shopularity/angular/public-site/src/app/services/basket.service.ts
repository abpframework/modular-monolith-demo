import {RestService} from "@abp/ng.core";
import {inject, Injectable, signal} from "@angular/core";
import {catchError, Observable, of, tap} from "rxjs";
import {BasketItem, BasketItemResult} from "../models";
import {ToasterService} from "@abp/ng.theme.shared";

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  apiName = 'BasketService';
  readonly basketCount = signal<number>(0);
  basketItems = signal<BasketItem[]>([]);
  private restService = inject(RestService);
  private toasterService = inject(ToasterService);

  fetchBasketCount(): Observable<number> {
    return this.restService.request<void, number>(
      { method: 'GET', url: '/api/basket/basket/count' },
      { apiName: this.apiName }
    ).pipe(
      tap(count => this.basketCount.set(count)),
      catchError(err => {
        return of(0); // fallback
      })
    );
  }

  addToBasket = (itemId: string, amount: number) =>
    this.restService.request<any, number>({
        method: 'GET',
        url: `/api/basket/basket/add?itemId=${itemId}&amount=${amount}`,
        params: {  },
      },
      { apiName: this.apiName})
      .pipe(tap(res => this.toasterService.success("Added to Basket")))

  removeFromBasket = (itemId: string, amount: number) =>
    this.restService.request<any, number>({
        method: 'GET',
        url: `/api/basket/basket/remove?itemId=${itemId}&amount=${amount}`,
        params: {  },
      },
      { apiName: this.apiName})
      .pipe(tap(res => this.toasterService.success("Removed from Basket")))

  fetchBasket(): Observable<any> {
    return this.restService.request<void, BasketItemResult>(
      { method: 'GET', url: '/api/basket/basket' },
      { apiName: this.apiName }
    ).pipe(
      tap(res => {
        this.basketItems.set(res.items);
      }),
      catchError(err => {
        return of([]);
      })
    );
  }
}
