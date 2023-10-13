import {Component, Input} from '@angular/core'
import { Router } from '@angular/router'
import { ITheme } from 'src/app/models/theme'

@Component({
    selector: 'app-theme',
    templateUrl: './theme.component.html'
})
export class ThemeComponent {
    @Input()
    theme!: ITheme

    constructor(private router: Router)
    {
    }

    submit()
    {
        this.router.navigate(['themes/'+ this.theme.id])
    }
}