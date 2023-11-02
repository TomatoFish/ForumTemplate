import { Component } from '@angular/core';
import { IdentityService } from './services/identity.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'WebApp';

  constructor(private identity: IdentityService, private router: Router) {
  }

  isLogedIn(): boolean {
    return this.identity.isLoggedIn();
  }

  public submitLogout() {
    this.identity.logout();
  }
}
