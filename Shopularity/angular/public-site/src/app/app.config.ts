import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import {
  provideClientHydration,
  withIncrementalHydration,
  withHttpTransferCacheOptions,
} from '@angular/platform-browser';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideAbpCore, withOptions } from '@abp/ng.core';
import { registerLocaleForEsBuild } from '@abp/ng.core/locale';
import { provideThemeBasicConfig } from '@abp/ng.theme.basic';
import { provideAbpOAuth } from '@abp/ng.oauth';
import { provideFeatureManagementConfig } from '@abp/ng.feature-management';
import { provideAbpThemeShared, provideLogo, withEnvironmentOptions } from '@abp/ng.theme.shared';
import { provideAccountPublicConfig } from '@volo/abp.ng.account/public/config';
import { provideOpeniddictproConfig } from '@volo/abp.ng.openiddictpro/config';
import { environment } from '../environments/environment';
import { APP_ROUTES } from './app.routes';
import { APP_ROUTE_PROVIDER } from './route.provider';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(APP_ROUTES),
    APP_ROUTE_PROVIDER,
    provideAnimations(),
    provideAbpCore(
      withOptions({
        environment,
        registerLocaleFn: registerLocaleForEsBuild(),
      })
    ),
    provideAbpOAuth(),
    provideFeatureManagementConfig(),
    provideAccountPublicConfig(),
    provideAbpThemeShared(),
    provideThemeBasicConfig(),
    provideLogo(withEnvironmentOptions(environment)),
    provideOpeniddictproConfig(),
    provideClientHydration(withIncrementalHydration(), withHttpTransferCacheOptions({})),
  ],
};
