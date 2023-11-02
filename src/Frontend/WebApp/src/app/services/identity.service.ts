import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http'
import { EMPTY, Observable, catchError, finalize, retry, shareReplay, tap, throwError } from "rxjs";
import { ErrorService } from "./error.service";
import { ILoginResponce } from "../models/identity/login.responce";
import { IUser } from "../models/identity/user";
import { IRegistration } from "../models/identity/registration";
import { ILoginRequest } from "../models/identity/login.request";
import * as moment from "moment";
import { Router } from "@angular/router";

@Injectable({
    providedIn: 'root'
})
export class IdentityService
{
    url: string = "http://localhost:8040/auth"
    registerTemplate: string = "/register"
    loginTemplate: string = "/login"
    refreshTemplate: string = "/refreshtoken"
    revokeTemplate: string = "/revoketoken"
    userTemplate: string = "/user"

    constructor(private http: HttpClient, private router: Router, private errorService: ErrorService)
    {
    }

    public registration(registration: IRegistration)
    {
        return this.http.post<ILoginResponce>(this.url + this.registerTemplate, registration)
        // this is just the HTTP call, 
        // we still need to handle the reception of the token
        .pipe(
            tap(res => this.setSession(res)),
            shareReplay()
        );
    }

    public login(login: ILoginRequest)
    {
        console.info(login);
        var headers = new HttpHeaders({
            "Content-Type": "application/json",
            "Accept": "*/*"
        });
        return this.http.post<ILoginResponce>(this.url + this.loginTemplate, login, {headers: headers})
        // this is just the HTTP call, 
        // we still need to handle the reception of the token
        .pipe(
            tap(res => this.setSession(res)),
            shareReplay()
        );
    }

    public getAllUsers() : Observable<IUser[]>
    {
        return this.http.get<IUser[]>(this.url + this.userTemplate);
    }

    public getUserById(id: number) : Observable<IUser[]>
    {
        return this.http.get<IUser[]>(this.url + this.userTemplate, {
            params: new HttpParams({
                fromObject: {id: id}
            })
        }).pipe(
            retry(),
            catchError(this.errorHandler.bind(this))
        )
    }

    private setSession(loginResponce: ILoginResponce) {
        const expirationTime = moment(loginResponce.expirationTime).utc();

        localStorage.setItem('id_token', loginResponce.token);
        localStorage.setItem('refresh_token', loginResponce.refreshToken);
        localStorage.setItem("expiration_time", JSON.stringify(expirationTime.valueOf()) );
    }  

    public logout() {
        this.http.post(this.url + this.revokeTemplate, "", {})
        .pipe(
            catchError(this.errorHandler.bind(this))
        )
        .subscribe(
            () => {
                localStorage.removeItem("id_token");
                localStorage.removeItem('refresh_token');
                localStorage.removeItem("expiration_time");

                this.router.navigateByUrl('/');
            }
        );
    }

    public isLoggedIn() {
        return moment().isBefore(this.getExpiration());
    }

    public getToken() {
        return localStorage.getItem("id_token");
    }

    public getExpiration() {
        const expiration = localStorage.getItem("expiration_time");

        if (!expiration) {
            return null;
        }

        const expiresAt = JSON.parse(expiration);
        return moment(expiresAt);
    }

    public refreshToken(): Observable<ILoginResponce> {
        if (!this.isNeedRefreshToken()) {
            return EMPTY;
        }

        return this.http.post<ILoginResponce>(this.url + this.refreshTemplate, {
            refreshToken: localStorage.getItem('refresh_token')
        })
        .pipe(
            tap((res) => this.setSession(res)),
            catchError(this.errorHandler.bind(this))
        );
    }

    public isNeedRefreshToken(): boolean {
        const expiresAt = this.getExpiration();

        if (!expiresAt)
        {
            return false;
        }
        
        let isExpireInMinute = moment().utc() > expiresAt.subtract(1, 'm');
        return isExpireInMinute;
    }

    private errorHandler(error: HttpErrorResponse)
    {
        this.errorService.handle(error.message)
        return throwError(() => error.message)
    }
}