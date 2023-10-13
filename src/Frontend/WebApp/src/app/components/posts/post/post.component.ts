import {Component, Input} from '@angular/core'
import { Router } from '@angular/router'
import { IPost } from 'src/app/models/post'

@Component({
    selector: 'app-post',
    templateUrl: './post.component.html'
})
export class PostComponent {
    @Input()
    post!: IPost

    constructor(private router: Router)
    {
    }

    submit()
    {
        this.router.navigate(['post/'+ this.post.id])
    }
}