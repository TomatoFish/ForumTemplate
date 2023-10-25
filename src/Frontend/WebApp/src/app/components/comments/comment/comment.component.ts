import { Component, HostBinding, Input } from '@angular/core'
import { IComment } from 'src/app/models/comment'

@Component({
    selector: 'app-comment',
    templateUrl: './comment.component.html'
})
export class CommentComponent {
    @Input()
    comment!: IComment

    get haveChilds(): boolean {
        return this.comment.comments.length > 0;
    }
}