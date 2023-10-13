import {Component, OnInit} from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { Observable } from 'rxjs'
import { IComment } from 'src/app/models/comment'
import { IPost } from 'src/app/models/post'
import { PostService } from 'src/app/services/posts.service'

@Component({
    selector: 'app-post-details',
    templateUrl: './post-details.component.html'
})
export class PostDetailsComponent implements OnInit {
    post!: IPost
    comments$!: Observable<IComment[]>

    constructor(private postService: PostService, private route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.route.paramMap.subscribe(params => {
            const postId = Number(params.get('id'))
            this.showPost(postId)
        })
    }

    showPost(id: number): void {
        this.postService.getPostsById(id).subscribe(posts => {
            this.post = posts[0]
        });
    }
}