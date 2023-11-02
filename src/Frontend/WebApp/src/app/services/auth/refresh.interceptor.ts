import { HttpClient, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, Subscriber, finalize } from "rxjs";
import { IdentityService } from "../identity.service";
import { ILoginResponce } from "src/app/models/identity/login.responce";

type CallerRequest = {
    subscriber: Subscriber<any>;
    failedRequest: HttpRequest<any>;
};

@Injectable()
export class RefreshTokenInterceptor implements HttpInterceptor {
    private refreshInProgress: boolean;
    private requests: CallerRequest[] = [];

    constructor(private identity: IdentityService, private http: HttpClient) {
        this.refreshInProgress = false;
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let observable = new Observable<HttpEvent<any>>((subscriber) => {
            let originalRequestSubscription = next.handle(req)
                .subscribe({
                    next: (response) => {
                        subscriber.next(response);
                    },
                    error: (err) => {
                        if (err.status === 401) {
                            this.handleUnauthorizedError(subscriber, req);
                        } else {
                            subscriber.error(err);
                        }
                    },
                    complete: () => subscriber.complete()
                });

            return () => {
                originalRequestSubscription.unsubscribe();
            };
        });

        return observable;
    }

    private handleUnauthorizedError(subscriber: Subscriber<any>, request: HttpRequest<any>) {
        this.requests.push({ subscriber, failedRequest: request });
        if (!this.refreshInProgress) {
            this.refreshInProgress = true;
            this.identity.refreshToken()
                .pipe(
                    finalize(() => { this.refreshInProgress = false; })
                )
                .subscribe({
                    next: (authHeader) =>
                        this.repeatFailedRequests(authHeader),
                    error: () => {
                        this.identity.logout();
                    }
                });
        }
    }

    private repeatFailedRequests(authHeader: ILoginResponce) {
        this.requests.forEach((c) => {
            const requestWithNewToken = c.failedRequest.clone({
                headers: c.failedRequest.headers.set('Authorization', "Bearer " + authHeader.token)
            });
            
            this.repeatRequest(requestWithNewToken, c.subscriber);
        });
        this.requests = [];
    }

    private repeatRequest(requestWithNewToken: HttpRequest<any>, subscriber: Subscriber<any>) {
        this.http.request(requestWithNewToken).subscribe((res) => {
            subscriber.next(res);
        },
            (err) => {
                if (err.status === 401) {
                    this.identity.logout();
                }
                subscriber.error(err);
            },
            () => {
                subscriber.complete();
            });
    }
}