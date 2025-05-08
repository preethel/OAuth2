import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { AuthService } from '../../auth.service';

@Component({
  selector: 'app-auth-callback',
  imports: [],
  templateUrl: './auth-callback.component.html',
  styleUrl: './auth-callback.component.scss'
})
export class AuthCallbackComponent implements OnInit {
  constructor(
    private oauthService: OAuthService,
    private authService: AuthService,
    private router: Router
  ) {}

  async ngOnInit() {
    await this.oauthService.loadDiscoveryDocumentAndTryLogin();

    if (this.oauthService.hasValidAccessToken()) {
      await this.authService.handleLogin();
      this.router.navigate(['/']);
    } else {
      this.router.navigate(['/login']);
    }
  }
}
