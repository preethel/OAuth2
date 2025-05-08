import { Routes } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { AuthCallbackComponent } from './component/auth-callback/auth-callback.component';
import { AuthorizedComponent } from './component/authorized/authorized.component';
import { LoginComponent } from './component/login/login.component';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'authorized', component: AuthorizedComponent, canActivate: [AuthGuard] },
    { path: 'auth-callback', component: AuthCallbackComponent },
];
