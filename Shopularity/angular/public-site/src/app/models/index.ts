import {ImageLoaderConfig} from "@angular/common";

type UUID = string;

export interface Product {
  id: UUID;
  name: string;
  description: string;
  price: number;
  stockCount: number;
  categoryId: UUID;
}

export interface Category {
  id: UUID;
  name: string;
}

export interface ProductWithCategory {
  product: Product;
  category: Category;
}

export interface BasketItem {
  product: Product;
  amount: number;
}

export interface PagedResult<T> {
  totalCount: number;
  items: T[];
}

export interface CustomImageLoaderConfig extends ImageLoaderConfig {
  productId?: string;
}

export type PagedProducts = PagedResult<ProductWithCategory>;
export type CategoryList = Omit<PagedResult<Category>, 'totalCount'>
export type BasketItemResult = Omit<PagedResult<BasketItem>, 'totalCount'>;
