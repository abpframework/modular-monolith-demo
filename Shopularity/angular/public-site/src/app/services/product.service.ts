import {inject, Injectable} from "@angular/core";
import {Rest, RestService} from "@abp/ng.core";
import {PagedProducts, ProductWithCategory} from "../models";
import ResponseType = Rest.ResponseType;
import {map} from "rxjs";

@Injectable({
  providedIn: 'root'
})

export class ProductService {

  private restService = inject(RestService);
  apiName = 'ProductService';

  getProducts = (categoryName?: string) => {
    const url = categoryName ? `/api/catalog/public/products?categoryName=${categoryName}` : '/api/catalog/public/products';
    return this.restService.request<any, PagedProducts>({
        method: 'GET',
        url,
        params: {  },
      },
      { apiName: this.apiName});
  }


  getProductDetail = (productId: string) =>
    this.restService.request<any, ProductWithCategory>({
        method: 'GET',
        url: `/api/catalog/public/products/${productId}`,
        params: {  },
      },
      { apiName: this.apiName});

  getProductImage = (productId: string) =>
    this.restService.request<any, string>({
        method: 'GET',
        url: `/api/catalog/public/products/image-as-bytes/${productId}`,
        params: {  },
      },
      { apiName: this.apiName}).pipe(map(res => this.convertByteStringToDataUrl(res)));

  private convertByteStringToDataUrl(byteString: string, mimeType: string = 'image/jpeg'): string {
    if (byteString.startsWith('data:')) {
      return byteString;
    }
    return `data:${mimeType};base64,${byteString}`;
  }
}
