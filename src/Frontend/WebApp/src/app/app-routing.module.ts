import { NgModule, inject } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ThemeCollectionComponent } from './components/themes/theme-collection/theme-collection.component';
import { PostDetailsComponent } from './components/posts/post-details/post-details.component';
import { RegistrationComponent } from './components/identity/registration/registration.component';
import { LoginComponent } from './components/identity/login/login.component';
import { IdentityService } from './services/identity.service';

const routes: Routes = [
  { path: "", redirectTo: "/themes/0", pathMatch: "full" },
  { path: "themes/:parentThemeId", component: ThemeCollectionComponent },
  { path: "post/:id", component: PostDetailsComponent },
  { path: "register", component: RegistrationComponent, canActivate: [() => !inject(IdentityService).isLoggedIn()] },
  { path: "login", component: LoginComponent, canActivate: [() => !inject(IdentityService).isLoggedIn()] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
