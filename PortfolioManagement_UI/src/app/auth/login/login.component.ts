import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router'
import { UserModel } from 'src/app/pages/account/user/user.model';
import { UserService } from 'src/app/pages/account/user/user.service';
import { SessionService } from 'src/app/services/session.service';
import { ToastService } from 'src/app/services/toast.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: [
        './login.component.scss'
    ]
})
export class LoginComponent implements OnInit {
    public user: UserModel = new UserModel();
    public createUserModel: UserModel = new UserModel();

    constructor(private router: Router,
        private userService: UserService,
        private session: SessionService,
        private toaster: ToastService,
    ) {
    }

    ngOnInit() {
        //this.themeService.getJsTheme()
        //    .subscribe((theme: NbJSThemeOptions) => this.theme = theme);
    }

    public login(IsValid: boolean): any {
        if (IsValid) {
            this.userService.validateLogin(this.user).subscribe(data => {
                if (data != null && data.id != 0) {
                    this.session.setUser(data);
                    //this.broadcaster.broadcast('OnLoggedIn', 'Success');
                    this.router.navigate(['../app/dashboard'])
                        .then(success => {
                            if (success) {
                                console.log('Navigation is successful!');
                            } else {
                                console.error('Navigation has failed!');
                            }
                        });
                }
                else {
                    this.toaster.error('Username or Password is incorrect.', 'Incorrect Credentials')
                    this.session.destroy();
                }
            })
        }
        else {
            this.toaster.warning('Provide required fields.');
        }
    }

    showPassword: { password: 'password' | 'text' } = { password: 'password' };

    togglePassword(field: 'password'): void {
        this.showPassword[field] = this.showPassword[field] === 'password' ? 'text' : 'password';
    }
}
