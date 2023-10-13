import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpParams, HttpResponse } from '@angular/common/http'
import { Observable, catchError, retry, throwError } from "rxjs";
import { IPost } from "../models/post";
import { ErrorService } from "./error.service";

@Injectable({
    providedIn: 'root'
})
export class PostService 
{
    url: string = "http://localhost:5152/post"

    constructor(private http: HttpClient, private errorService: ErrorService)
    {
    }

    getAllPosts() : Observable<IPost[]>
    {
        return this.http.get<IPost[]>(this.url);
    }

    getPostsByTheme(themeId: number) : Observable<IPost[]>
    {
        return this.http.get<IPost[]>(this.url, {
            params: new HttpParams({
                fromObject: {themeId: themeId}
            })
        }).pipe(
            retry(),
            catchError(this.errorHandler.bind(this))
        )
    }

    getPostsById(id: number) : Observable<IPost[]>
    {
        return this.http.get<IPost[]>(this.url, {
            params: new HttpParams({
                fromObject: {postId: id}
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