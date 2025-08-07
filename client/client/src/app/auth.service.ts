import { HttpClient, HttpHeaders } from '@angular/common/http';
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
    if (this.oauthService.hasValidAccessToken()) {
      const idToken = this.oauthService.getIdToken();
      console.log('ID Token:', idToken);

      const headers = new HttpHeaders().set('Authorization', `Bearer ${idToken}`);
      this.http.post('https://localhost:7240/api/auth/google-login', null, { headers }).subscribe({
        next: (res: any) => {
          console.log('Login successful', res);
          if (res.token)
            localStorage.setItem('token', res.token);
        }
      });
    }
  }

  logout() {
    this.http.post('https://localhost:7240/api/auth/logout', {}).subscribe(() => {
      this.oauthService.logOut();
    });
  }
}
