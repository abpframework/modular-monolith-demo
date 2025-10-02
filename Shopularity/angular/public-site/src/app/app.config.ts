import { provideAbpCore, withOptions } from '@abp/ng.core';
import { provideAbpOAuth } from '@abp/ng.oauth';
import { provideFeatureManagementConfig } from '@abp/ng.feature-management';
import { provideAbpThemeShared, withValidationBluePrint,withHttpErrorConfig } from '@abp/ng.theme.shared';
import { provideAccountPublicConfig } from '@volo/abp.ng.account/public/config';
import { provideOpeniddictproConfig } from '@volo/abp.ng.openiddictpro/config';
import { provideLogo, withEnvironmentOptions } from "@volo/ngx-lepton-x.core";
import { ApplicationConfig } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { environment } from '../environments/environment';
import { APP_ROUTES } from './app.routes';
import { APP_ROUTE_PROVIDER } from './route.provider';
import { provideClientHydration, withIncrementalHydration, withHttpTransferCacheOptions } from '@angular/platform-browser';
import {registerLocaleForEsBuild} from "@abp/ng.core/locale";
import { provideThemeBasicConfig } from '@abp/ng.theme.basic';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(APP_ROUTES),
    APP_ROUTE_PROVIDER,
    provideAnimations(),
    provideAbpCore(
      withOptions({
        environment,
        registerLocaleFn: registerLocaleForEsBuild(),
      }),
    ),
    provideAbpOAuth(),
    provideFeatureManagementConfig(),
    provideAccountPublicConfig(),
    provideAbpThemeShared(),
    provideThemeBasicConfig(),
    provideLogo(withEnvironmentOptions(environment)),
    provideOpeniddictproConfig(), provideClientHydration(withIncrementalHydration(), withHttpTransferCacheOptions({}))
  ]
};
