import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { APP_INITIALIZER, NgModule } from "@angular/core";
import { Observable, Subject, catchError, finalize, lastValueFrom, throwError } from "rxjs";
import { IdentityService } from "./services/identity.service";
import { ApplyTokenInterceptor } from "./services/auth/apply.interceptor";
import { RefreshTokenInterceptor } from "./services/auth/refresh.interceptor";

@NgModule({
  providers: [
    IdentityService,
    {
      provide: APP_INITIALIZER,
      useFactory: refreshToken,
      deps: [IdentityService],
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ApplyTokenInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: RefreshTokenInterceptor,
      multi: true
    }
  ],
  exports: [HttpClientModule]
})

export class CoreModule {
}

export function refreshToken(identity: IdentityService) {
  return () => {
    const subj = new Subject();
    identity.refreshToken()
      .pipe(
        finalize(() => {
          subj.complete();
        }),
        catchError((err, caught: Observable<any>) => {
          identity.logout();
          return throwError(() => new Error(err));
        })
      )
      .subscribe();
      
    return lastValueFrom(subj, { defaultValue: null });
  };
}