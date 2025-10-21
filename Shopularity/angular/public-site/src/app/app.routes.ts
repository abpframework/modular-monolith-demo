import { Routes } from '@angular/router';
import {authGuard} from "@abp/ng.core";

export const APP_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () => import('./containers/product-list').then(c => c.ProductListComponent),
  },
  {
    path: 'product/:productId',
    loadComponent: () => import('./containers/product-detail').then(c => c.ProductDetailComponent),
  },
  {
    path: 'check-out',
    loadComponent: () => import('./containers/check-out').then(c => c.CheckOutComponent),
    canActivate: [authGuard],
  },
  {
    path: 'order-history',
    loadComponent: () => import('./containers/order-history').then(c => c.OrderHistoryComponent),
    canActivate: [authGuard],
  }
];
