import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {UserLoginModel, UserModel } from 'src/app/pages/account/user/user.model';
import { UserService } from 'src/app/pages/account/user/user.service';
import { SessionService } from 'src/app/services/session.service';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  public currentUser: UserLoginModel = new UserLoginModel();
  // @Output() closeModal = new EventEmitter<void>();
  public user: UserModel = new UserModel();

  constructor(private userService: UserService, private sessionService: SessionService,
    private router: Router,
    private route: ActivatedRoute,
    private toaster: ToastService) {
  }
  ngOnInit(): void {
    this.currentUser = this.sessionService.getUser();
  }
  public saveProfile(): void {
    if (this.currentUser) {
      this.userService.userUpadte(this.currentUser).subscribe(
        (response) => {
          this.toaster.success('Profile updated successfully!');
          this.router.navigate(['auth/login'])
        },
        (error) => {
          this.toaster.warning('Failed to update profile.');
        }
      );
    }
  }

  // close() {
  //   this.closeModal.emit();
  // }

  public cancel(): void {
    this.router.navigate(['auth/profile'])
  }
}

