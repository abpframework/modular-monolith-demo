import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44365/',
  redirectUri: baseUrl,
  clientId: 'Shopularity_Angular_Public',
  responseType: 'code',
  scope: 'offline_access Shopularity_Angular_Public',
  requireHttps: true,
  ssrAuthorizationUrl: '/authorize',
  impersonation: {
    userImpersonation: true,
  }
};

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'Shopularity',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44324',
      rootNamespace: 'Shopularity_Angular_Public',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
} as Environment;
