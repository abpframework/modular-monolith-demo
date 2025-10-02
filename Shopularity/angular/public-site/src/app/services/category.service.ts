import {inject, Injectable} from "@angular/core";
import {RestService} from "@abp/ng.core";
import {CategoryList} from "../models";

@Injectable({
  providedIn: 'root'
})

export class CategoryService {
  private restService = inject(RestService);
  apiName = 'CategoryService';

  getPublicCategories = () =>
    this.restService.request<any, CategoryList>({
        method: 'GET',
        url: '/api/catalog/public/categories',
        params: {  },
      },
      { apiName: this.apiName});
}
