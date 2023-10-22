import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpParams, HttpResponse } from '@angular/common/http'
import { Observable, catchError, retry, throwError } from "rxjs";
import { IComment } from "../models/comment";
import { ErrorService } from "./error.service";

@Injectable({
    providedIn: 'root'
})
export class CommentService 
{
    url: string = "http://localhost:8010/comment"

    constructor(private http: HttpClient, private errorService: ErrorService)
    {
    }

    getAllComments() : Observable<IComment[]>
    {
        return this.http.get<IComment[]>(this.url);
    }

    getCommentsByPost(postId: number) : Observable<IComment[]>
    {
        return this.http.get<IComment[]>(this.url, {
            params: new HttpParams({
                fromObject: {themeId: postId}
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