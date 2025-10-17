import {Component, inject} from '@angular/core';
import { DynamicLayoutComponent } from '@abp/ng.core';
import {LoaderBarComponent, NavItemsService, UserMenuService} from '@abp/ng.theme.shared';
import {BasketComponent} from "./components";
import {Router} from "@angular/router";

@Component({
  selector: 'app-root',
  template: `
    <abp-loader-bar />
    <abp-dynamic-layout />
  `,
  imports: [LoaderBarComponent, DynamicLayoutComponent],
})
export class AppComponent {
  private navItemsService = inject(NavItemsService);
  private userMenuService = inject(UserMenuService);
  private router = inject(Router);

  constructor() {
    this.navItemsService.addItems([
      {
        name: '::Menu:Basket',
        order: 100,
        component: BasketComponent
      }
    ])

    this.userMenuService.addItems([
      {
        id: 'order-history',
        order: 100,
        textTemplate: {
          text: 'Order History',
          icon: 'fa fa-cart-shopping',
        },
        action: () => {
          this.router.navigateByUrl('/order-history');
        },
      },
    ])
  }
}
