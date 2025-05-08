import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../auth.service';

@Component({
  selector: 'app-authorized',
  imports: [],
  templateUrl: './authorized.component.html',
  styleUrl: './authorized.component.scss'
})
export class AuthorizedComponent implements OnInit {
  constructor(private authService: AuthService) {}

  ngOnInit() {
    // Here you could call backend to fetch more user info if needed
  }

  logout() {
    this.authService.logout();
  }
}