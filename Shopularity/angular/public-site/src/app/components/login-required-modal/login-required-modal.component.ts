import {Component, inject, model} from "@angular/core";
import {ButtonComponent, ModalComponent} from "@abp/ng.theme.shared";
import {AuthService} from "@abp/ng.core";

@Component({
  selector: 'app-login-required-modal',
  imports: [
    ButtonComponent,
    ModalComponent
  ],
  template: `
    <abp-modal [visible]="visible()" (visibleChange)="visible.set($event)">
      <ng-template #abpHeader>
        <h3>Information</h3>
      </ng-template>

      <ng-template #abpBody>
        You need to login to continue.
      </ng-template>

      <ng-template #abpFooter>
        <abp-button iconClass="fa fa-check" (click)="navToLogin()">OK</abp-button>
      </ng-template>
    </abp-modal>
  `
})

export class LoginRequiredModalComponent {
  visible = model<boolean>(false);
  private authService = inject(AuthService);

  navToLogin() {
    this.authService.navigateToLogin();
  }
}
