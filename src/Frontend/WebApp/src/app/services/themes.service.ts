import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpParams, HttpResponse } from '@angular/common/http'
import { Observable, catchError, retry, throwError } from "rxjs";
import { ITheme } from "../models/theme";
import { ErrorService } from "./error.service";

@Injectable({
    providedIn: 'root'
})
export class ThemeService 
{
    url: string = "http://localhost:5247/theme"

    constructor(private http: HttpClient, private errorService: ErrorService)
    {
    }

    getAllThemes() : Observable<ITheme[]>
    {
        return this.http.get<ITheme[]>(this.url);
    }

    getThemesByParentTheme(perentTheme: number) : Observable<ITheme[]>
    {
        return this.http.get<ITheme[]>(this.url, {
            params: new HttpParams({
                fromObject: {parentThemeId: perentTheme}
            })
        }).pipe(
            retry(),
            catchError(this.errorHandler.bind(this))
        )
    }

    private errorHandler(error: HttpErrorResponse)
    {
        this.errorService.handle(error.message)
        return throwError(() => error.message)
    }
}