# Angular Shopularity Public Site

### Introduction

This is the Angular SSR public storefront for the Shopularity modular monolith demo. It consumes the Shopularity backend APIs and authentication endpoints exposed by the .NET applications in the repository.

Use this project when you want to run the optional Angular public website instead of, or alongside, the MVC/Razor public website.

### Pre-requirements

- [Node.js 22 LTS](https://nodejs.org/en) (Node.js 20.19+ is also supported by Angular 21)
- [Yarn](https://classic.yarnpkg.com/)
- Running Shopularity backend services for local API calls

### Initial setup

Before running this Angular app, initialize the root solution:

1. Open the root solution in ABP Studio.
2. Go to **Solution Runner > Tasks**.
3. Run **Initialize Solution** if it has not already run.

The initialization task migrates the database, installs client-side libraries, and generates the OpenIddict development signing certificate.

You can also run the same setup manually from the repository root:

```bash
./initialize-project.ps1
```

### How to run with ABP Studio

1. Start **Shopularity.Admin** from ABP Studio Solution Runner.
2. Start **Shopularity.Public** if you also want the MVC/Razor public website.
3. Start this Angular application from a terminal:

```bash
yarn
yarn start
```

The Angular public site will be available at `http://localhost:4200/`.

### How to run manually

1. Navigate to the repository main directory and run these commands one by one:
  - `initialize-project.ps1`
  - `run-apps.ps1`
  after that, the backend applications will be available locally.
2. Navigate to the `Shopularity/angular/public-site` directory and run:
  - `yarn`
  - `yarn start`
  after that, the application will be available at `http://localhost:4200/`.

### Note

- Admin application is located at `https://localhost:44321/`.
- Public site application is located at `http://localhost:4200/`.
- You can login to the admin application with the following credentials:
  - USERNAME: `admin`
  - PASSWORD: `1q2w3E*`

### Additional resources

- [ABP Angular UI](https://abp.io/docs/latest/framework/ui/angular)
- [ABP Studio Solution Runner](https://abp.io/docs/latest/studio/running-applications)
- [Modular Monolith Solution Template](https://abp.io/docs/latest/solution-templates/modular-monolith)
- [Shopularity Modular Monolith Demo](https://github.com/abpframework/modular-monolith-demo)
