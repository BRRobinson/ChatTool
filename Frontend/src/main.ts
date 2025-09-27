import { bootstrapApplication } from '@angular/platform-browser';
import { App } from './app/app';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { API_URL, appConfig } from './app/app.config';
import { environment } from './environments/environment';
import { provideToastr } from 'ngx-toastr';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';

bootstrapApplication(App, {
  ...appConfig,
  providers: [
    ...(appConfig.providers || []),
    { provide: API_URL, useValue: environment.apiUrl },
    provideRouter(routes),
    provideToastr(),
    provideAnimations(),
    provideHttpClient(withInterceptorsFromDi())
  ]
}).catch((err) => console.error(err));
