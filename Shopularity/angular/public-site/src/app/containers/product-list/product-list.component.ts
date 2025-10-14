import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {filter} from "rxjs";

import { AuthService } from '@abp/ng.core';
import {ProductService} from "../../services/product.service";
import {ProductWithCategory} from "../../models";
import {CategoryListComponent} from "../../components/category-list/category-list.component";
import {ProductListItemComponent} from "./product-list-item/product-list-item.component";

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss'],
  imports: [CategoryListComponent, ProductListItemComponent],
})
export class ProductListComponent implements OnInit {
  private authService = inject(AuthService);
  private productService = inject(ProductService);
  private activatedRoute = inject(ActivatedRoute);
  private router = inject(Router);
  public products: ProductWithCategory[] = [];

  ngOnInit() {
    this.getProducts();
    this.activatedRoute.queryParams
      .pipe(filter(params => params.category))
      .subscribe(params => {
        this.getProducts();
        }
      );
  }

  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated
  }

  login() {
    this.authService.navigateToLogin();
  }

  getCategoryProducts(category: string): void {
    this.router.navigate(
      ['.'],
      { queryParams: { category: category } }
    );
  }

  private getProducts(): void {
    const categoryName = this.activatedRoute.snapshot.queryParamMap.get('category');
    this.productService.getProducts(categoryName).subscribe(products => {
      this.products = products.items || [];
    })
  }
}
