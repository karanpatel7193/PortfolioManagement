import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SessionService } from 'src/app/services/session.service';

@Component({
  selector: 'app-auth-layout',
  templateUrl: './auth-layout.component.html',
  styleUrls: ['./auth-layout.component.scss']
})
export class AuthLayoutComponent implements OnInit {
    
  constructor(private sessionService: SessionService, private router: Router) {
    const currentUser = this.sessionService.getUser();
    const isAuthenticated = this.sessionService.isAuthenticated;
    if (isAuthenticated && currentUser != null && currentUser.token != null) {
      this.router.navigate(["/app/dashboard"]);
    }
  }

  ngOnInit(): void {
  }

}
