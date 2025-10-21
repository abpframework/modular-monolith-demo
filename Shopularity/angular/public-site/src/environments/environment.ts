import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44365/',
  redirectUri: baseUrl,
  clientId: 'Shopularity_Angular_Public',
  responseType: 'code',
  clientSecret: '1q2w3e*',
  scope: 'Shopularity',
  requireHttps: true,
  ssrAuthorizationUrl: '/authorize',
  impersonation: {
    userImpersonation: true,
  }
};

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'Shopularity',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44365',
      rootNamespace: 'Shopularity_Angular_Public',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
} as Environment;
