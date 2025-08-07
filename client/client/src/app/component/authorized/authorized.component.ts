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

  userName = '';
  ngOnInit() {
    this.userName = localStorage.getItem('userName')??''
  }

  logout() {
    this.authService.logout();
  }
}