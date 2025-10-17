import {Pipe, PipeTransform} from "@angular/core";
import {BasketItem} from "../models";

@Pipe({
  name: 'getTotalPrice'
})
export class GetTotalPricePipe implements PipeTransform {
  transform(products: BasketItem[]): number {
    if (!products || products.length === 0) {
      return 0;
    } else {
      const totalPrice = (products.map(p => p.product.price * p.amount).reduce((a, b) => a + b));
      return totalPrice;
    }
  }
}
