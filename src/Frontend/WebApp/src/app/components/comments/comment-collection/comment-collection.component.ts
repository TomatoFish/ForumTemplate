import { Component, Input, OnInit } from '@angular/core'
import { Observable } from 'rxjs/internal/Observable'
import { IComment } from 'src/app/models/comment'
import { CommentService } from 'src/app/services/comment.service'
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-comment-collection',
    templateUrl: './comment-collection.component.html'
})
export class CommentCollectionComponent implements OnInit {
    @Input()
    comments$!: Observable<IComment[]>

    constructor(private commentService: CommentService, private route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.route.paramMap.subscribe(params => {
            const postId = Number(params.get('id'))
            this.changeCommentHierarchy(postId)
        })
    }

    changeCommentHierarchy(postId: number): void {
        this.comments$ = this.commentService.getCommentsByPost(postId);
    }
}