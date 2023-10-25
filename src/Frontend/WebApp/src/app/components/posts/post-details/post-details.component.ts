import {Component, OnInit} from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { Observable } from 'rxjs'
import { IComment } from 'src/app/models/comment'
import { IPost } from 'src/app/models/post'
import { CommentService } from 'src/app/services/comment.service'
import { PostService } from 'src/app/services/posts.service'

@Component({
    selector: 'app-post-details',
    templateUrl: './post-details.component.html'
})
export class PostDetailsComponent implements OnInit {
    post!: IPost
    comments$!: Observable<IComment[]>

    constructor(private postService: PostService, private commentService: CommentService, private route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.route.paramMap.subscribe(params => {
            const postId = Number(params.get('id'))
            this.showPost(postId)
            this.changeCommentHierarchy(postId)
        })
    }

    showPost(id: number): void {
        this.postService.getPostsById(id).subscribe(posts => {
            this.post = posts[0]
        });
    }

    changeCommentHierarchy(postId: number): void {
        this.comments$ = this.commentService.getCommentsByPost(postId);
    }
}