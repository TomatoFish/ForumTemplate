import { Component, OnInit } from '@angular/core'
import { Observable } from 'rxjs/internal/Observable'
import { IPost } from 'src/app/models/post'
import { PostService } from 'src/app/services/posts.service'
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-post-collection',
    templateUrl: './post-collection.component.html'
})
export class PostCollectionComponent implements OnInit {
    posts$!: Observable<IPost[]>

    constructor(private postService: PostService, private route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.route.paramMap.subscribe(params => {
            const themeId = Number(params.get('parentThemeId'))
            this.changeThemeHierarchy(themeId)
        })
    }

    changeThemeHierarchy(themeId: number): void {
        this.posts$ = this.postService.getPostsByTheme(themeId);
    }
}