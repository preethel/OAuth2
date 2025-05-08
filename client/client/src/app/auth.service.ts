import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { authConfig } from './auth.config';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private oauthService: OAuthService, private http: HttpClient) {
    this.oauthService.configure(authConfig);
    this.oauthService.loadDiscoveryDocumentAndTryLogin();
  }

  login() {
    this.oauthService.initLoginFlow();
  }

  async handleLogin() {
    debugger
    if (this.oauthService.hasValidAccessToken()) {
      const idToken = this.oauthService.getIdToken();
      await this.http.post('https://localhost:7240/api/auth/google-login', { idToken }).toPromise();
    }
  }

  logout() {
    this.http.post('https://localhost:7240/api/auth/logout', {}).subscribe(() => {
      this.oauthService.logOut();
    });
  }
}
