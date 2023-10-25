import { Component, Input } from '@angular/core'
import { IComment } from 'src/app/models/comment'

@Component({
    selector: 'app-comment-collection',
    templateUrl: './comment-collection.component.html'
})
export class CommentCollectionComponent {
    @Input()
    comments!: IComment[] | null
}