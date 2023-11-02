import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { IdentityService } from "../identity.service";

@Injectable()
export class ApplyTokenInterceptor implements HttpInterceptor {

    constructor(private identity: IdentityService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const idToken = this.identity.getToken();

        if (idToken) {
            const cloned = req.clone({
                headers: req.headers.set("Authorization", "Bearer " + idToken)
            });

            return next.handle(cloned);
        }
        else {
            return next.handle(req);
        }
    }
}