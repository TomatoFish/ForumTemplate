import { Component, HostBinding, Input } from '@angular/core'
import { IComment } from 'src/app/models/comment'

@Component({
    selector: 'app-comment',
    templateUrl: './comment.component.html'
})
export class CommentComponent {
    @Input()
    comment!: IComment

    @HostBinding("class._array")
    get isArray(): boolean {
        return this.comment.comments.length > 0;
    }

    constructor()
    {
    }
}