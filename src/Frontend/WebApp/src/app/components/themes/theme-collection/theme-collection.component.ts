import { Component, OnInit } from '@angular/core'
import { Observable } from 'rxjs/internal/Observable'
import { ITheme } from 'src/app/models/theme'
import { ThemeService } from 'src/app/services/themes.service'
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-theme-collection',
    templateUrl: './theme-collection.component.html'
})
export class ThemeCollectionComponent implements OnInit {
    themes$!: Observable<ITheme[]>

    constructor(private themeService: ThemeService, private route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.route.paramMap.subscribe(params => {
            const parentThemeId = Number(params.get('parentThemeId'))
            this.changeThemeHierarchy(parentThemeId)
        })
    }

    changeThemeHierarchy(targetThemeId: number): void {
        this.themes$ = this.themeService.getThemesByParentTheme(targetThemeId);
    }
}