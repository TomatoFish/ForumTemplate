import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ThemeCollectionComponent } from './components/themes/theme-collection/theme-collection.component';
import { ThemeComponent } from './components/themes/theme/theme.component';
import { HttpClientModule } from '@angular/common/http'
import { GlobalErrorComponent } from './components/global-error/global-error.component';
import { PostCollectionComponent } from './components/posts/post-collection/post-collection.component';
import { PostComponent } from './components/posts/post/post.component';
import { PostDetailsComponent } from './components/posts/post-details/post-details.component';

@NgModule({
  declarations: [
    AppComponent,
    ThemeCollectionComponent,
    ThemeComponent,
    PostCollectionComponent,
    PostComponent,
    PostDetailsComponent,
    GlobalErrorComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
