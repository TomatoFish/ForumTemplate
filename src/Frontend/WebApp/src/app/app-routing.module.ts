import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ThemeCollectionComponent } from './components/themes/theme-collection/theme-collection.component';
import { PostDetailsComponent } from './components/posts/post-details/post-details.component';

const routes: Routes = [
  { path: "", redirectTo: "/themes/0", pathMatch: "full" },
  { path: "themes/:parentThemeId", component: ThemeCollectionComponent },
  { path: "post/:id", component: PostDetailsComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
